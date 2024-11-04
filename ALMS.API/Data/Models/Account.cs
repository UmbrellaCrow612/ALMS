namespace ALMS.API.Data.Models
{
    public class Account
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
    }
}
