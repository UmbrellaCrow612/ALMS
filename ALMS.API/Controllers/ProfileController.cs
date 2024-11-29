using ALMS.API.Core.Constants;
using ALMS.API.Data;
using ALMS.API.DTOs.Media;
using ALMS.API.DTOs.Users;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ALMS.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("profile")]
    public class ProfileController (ApplicationDbContext dbContext, IMapper mapper) : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext = dbContext;
        private readonly IMapper _mapper = mapper;

        [Authorize(Roles = UserRoles.CallCenterOperator)]
        [HttpPatch("update/{id}")]
        public async Task<ActionResult> UpdateUser([FromBody] UpdateUserDto updateUserDto, string id)
        {
            var userToUpdate = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (userToUpdate is null) return NotFound("User not found.");

            // Simple validation of the data (for example, missing required fields)
            if (string.IsNullOrEmpty(updateUserDto.FirstName) || string.IsNullOrEmpty(updateUserDto.LastName))
                return BadRequest("Invalid data entered. Please correct and try again.");

            _mapper.Map(updateUserDto, userToUpdate);

            var emailService = new EmailService();
            if(userToUpdate.Email != null)
            {
                emailService.SendEmail(userToUpdate.Email, "Information Updated", $"Hi {userToUpdate.FirstName}, \n\n We have updated your details as requested.\n\n Thank you" +
              $"\nKind Regards\nCall Center Support Team\n\n**PLEASE CONTACT US ON 011425465466 IF YOU DID NOT REQUEST TO CHANGE YOUR DETAILS");

            }

            await _dbContext.SaveChangesAsync();

            return Ok("Profile updated successfully.");
        }

        [Authorize(Roles = $"{UserRoles.BranchLibarian},{UserRoles.CallCenterOperator},{UserRoles.Accountant}")]
        [HttpGet("search")]
        public async Task<ActionResult<List<UserDto>>> GetProfile([FromQuery] SearchUserQuery query)
        {
            var userQuery = _dbContext.Users.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.FirstName))
            {
                userQuery = userQuery.Where(m => m.FirstName.Contains(query.FirstName));
            }

            if (!string.IsNullOrWhiteSpace(query.LastName))
            {
                userQuery = userQuery.Where(m => m.LastName.Contains(query.LastName));
            }

            if (!string.IsNullOrWhiteSpace(query.Email))
            {
                userQuery = userQuery.Where(m => m.Email.Contains(query.Email));
            }
            if (!string.IsNullOrWhiteSpace(query.PhoneNumber))
            {
                userQuery = userQuery.Where(m => m.PhoneNumber.Contains(query.PhoneNumber));
            }
            if (!string.IsNullOrWhiteSpace(query.Address))
            {
                userQuery = userQuery.Where(m => m.Address.Contains(query.Address));
            }
            if (!string.IsNullOrWhiteSpace(query.UserName))
            {
                userQuery = userQuery.Where(m => m.UserName.Contains(query.UserName));
            }
            var filteredUser = await userQuery.ToListAsync();

            var user = _mapper.Map<IEnumerable<UserDto>>(filteredUser);

            return Ok(user);
        }
    }
}
