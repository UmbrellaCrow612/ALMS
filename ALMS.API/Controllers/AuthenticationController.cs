﻿using ALMS.API.Core.Constants;
using ALMS.API.Data.Models;
using ALMS.API.DTOs.Auth;
using ALMS.API.DTOs.Users;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ALMS.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("auth")]
    public class AuthenticationController(IMapper mapper, UserManager<ApplicationUser> userManager, IUserStore<ApplicationUser> userStore, RoleManager<IdentityRole> roleManager, IConfiguration configuration) : ControllerBase
    {
        private readonly IMapper _mapper = mapper;
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IUserStore<ApplicationUser> _userStore = userStore;
        private readonly IUserEmailStore<ApplicationUser> _userEmailStore = (IUserEmailStore<ApplicationUser>)userStore;
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;
        private readonly IConfiguration _configuration = configuration;

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

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<string>> Login([FromBody] LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user is null || !user.IsApproved)
            {
                return Unauthorized("User does not exist or is not approved yet, please ask for approval");
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            var isValid = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!isValid)
            {
                return Unauthorized();
            }

            var claims = new List<Claim>();
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id);
                new Claim(ClaimTypes.Email, user.Email!);
            };

            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? throw new ApplicationException("No Key provided"))), SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
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

            return Ok();
        }

        [Authorize(Roles = UserRoles.BranchLibarian)]
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
