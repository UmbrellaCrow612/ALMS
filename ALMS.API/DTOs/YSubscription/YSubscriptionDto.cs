namespace ALMS.API.DTOs.YSubscription
{
    public class YSubscriptionDto
    {
        public required string Id { get; set; } 
        public required decimal Amount { get; set; }
        public required string SubscriptionType { get; set; }
        public DateTime StartDate { get; set; } 
        public DateTime EndDate { get; set; } 
        public bool IsActive { get; set; } 
        public required string UserId { get; set; }
    }
}
