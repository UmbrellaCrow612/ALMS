using ALMS.API.Data;
using ALMS.API.Data.Models;
using ALMS.API.DTOs.Reservation;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ALMS.API.Controllers
{
    [ApiController]
    [Route("reservations")]
    public class ReservationController(ApplicationDbContext dbContext, IMapper mapper) : ControllerBase
    {
        private readonly ApplicationDbContext _dbcontext = dbContext;
        private readonly IMapper _mapper = mapper;

        [Authorize]
        [HttpGet]
        public async Task<ActionResult> GetReservations()
        {
            var resevations = await _dbcontext.Reservations.Where(x => x.IsApproved == false).ToListAsync();

            var dto = _mapper.Map<ICollection<ReservationDto>>(resevations);

            return Ok(dto);
        }

        [Authorize]
        [HttpPost("medias/{mediaId}/reserve")]
        public async Task<ActionResult> ReserverMediaById(string mediaId, [FromBody] CreateReservationDto createReservationDto) 
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if(userId is null) return Unauthorized("User id not found in JWT claim.");

            var mediaToReserve = await _dbcontext.Medias.FirstOrDefaultAsync(x => x.Id == mediaId);
            if (mediaToReserve is null) return NotFound("Media not found.");

            var reservation = _mapper.Map<Reservation>(createReservationDto);
            reservation.UserId = userId;
            reservation.MediaId = mediaId;

            await _dbcontext.Reservations.AddAsync(reservation);
            await _dbcontext.SaveChangesAsync();

            return NoContent();
        }

        [Authorize]
        [HttpPost("{reservationId}/approve")]
        public async Task<ActionResult> ApproveReservation(string reservationId)
        {
            var reservation = await _dbcontext.Reservations.FirstOrDefaultAsync(x => x.Id == reservationId);
            if (reservation is null) return NotFound("Reservation not found.");

            if (reservation.IsApproved is true)
            {
                return BadRequest("Reservation already approved.");
            }

            reservation.IsApproved = true;

            _dbcontext.Reservations.Update(reservation);
            await _dbcontext.SaveChangesAsync();

            return NoContent();
        }

        [Authorize]
        [HttpPost("{reservationId}/deny")]
        public async Task<ActionResult> DenyReservation(string reservationId)
        {
            var reservation = await _dbcontext.Reservations.FirstOrDefaultAsync(x => x.Id == reservationId);
            if (reservation is null) return NotFound("Reservation not found.");

            if (reservation.IsApproved is false)
            {
                return BadRequest("Reservation already denied.");
            }

            reservation.IsApproved = false;

            _dbcontext.Reservations.Update(reservation);
            await _dbcontext.SaveChangesAsync();

            return NoContent();
        }

    }
}
