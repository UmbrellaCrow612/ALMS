using ALMS.API.Data.Models;
using ALMS.API.DTOs.Users;

namespace ALMS.API.DTOs.Reservation
{
    public class ReservationDto
    {
        public required string Id { get; set; }

        public required string UserId { get; set; }

        public required UserDto User { get; set; }

        public required string MediaId { get; set; }

        public DateTime ReserveFrom { get; set; }

        public required DateTime ReserveTo { get; set; }

        public bool IsApproved { get; set; } = false;
    }
}
