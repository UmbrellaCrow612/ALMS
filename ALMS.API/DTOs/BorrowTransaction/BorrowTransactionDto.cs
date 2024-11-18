using ALMS.API.DTOs.Media;

namespace ALMS.API.DTOs.BorrowTransaction
{
    public class BorrowTransactionDto
    {
        public required string Id { get; set; }
        public required MediaDto Media { get; set; }
        public DateTime BorrowedAt { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsReturned { get; set; }
        public DateTime? ReturnedAt { get; set; }
    }
}
