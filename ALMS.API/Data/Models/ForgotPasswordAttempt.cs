namespace ALMS.API.Data.Models
{
    public class ForgotPasswordAttempt
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public required string UserId { get; set; }
        public ApplicationUser? User { get; set; } = null;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsSuccessful { get; set; } = false;
        public required string Code { get; set; }
    }
}
