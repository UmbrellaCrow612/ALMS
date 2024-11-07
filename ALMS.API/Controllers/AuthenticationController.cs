using ALMS.API.Data.Models;
using ALMS.API.DTOs.Auth;
using ALMS.API.DTOs.Users;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ALMS.API.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthenticationController(IMapper mapper, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager) : ControllerBase
    {
        private readonly IMapper _mapper = mapper;
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly SignInManager<ApplicationUser> _signInManager = signInManager;

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<IdentityResult>> Register([FromBody] CreateUserDto createUserDto)
        {

            var userToCreate = _mapper.Map<ApplicationUser>(createUserDto);

            var result = await _userManager.CreateAsync(userToCreate, createUserDto.Password);

            if (result.Succeeded)
            {
                // User creation successful
                return Ok(result);
            }
            else
            {
                // Handle errors
                return BadRequest(result);
            }
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null || !user.IsActive)
            {
                return Unauthorized("Account not found or not active.");
            }

            var result = await _signInManager.PasswordSignInAsync(user, loginDto.Password, loginDto.RememberMe, lockoutOnFailure: true);
            if (result.Succeeded)
            {
                return Ok("Login successful.");
            }

            if (result.IsLockedOut)
            {
                return Unauthorized("User is locked out.");
            }

            return Unauthorized("Invalid login attempt.");
        }

    }


}
