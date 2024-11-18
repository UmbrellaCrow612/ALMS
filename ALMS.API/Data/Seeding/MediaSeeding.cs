using ALMS.API.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace ALMS.API.Data.Seeding
{
    public class MediaSeeding
    {
        public static async Task SeedAsync(ApplicationDbContext dbContext)
        {
            List<Media> medias =
            [
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
                }
            ];

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
