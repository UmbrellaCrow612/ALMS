using ALMS.API.Core.Constants;
using ALMS.API.Data;
using ALMS.API.DTOs.Media;
using ALMS.API.DTOs.Users;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

            //userToUpdate.Email;

            await _dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
