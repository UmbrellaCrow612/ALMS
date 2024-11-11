using ALMS.API.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace ALMS.API.DTOs.Media
{
    public class SearchMediaQuery
    {
        public string? Title { get; set; } = null;
        public string? Author { get; set; } = null;
        public string? Genre { get; set; } = null;
        [EnumDataType(typeof(MediaType))]
        public MediaType? MediaType { get; set; } = null;
        public bool? IsAvailable { get; set; } = null;
    }
}
