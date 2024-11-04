namespace ALMS.API.Data.Models
{
    public class Subscription
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public required SubscriptionType Type { get; set; }

        public required DateTime StartDate { get; set; }


        public DateTime? EndDate { get; set;}

        public required bool IsActive { get; set; }

        //Relationships

        public User? User { get; set; } = null;
        public required string UserId { get; set; }

    }

    public enum SubscriptionType
    {
        AudioBookSubscription = 0
    }
}
