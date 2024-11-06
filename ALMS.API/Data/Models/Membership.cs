namespace ALMS.API.Data.Models
{
    public class Membership
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public required DateTime StartDate { get; set; } = DateTime.UtcNow;
        public DateTime? EndDate { get; set; }

        public required decimal MonthlyFee { get; set; }


        //Relationships

        public ApplicationUser? User { get; set; } = null;
        public required string UserId { get; set; }

    }

    public enum MembershipLevel
    {
        None = 0,
        Basic = 1,
        Silver = 2,
        Gold = 3
    }
}
