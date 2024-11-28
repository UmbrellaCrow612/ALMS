using ALMS.API.Data;
using ALMS.API.DTOs.BorrowTransaction;
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
    [Route("users")]
    public class UserController(ApplicationDbContext dbContext, IMapper mapper) : ControllerBase
    {
        private readonly ApplicationDbContext _dbcontext = dbContext;
        private readonly IMapper _mapper = mapper;


        [HttpGet("borrowed-transactions")]
        public async Task<ActionResult> GetBorrowTransactions()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User ID not found in token.");
            }

            var transactions = await _dbcontext.BorrowTransactions
                .Where(x => x.UserId == userId && x.IsReturned == false)
                .Include(x => x.Media)
                .ToListAsync();

            var dto = _mapper.Map<IEnumerable<BorrowTransactionDto>>(transactions);

            return Ok(dto);
        }

        [HttpPost("borrowed-transactions/{borrowTransactionId}/return")]
        public async Task<ActionResult> ReturnBook(string borrowTransactionId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User ID not found in token.");
            }

            var transaction = await _dbcontext.BorrowTransactions
                .Include(x => x.Media)
                .FirstOrDefaultAsync(x => x.Id == borrowTransactionId && x.UserId == userId);

            if (transaction is null) return NotFound("Transaction not found");
            if (transaction.Media is null) return NotFound("Transaction dose not contain a media.");

            transaction.IsReturned = true;
            transaction.ReturnedAt = DateTime.UtcNow;

            transaction.Media.IsAvailable = true;

            _dbcontext.BorrowTransactions.Update(transaction);
            await _dbcontext.SaveChangesAsync();

            return Ok();
        }

        [Authorize]
        [HttpGet("{userId}")]
        public async Task<ActionResult> GetUserById(string userId)
        {
            var user = await _dbcontext.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if (user is null) return NotFound("user not found.");

            var dto = _mapper.Map<UserDto>(user);

            return Ok(dto);
        }
    }
}
