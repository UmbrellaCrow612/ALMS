namespace ALMS.API.Data.Models
{
    public class YSubscription
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public required decimal Amount { get; set; }
        public required string SubscriptionType { get; set; }
        public DateTime StartDate { get; set; } = DateTime.UtcNow;
        public DateTime? EndDate { get; set; } = null;
        public bool IsActive { get; set; } = true;
        public required string UserId { get; set; }
        public ApplicationUser? User { get; set; } = null;
    }
}
