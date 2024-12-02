namespace ALMS.API.Data.Models
{
    public class StripeSession
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public required string SessionId { get; set; }
        public required string SessionUrl { get; set; }

        public ApplicationUser? User { get; set; } = null;

        public required string UserId { get; set; }

        public StripeProductEntity? Product { get; set; }
        public required string ProductId { get; set; }
    }
}
