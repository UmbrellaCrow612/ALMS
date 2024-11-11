using ALMS.API.Data.Models;

namespace ALMS.API.DTOs.Media
{
    public class MediaDto
    {
        public required string Id { get; set; }
        public required string Title { get; set; }
        public required string Author { get; set; }
        public required string Genre { get; set; }
        public required MediaType MediaType { get; set; }
        public required bool IsAvailable { get; set; }
        public required DateTime DateAdded { get; set; }
    }
}
