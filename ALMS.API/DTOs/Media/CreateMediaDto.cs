﻿using ALMS.API.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace ALMS.API.DTOs.Media
{
    public class CreateMediaDto
    {
        [Required]
        public required string Title { get; set; }

        [Required]
        public required string Description { get; set; }

        [Required]
        public required DateTime PublishedAt { get; set; }

        public required string? ISBN { get; set; } = null;

        [Required]
        public required string ImgUrl { get; set; }

        [Required]
        public required string Author { get; set; }

        [Required]
        public required string Genre { get; set; }

        [Required]
        [EnumDataType(typeof(MediaType))]
        public required MediaType MediaType { get; set; }
    }
}
