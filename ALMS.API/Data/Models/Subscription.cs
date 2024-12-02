namespace ALMS.API.Data.Models
{
    public class Subscription
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public required string Status { get; set; }
        public ApplicationUser? User { get; set; } = null;

        public required string ProductId { get; set; }
        public StripeProductEntity? StripeProduct { get; set; }

        public required string UserId { get; set; }

        public required DateTime StartDate { get; set; }
        public required DateTime EndDate { get; set; }
    }
}
