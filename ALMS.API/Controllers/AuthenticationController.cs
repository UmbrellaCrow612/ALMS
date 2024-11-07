using ALMS.API.Core.Constants;
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
    public class AuthenticationController(IMapper mapper, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IUserStore<ApplicationUser> userStore, RoleManager<IdentityRole> roleManager) : ControllerBase
    {
        private readonly IMapper _mapper = mapper;
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
        private readonly IUserStore<ApplicationUser> _userStore = userStore;
        private readonly IUserEmailStore<ApplicationUser> _userEmailStore = (IUserEmailStore<ApplicationUser>)userStore;
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;

        [Authorize(Roles = UserRoles.BranchLibarian)]
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

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginDto loginDto, [FromQuery] bool? useCookies, [FromQuery] bool? useSessionCookies)
        {
            var useCookieScheme = (useCookies == true) || (useSessionCookies == true);
            var isPersistent = (useCookies == true) && (useSessionCookies != true);
            _signInManager.AuthenticationScheme = useCookieScheme ? IdentityConstants.ApplicationScheme : IdentityConstants.BearerScheme;

            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user is null || !user.IsApproved)
            {
                return Unauthorized("User dose not exist or is approved yet, please ask for approval");
            }

            var result = await _signInManager.PasswordSignInAsync(loginDto.Email, loginDto.Password, isPersistent, lockoutOnFailure: true);

            if (!result.Succeeded)
            {
                return Unauthorized();
            }

            return Ok();
        }

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

            return Ok();
        }

        [HttpPost("users/{id}/roles")]
        public async Task<ActionResult> AddRoles([FromBody] IEnumerable<string> Roles, string id)
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
    }
}
