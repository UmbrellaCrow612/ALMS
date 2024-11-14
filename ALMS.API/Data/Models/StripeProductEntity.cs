namespace ALMS.API.Data.Models
{
    public class StripeProductEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public required string Product { get; set; }
        public required long Quanity { get; set; }
        public required string Rate { get; set; }
    }
}
