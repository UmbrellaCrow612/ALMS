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
    public class AuthenticationController(IMapper mapper, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IUserStore<ApplicationUser> userStore) : ControllerBase
    {
        private readonly IMapper _mapper = mapper;
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
        private readonly IUserStore<ApplicationUser> _userStore = userStore;
        private readonly IUserEmailStore<ApplicationUser> _userEmailStore = (IUserEmailStore<ApplicationUser>)userStore;

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

            return Ok();
        }

    }


}
