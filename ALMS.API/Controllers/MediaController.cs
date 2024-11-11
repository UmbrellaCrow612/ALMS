using ALMS.API.Core.Constants;
using ALMS.API.Data;
using ALMS.API.Data.Models;
using ALMS.API.DTOs.Media;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ALMS.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("media")]
    public class MediaController(ApplicationDbContext dbContext, IMapper mapper) : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext = dbContext;
        private readonly IMapper _mapper = mapper;


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
    }
}
