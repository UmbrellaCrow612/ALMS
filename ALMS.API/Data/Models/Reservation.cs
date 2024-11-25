namespace ALMS.API.Data.Models
{
    /// <summary>
    /// Represents a reservation for a media item when it is unavailable.
    /// </summary>
    public class Reservation
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public required string UserId { get; set; }
        public ApplicationUser? User { get; set; } = null;

        public required string MediaId { get; set; }
        public Media? Media { get; set; } = null;

        public DateTime ReserveFrom { get; set; }

        public required DateTime ReserveTo { get; set; }

        public bool IsApproved { get; set; }
    }
}
