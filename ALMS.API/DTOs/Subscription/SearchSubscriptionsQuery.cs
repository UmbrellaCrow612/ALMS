namespace ALMS.API.DTOs.Subscription
{
    public class SearchSubscriptionsQuery
    {
        public string? FirstName { get; set; } = null;
        public string? LastName { get; set;} = null;
        public string? Address { get; set;} = null;
        public string? PhoneNumber { get; set; } = null;
    }
}
