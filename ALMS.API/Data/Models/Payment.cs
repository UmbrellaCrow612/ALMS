namespace ALMS.API.Data.Models
{
    /// <summary>
    /// Represents a payment made by a library member for their subscription.
    /// </summary>
    public class Payment
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public required string UserId { get; set; }
        public ApplicationUser? User { get; set; } = null;

        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public required string PaymentMethod { get; set; }
        public bool IsSuccessful { get; set; }
    }
}
