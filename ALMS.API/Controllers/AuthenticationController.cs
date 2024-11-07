using ALMS.API.Data.Models;
using ALMS.API.DTOs.Users;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ALMS.API.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthenticationController(IMapper mapper, UserManager<ApplicationUser> userManager) : ControllerBase
    {
        private readonly IMapper _mapper = mapper;
        private readonly UserManager<ApplicationUser> _userManager = userManager;


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
    }
}
