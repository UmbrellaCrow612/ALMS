namespace ALMS.API.Data.Models
{
    /// <summary>
    /// Tracks changes to the media inventory, such as additions, losses, and returns.
    /// </summary>
    public class InventoryLog
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public required string MediaId { get; set; }
        public Media? Media { get; set; } = null;

        public required string Action { get; set; }
        public DateTime ActionDate { get; set; }

        public required string UserId { get; set; }
        public ApplicationUser? User { get; set; } = null;
    }
}
