using ALMS.API.Core;
using ALMS.API.Core.Constants;
using ALMS.API.Data;
using ALMS.API.Data.Models;
using ALMS.API.DTOs.Auth;
using ALMS.API.DTOs.Users;
using ALMS.API.Helpers;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ALMS.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("auth")]
    public class AuthenticationController(IMapper mapper, UserManager<ApplicationUser> userManager, IUserStore<ApplicationUser> userStore, RoleManager<IdentityRole> roleManager, JWTService jwtService, RandomStringGenerator randomStringGenerator, ApplicationDbContext dbContext) : ControllerBase
    {
        private readonly IMapper _mapper = mapper;
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IUserStore<ApplicationUser> _userStore = userStore;
        private readonly IUserEmailStore<ApplicationUser> _userEmailStore = (IUserEmailStore<ApplicationUser>)userStore;
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;
        private readonly JWTService _jwtService = jwtService;
        private readonly RandomStringGenerator _randomStringGenerator = randomStringGenerator;
        private readonly ApplicationDbContext _dbContext = dbContext;

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<IdentityResult>> Register([FromBody] CreateUserDto createUserDto)
        {
            var userToCreate = _mapper.Map<ApplicationUser>(createUserDto);

            await _userStore.SetUserNameAsync(userToCreate, userToCreate.Email, CancellationToken.None);
            await _userEmailStore.SetEmailAsync(userToCreate, userToCreate.Email, CancellationToken.None);

            var result = await _userManager.CreateAsync(userToCreate, createUserDto.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result);
            }

            return Ok(new {id = userToCreate.Id });
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<object>> Login([FromBody] LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user is null || !user.IsApproved)
            {
                return Unauthorized("User does not exist or is not approved yet, please ask for approval");
            }
      
            var isValid = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!isValid)
            {
                return Unauthorized();
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            var token = _jwtService.GenerateToken(user, userRoles);

            return Ok(new
            {
                accessToken = token
            });
        }

        [Authorize(Roles = UserRoles.BranchLibarian)]
        [HttpPost("users/{id}/approve")]
        public async Task<ActionResult> ApproveUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user is null)
            {
                return NotFound("User dose not exist.");
            }

            if (user.IsApproved)
            {
                return BadRequest("User is already approved");
            }

            user.IsApproved = true;

            await _userStore.UpdateAsync(user, CancellationToken.None);

            await _userManager.AddToRoleAsync(user, UserRoles.LibaryMember);

            return Ok();
        }

        [HttpPost("users/{id}/deny")]
        public async Task<ActionResult> DenyUser(string id)
        {
            var userToDeny = await _userManager.FindByIdAsync(id);
            if (userToDeny is null) return NotFound("User not found");

            await _userManager.DeleteAsync(userToDeny);

            return NoContent();
        }

        [Authorize(Roles = UserRoles.BranchLibarian)]
        [HttpGet("users/un-approved")]
        public async Task<ActionResult> GetUnApprovedUsers()
        {
            var users = await _userManager.Users.Where(u => u.IsApproved == false).Select(x => new { x.Id, x.UserName, x.Email, x.FirstName, x.LastName }).ToListAsync();

            return Ok(users);
        }

        [Authorize(Roles = UserRoles.BranchLibarian)]
        [HttpPost("users/{id}/roles")]
        public async Task<ActionResult<List<IdentityRole>>> AddRoles([FromBody] IEnumerable<string> Roles, string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user is null)
            {
                return NotFound("User not found");
            }

            foreach (var role in Roles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    return BadRequest($"{role} dose not exist in database.");
                }
            }

            await _userManager.AddToRolesAsync(user, Roles);

            return Ok();
        }

        [HttpGet("roles")]
        public async Task<ActionResult> GetRoles()
        {
            var rolesQuery  = _roleManager.Roles;

            var rolesInTheSystem = await rolesQuery.ToListAsync();

            return Ok(rolesInTheSystem);
        }

        [AllowAnonymous]
        [HttpPost("forgot-password")]
        public async Task<ActionResult> ForgotPassword([FromBody] ForgotPasswordDto forgotPasswordDto)
        {

            var user = await _userManager.FindByEmailAsync(forgotPasswordDto.Email);
            if (user is null) return Ok(); // we don't reveal if the user exists

            var cantMakeAttempt = await _dbContext.ForgotPasswordAttempts
                .AnyAsync(x => x.UserId == user.Id &&
                           x.IsSuccessful == false &&
                           x.CreatedAt > DateTime.UtcNow.AddMinutes(-30));

            if (cantMakeAttempt) return Ok(); // Already made an attempt within 30 minutes so we do not send a new one.

            var code = _randomStringGenerator.Generate(20);

            var attempt = new ForgotPasswordAttempt
            {
                UserId = user.Id,
                Code = code,
            };

            var emailService = new EmailService();
          
            emailService.SendEmail(user.Email!, "Forgot password", $"http://localhost:5173/forgot-password/{code}");

            // they will go there and put there new password and confirm
            // they will send the code, and new password
            // we will check code is valid and new password and then change the password


            _dbContext.ForgotPasswordAttempts.Add(attempt);
            await _dbContext.SaveChangesAsync();

            return Ok(code);
        }

        [AllowAnonymous]
        [HttpPost("forgot-password/{code}")]
        public async Task<ActionResult> ConfirmForgotPassword([FromBody] ConfirmForgotPasswordDto confirmForgotPasswordDto, string code)
        {
            var attempt = await _dbContext.ForgotPasswordAttempts.Where(x => x.Code == code).FirstOrDefaultAsync();

            if (attempt is null) return NotFound("Attempt not found.");

            if (DateTime.UtcNow - attempt.CreatedAt > TimeSpan.FromMinutes(30))
                return BadRequest("Password reset attempt has expired.");

            var user = await _userManager.FindByIdAsync(attempt.UserId);
            if (user is null) return NotFound("User not found.");

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, confirmForgotPasswordDto.NewPassword);

            if (!result.Succeeded) return BadRequest(result.Errors);

            attempt.IsSuccessful = true;

            await _dbContext.SaveChangesAsync();

            return NoContent();
        }
    }

   
}
