﻿using ALMS.API.Core.Constants;
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
            if (userToUpdate is null) return NotFound();

            _mapper.Map(updateUserDto, userToUpdate);

  

            var emailService = new EmailService();
            if(userToUpdate.Email != null)
            {
                emailService.SendEmail(userToUpdate.Email, "Information Updated", $"Hi {userToUpdate.FirstName}, \n\n We have updated your details as requested.\n\n Thank you" +
              $"\nKind Regards\nCall Center Support Team\n\n**PLEASE CONTACT US ON 011425465466 IF YOU DID NOT REQUEST TO CHANGE YOUR DETAILS");

            }
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}