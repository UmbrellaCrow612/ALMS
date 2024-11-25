using System.ComponentModel.DataAnnotations;

namespace ALMS.API.DTOs.Reservation
{
    public class CreateReservationDto
    {
        [Required]
        public required DateTime ReserveFrom { get; set; }

        [Required]
        public required DateTime ReserveTo { get; set; }
    }
}
