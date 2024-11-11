using ALMS.API.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace ALMS.API.DTOs.Media
{
    public class CreateMediaDto
    {
        [Required]
        public required string Title { get; set; }

        [Required]
        public required string Author { get; set; }

        [Required]
        public required string Genre { get; set; }

        [Required]
        [EnumDataType(typeof(MediaType))]
        public required MediaType MediaType { get; set; }
    }
}
