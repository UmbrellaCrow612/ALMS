using ALMS.API.Data;
using ALMS.API.Data.Models;
using ALMS.API.DTOs.YSubscription;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ALMS.API.Controllers
{
    [ApiController]
    [Route("y-subscriptions")]
    public class YSubscriptionController(ApplicationDbContext dbContext, IMapper mapper) : ControllerBase
    {
        private readonly ApplicationDbContext _dbcontext = dbContext;
        private readonly IMapper _mapper = mapper;

        [Authorize]
        [HttpPost("subscribe")]
        public async Task<ActionResult> Subscribe([FromBody] CreateYSubscriptionDto dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId is null) return Unauthorized("User id not found in JWT claim.");

            var subscription = _mapper.Map<YSubscription>(dto);
            subscription.UserId = userId;

            await _dbcontext.YSubscriptions.AddAsync(subscription);
            await _dbcontext.SaveChangesAsync();

            return Ok();
        }

        [Authorize]
        [HttpGet("users/{userId}/subscriptions")]
        public async Task<ActionResult> GetUserSubscriptions(string userId)
        {
            var userExists = await _dbcontext.Users.AnyAsync(x => x.Id == userId);
            if (!userExists) return NotFound("User not found");

            var subs = await _dbcontext.YSubscriptions.Where(x => x.UserId == userId).ToListAsync();

            var dto = _mapper.Map<ICollection<YSubscriptionDto>>(subs);

            return Ok(dto);
        }

        [Authorize]
        [HttpGet("subscription/{subscriptionId}")]
        public async Task<ActionResult> GetSubscriptionById(string subscriptionId)
        {
            var sub = await _dbcontext.YSubscriptions.FirstOrDefaultAsync(x => x.Id == subscriptionId);
            if (sub is null) return NotFound("Subscription not found.");

            var dto = _mapper.Map<YSubscriptionDto>(sub);

            return Ok(dto);
        }

        [Authorize]
        [HttpPatch("subscription/{subscriptionId}")]
        public async Task<ActionResult> UpdateSubscription(string subscriptionId, [FromBody] UpdateYSubscriptionDto dto)
        {
            var sub = await _dbcontext.YSubscriptions.FirstOrDefaultAsync(x => x.Id == subscriptionId);
            if (sub is null) return NotFound("Subscription not found.");

            _mapper.Map(dto, sub);

            _dbcontext.YSubscriptions.Update(sub);

            await _dbcontext.SaveChangesAsync();

            return NoContent();
        }

        [Authorize]
        [HttpDelete("subscription/{subscriptionId}")]
        public async Task<ActionResult> DeleteSubscription(string subscriptionId)
        {
            var sub = await _dbcontext.YSubscriptions.FirstOrDefaultAsync(x => x.Id == subscriptionId);
            if (sub is null) return NotFound("Subscription not found.");

            _dbcontext.YSubscriptions.Remove(sub);

            await _dbcontext.SaveChangesAsync();

            return NoContent();
        }
}
}
