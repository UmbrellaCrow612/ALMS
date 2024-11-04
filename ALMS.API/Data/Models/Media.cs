
using ALMS.API.Data.Models;

namespace ALMS.API.Data
{
    public class Media
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public required string Title { get; set; }

        public required string AuthorName { get; set; }

        public required string Genre { get; set; }
        public string? ISBN { get; set; }

        public required decimal RentalPrice { get; set; }

        public MediaType Type { get; set; }

        //Relationships
        public IEnumerable<Transaction>? Transactions { get; set; } = [];
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
