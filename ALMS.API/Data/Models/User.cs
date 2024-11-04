namespace ALMS.API.Data.Models
{
    public class User
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public UserRole Role { get; set; } 

        public required string FirstName { get; set; }
        public required string LastName { get; set; }

        public required string Email { get; set; }

        public required string Address { get; set; }

        public required string PhoneNumber { get; set;}


        //RelationShips:

        public Membership? Membership { get; set; } = null;

        public string? MembershipId {  get; set; } = null;

        public IEnumerable<Payment>? Payments { get; set; } = [];

        public IEnumerable<Subscription>? Subscriptions { get; set; } = [];

        public Account? Account { get; set; } = null!;

        public required string AccountId { get; set; }
    }

    public enum UserRole
    {
        Guest = 0,
        BranchLibarian = 1,
        LibaryMember = 2,
        CallCenerOperator = 3,
        Accountant = 4
    }
}
