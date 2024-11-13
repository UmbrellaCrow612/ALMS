namespace ALMS.API.Data.Models
{
    public class StripeProductEntity
    {
        public int Id { get; set; }
        public string Product {  get; set; }

        public string Rate { get; set; }
        public long Quanity { get; set; }
    }
}
