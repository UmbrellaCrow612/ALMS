namespace ALMS.API.Data.Models
{
    /// <summary>
    /// Transactions Track Each Media item borrowed or rented by a user
    /// </summary>
    public class Transaction
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public required DateTime BorrowDate { get; set; }

        public DateTime? ReturnDate { get; set; }

        public required decimal Fee { get; set; }

        public required bool IsReturned { get; set; }


        //Relationships 

        public Media? Media { get; set; } = null;

        public required string MediaId { get; set; }
    }

    
}
