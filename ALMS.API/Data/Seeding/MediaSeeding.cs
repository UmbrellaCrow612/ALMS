using ALMS.API.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace ALMS.API.Data.Seeding
{
    public class MediaSeeding
    {
        public static async Task SeedAsync(ApplicationDbContext dbContext)
        {
            List<Media> medias = new List<Media>
{
    new() {
        Id = "1243",
        Author = "Marcus",
        DateAdded = DateTime.Now,
        Description = "A book about stoics",
        Genre = "philosophy",
        ImgUrl = "https://picsum.photos/200/300",
        IsAvailable = true,
        ISBN = "123456",
        MediaType = MediaType.Book,
        PublishedAt = DateTime.Now,
        Title = "Mediations",
    },
    new() {
        Id = "4321",
        Author = "Diogenes",
        DateAdded = DateTime.Now,
        Description = "A book about cynicsm",
        Genre = "philosophy",
        ImgUrl = "https://picsum.photos/200/300",
        IsAvailable = true,
        ISBN = "654321",
        MediaType = MediaType.Book,
        PublishedAt = DateTime.Now,
        Title = "Sayings and anecdotes",
    },
    new() {
        Id = "8432759823759235",
        Author = "Sami",
        DateAdded = DateTime.Now,
        Description = "A book about sami",
        Genre = "Sami",
        ImgUrl = "https://picsum.photos/200/300",
        IsAvailable = false,
        ISBN = "241092401",
        MediaType = MediaType.Book,
        PublishedAt = DateTime.Now,
        Title = "Sayings and Sami",
    },
    // New additions start here
    new() {
        Id = "9876543210",
        Author = "Elena Petrova",
        DateAdded = DateTime.Now,
        Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
        Genre = "Science Fiction",
        ImgUrl = "https://picsum.photos/200/300",
        IsAvailable = true,
        ISBN = "987654321",
        MediaType = MediaType.Book,
        PublishedAt = DateTime.Now,
        Title = "Quantum Horizons",
    },
    new() {
        Id = "1357924680",
        Author = "James Robertson",
        DateAdded = DateTime.Now,
        Description = "Sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.",
        Genre = "Historical Fiction",
        ImgUrl = "https://picsum.photos/200/300",
        IsAvailable = true,
        ISBN = "135792468",
        MediaType = MediaType.Book,
        PublishedAt = DateTime.Now,
        Title = "Echoes of Empires",
    },
    new() {
        Id = "2468013579",
        Author = "Maria Gonzalez",
        DateAdded = DateTime.Now,
        Description = "Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris.",
        Genre = "Poetry",
        ImgUrl = "https://picsum.photos/200/300",
        IsAvailable = true,
        ISBN = "246801357",
        MediaType = MediaType.Book,
        PublishedAt = DateTime.Now,
        Title = "Whispers of the Soul",
    },
    new() {
        Id = "1122334455",
        Author = "David Kim",
        DateAdded = DateTime.Now,
        Description = "Duis aute irure dolor in reprehenderit in voluptate velit esse cillum.",
        Genre = "Technology",
        ImgUrl = "https://picsum.photos/200/300",
        IsAvailable = false,
        ISBN = "112233445",
        MediaType = MediaType.Book,
        PublishedAt = DateTime.Now,
        Title = "The Digital Revolution",
    },
    new() {
        Id = "9988776655",
        Author = "Sophia Laurent",
        DateAdded = DateTime.Now,
        Description = "Excepteur sint occaecat cupidatat non proident, sunt in culpa qui.",
        Genre = "Romance",
        ImgUrl = "https://picsum.photos/200/300",
        IsAvailable = true,
        ISBN = "998877665",
        MediaType = MediaType.Book,
        PublishedAt = DateTime.Now,
        Title = "Paris Nights",
    },
    new() {
        Id = "5544332211",
        Author = "Mohammed Abdullah",
        DateAdded = DateTime.Now,
        Description = "Sunt in culpa qui officia deserunt mollit anim id est laborum.",
        Genre = "Philosophy",
        ImgUrl = "https://picsum.photos/200/300",
        IsAvailable = true,
        ISBN = "554433221",
        MediaType = MediaType.Book,
        PublishedAt = DateTime.Now,
        Title = "Reflections on Existence",
    },
    new() {
        Id = "7766554433",
        Author = "Chen Wei",
        DateAdded = DateTime.Now,
        Description = "Lorem ipsum cupidatat non proident, sunt in culpa qui officia.",
        Genre = "Mystery",
        ImgUrl = "https://picsum.photos/200/300",
        IsAvailable = false,
        ISBN = "776655443",
        MediaType = MediaType.Book,
        PublishedAt = DateTime.Now,
        Title = "The Jade Cipher",
    },
    new() {
        Id = "6677889900",
        Author = "Isabella Rodriguez",
        DateAdded = DateTime.Now,
        Description = "Dolor in reprehenderit in voluptate velit esse cillum dolore.",
        Genre = "Fantasy",
        ImgUrl = "https://picsum.photos/200/300",
        IsAvailable = true,
        ISBN = "667788990",
        MediaType = MediaType.Book,
        PublishedAt = DateTime.Now,
        Title = "Realms of Enchantment",
    },
    new() {
        Id = "2233445566",
        Author = "Alexandre Dupont",
        DateAdded = DateTime.Now,
        Description = "Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi.",
        Genre = "Biography",
        ImgUrl = "https://picsum.photos/200/300",
        IsAvailable = true,
        ISBN = "223344556",
        MediaType = MediaType.Book,
        PublishedAt = DateTime.Now,
        Title = "Pioneers of Change",
    }
};
            foreach (var media in medias)
            {
                if (!await dbContext.Medias.AnyAsync(x => x.Id == media.Id))
                {
                    await dbContext.Medias.AddAsync(media);
                    await dbContext.SaveChangesAsync();
                }
            }
        }
    }
}
