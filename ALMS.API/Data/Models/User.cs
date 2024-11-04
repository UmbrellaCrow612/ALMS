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
