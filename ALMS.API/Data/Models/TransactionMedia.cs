namespace ALMS.API.Data.Models
{
    /// <summary>
    /// Join table between a <see cref="Models.Transaction"/> and <see cref="Models.Media"/>
    /// </summary>
    public class TransactionMedia
    {
        public required string TransactionId { get; set; }

        public Transaction? Transaction { get; set; } = null;

        public required string MediaId { get; set; }

        public Media? Media { get; set; } = null;


    }
}
