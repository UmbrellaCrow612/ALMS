namespace ALMS.API.Data.Models
{
    public class Account
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public required string Username { get; set; }
        public required string PasswordHash { get; set; }

        public required AccountStatus Status { get; set; }
    }

    public enum AccountStatus
    {
        Approved = 0,
        Denied = 1,
        Pending = 2,
        Suspended = 3
    }
}
