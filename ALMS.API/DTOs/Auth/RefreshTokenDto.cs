using System.ComponentModel.DataAnnotations;

namespace ALMS.API.DTOs.Auth
{
    public class RefreshTokenDto
    {
        [Required]
        public required string RefreshToken { get; set; }
    }
}
