using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ALMS.API.Core
{
    public class JWTService
    {
        private readonly IConfiguration _configuration;
        private readonly string _issuer;
        private readonly List<string> _audiences;
        private readonly string _key;
        private readonly int _accessTokenExpiryMinutes;
        private readonly int _refreshTokenExpiryMinutes;

        public JWTService(IConfiguration configuration)
        {
            _configuration = configuration;
            _issuer = _configuration["Jwt:Issuer"] ?? throw new ApplicationException("JWT Issuer is not configured");
            _key = _configuration["Jwt:Key"] ?? throw new ApplicationException("JWT key is not configured");
            _accessTokenExpiryMinutes = int.Parse(_configuration["Jwt:AccessTokenExpiryMinutes"] ?? "30");
            _refreshTokenExpiryMinutes = int.Parse(_configuration["Jwt:RefreshTokenExpiryMinutes"] ?? "1440"); // Default 1 day

            // Retrieve audiences from configuration
            _audiences = _configuration.GetSection("Jwt:Audiences").Get<List<string>>() ??
                         throw new ApplicationException("JWT Audiences are not configured");
        }

        public (string AccessToken, string RefreshToken) GenerateTokens(IEnumerable<Claim> claims)
        {
            var accessToken = GenerateAccessToken(claims);
            var refreshToken = GenerateRefreshToken();
            return (AccessToken: accessToken, RefreshToken: refreshToken);
        }

        private string GenerateAccessToken(IEnumerable<Claim> claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audiences.First(), // Default to the first audience
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_accessTokenExpiryMinutes),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private static string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public ClaimsPrincipal? ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = GetValidationParameters();

            try
            {
                var principal = tokenHandler.ValidateToken(token, validationParameters, out _);
                return principal;
            }
            catch
            {
                return null;
            }
        }

        public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = GetValidationParameters(validateLifetime: false);

            try
            {
                var principal = tokenHandler.ValidateToken(token, validationParameters, out var securityToken);
                if (securityToken is not JwtSecurityToken jwtToken || !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    throw new SecurityTokenException("Invalid token");
                }

                return principal;
            }
            catch
            {
                return null;
            }
        }

        public bool ValidateRefreshToken(string token, string storedToken)
        {
            return token == storedToken; // This would be more secure if hashed or encrypted
        }

        private TokenValidationParameters GetValidationParameters(bool validateLifetime = true)
        {
            return new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = _issuer,
                ValidateAudience = true,
                ValidAudiences = _audiences, // Use the list of audiences here
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key)),
                ValidateLifetime = validateLifetime,
                ClockSkew = TimeSpan.Zero
            };
        }
    }
}
