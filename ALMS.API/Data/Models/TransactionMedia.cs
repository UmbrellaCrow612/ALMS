namespace ALMS.API.Data.Models
{
    public class TransactionMedia
    {
        public required string TransactionId { get; set; }

        public Transaction? Transaction { get; set; } = null;
        public required string MediaId { get; set; }

        public Media? Media { get; set; } = null;


    }
}
