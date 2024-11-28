using ALMS.API.Data.Models;

namespace ALMS.API.DTOs.Subscription
{
    public class SubscriptionDto
    {
        public required string Id { get; set; }
        public required string Status { get; set; }
        public required string UserId { get; set; }
        public required DateTime StartDate { get; set; }
        public required DateTime EndDate { get; set; }


        public string? NameType { get; set; } = null;
    }
}
