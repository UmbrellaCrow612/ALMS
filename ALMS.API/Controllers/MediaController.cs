using ALMS.API.Core.Constants;
using ALMS.API.Data;
using ALMS.API.Data.Models;
using ALMS.API.DTOs.Media;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System.Security.Claims;

namespace ALMS.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("media")]
    public class MediaController(ApplicationDbContext dbContext, IMapper mapper, UserManager<ApplicationUser> userManager) : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext = dbContext;
        private readonly IMapper _mapper = mapper;
        private readonly UserManager<ApplicationUser> _userManager = userManager;


        [HttpGet]
        public async Task<ActionResult<List<MediaDto>>> GetMedia([FromQuery] SearchMediaQuery query)
        {
            var mediaQuery = _dbContext.Medias.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.Title))
            {
                mediaQuery = mediaQuery.Where(m => m.Title.Contains(query.Title));
            }

            if (!string.IsNullOrWhiteSpace(query.Author))
            {
                mediaQuery = mediaQuery.Where(m => m.Author.Contains(query.Author));
            }

            if (!string.IsNullOrWhiteSpace(query.Genre))
            {
                mediaQuery = mediaQuery.Where(m => m.Genre.Contains(query.Genre));
            }

            if (query.MediaType.HasValue)
            {
                mediaQuery = mediaQuery.Where(m => m.MediaType == query.MediaType);
            }

            if (query.IsAvailable.HasValue)
            {
                mediaQuery = mediaQuery.Where(m => m.IsAvailable == query.IsAvailable);
            }

            var filteredMedia = await mediaQuery.ToListAsync();

            var media = _mapper.Map<List<MediaDto>>(filteredMedia);

            return Ok(media);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetMediaById(string id)
        {
            var media = await _dbContext.Medias.FirstOrDefaultAsync(m => m.Id == id);
            if (media is null) return NotFound();

            var mediaDto = _mapper.Map<MediaDto>(media);

            return Ok(mediaDto);
        }

        [Authorize(Roles = UserRoles.BranchLibarian)]
        [HttpPost]
        public async Task<ActionResult> CreateMediaItem([FromBody] CreateMediaDto createMediaDto)
        {
            var mediaItemToCreate = _mapper.Map<Media>(createMediaDto);

            await _dbContext.Medias.AddAsync(mediaItemToCreate);
            await _dbContext.SaveChangesAsync();

            return Created(nameof(CreateMediaItem), new { mediaItemToCreate.Id });
        }

        [Authorize(Roles = UserRoles.BranchLibarian)]
        [HttpPatch("{id}")]
        public async Task<ActionResult> UpdateMediaItems([FromBody] UpdateMediaDto updateMediaDto, string id)
        {
            var mediaItemToUpdate = await _dbContext.Medias.FirstOrDefaultAsync(x => x.Id == id);
            if(mediaItemToUpdate is null) return NotFound();

            _mapper.Map(updateMediaDto, mediaItemToUpdate);

            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        [Authorize(Roles = UserRoles.BranchLibarian)]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMediaItem(string id)
        {
            var mediaItemToDelete = await _dbContext.Medias.FirstOrDefaultAsync(x => x.Id == id);
            if(mediaItemToDelete is null) return NotFound();

            _dbContext.Medias.Remove(mediaItemToDelete);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }


        [HttpPost("{id}/borrow")]
        public async Task<ActionResult> BorrowMedia(string id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User ID not found in token.");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound("User not found");

            var media = await _dbContext.Medias.FirstOrDefaultAsync(x => x.Id == id);
            if(media is null) return NotFound("Media to borrow not found.");

            if (!media.IsAvailable)
            {
                return BadRequest("Media is currently not Available");
            }

            int borrowedMediCount = _dbContext.BorrowTransactions.Where(x => x.UserId == userId && x.IsReturned == false).Select(x => x.Id).Count();

            if (borrowedMediCount > user.MaxBorrowLimit)
            {
                return BadRequest("User has already borrowed more media than they are allowed, cannot borrow this media.");
            }

            BorrowTransaction borrowTransaction = new()
            {
                MediaId = media.Id,
                UserId = userId,
                DueDate = DateTime.UtcNow.AddDays(7),
                BorrowedAt = DateTime.UtcNow,
            };

            media.IsAvailable = false;

            await _dbContext.BorrowTransactions.AddAsync(borrowTransaction);
            await _dbContext.SaveChangesAsync();

            return Ok(new {id = borrowTransaction.Id, dueDate = borrowTransaction.DueDate , borrowedAt = borrowTransaction.BorrowedAt });
        }

    }
}
