namespace ALMS.API.Data.Models
{
    /// <summary>
    /// Represents a media item in the library, such as a book, DVD, etc.
    /// </summary>
    public class Media
    {
        public required string Id { get; set; } = Guid.NewGuid().ToString();
        public required string Title { get; set; }
        public required string Author { get; set; }
        public required string Genre { get; set; }
        public required MediaType MediaType { get; set; }
        public required bool IsAvailable { get; set; } = true;
        public required DateTime? DateAdded { get; set; }

        /// <summary>
        /// Ef COre Nav Property
        /// </summary>
        public ICollection<BorrowTransaction> BorrowedTransactions { get; set; } = [];
        /// <summary>
        /// Ef COre Nav Property
        /// </summary>
        public ICollection<Reservation> Reservations { get; set; } = [];
    }

    public enum MediaType
    {
        DVD = 0,
        Book = 1,
        AudioBook = 2,
        Games = 3,
        Journal = 4,
        Periodicals = 5,
        CDs = 6,
        MultimediaTitles = 7
    }
}
