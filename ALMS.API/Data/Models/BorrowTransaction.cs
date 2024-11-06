namespace ALMS.API.Data.Models
{
    /// <summary>
    /// Records a borrowing transaction for a media item by a library member.
    /// </summary>
    public class BorrowTransaction
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public required string UserId { get; set; }
        public ApplicationUser? User { get; set; } = null;

        public required string MediaId { get; set; }
        public Media? Media { get; set; } = null;

        public DateTime BorrowedAt { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsReturned { get; set; }


        public DateTime? ReturnedAt { get; set; }
    }
}
