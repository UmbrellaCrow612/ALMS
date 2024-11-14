using ALMS.API.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace ALMS.API.DTOs.Subscription
{
    public class CreateStripeProductDto
    {
        [Required]
        public required string Product { get; set; }
        [Required]
        public required long Quanity { get; set; }
        [Required]
        public required string Rate { get; set; }
    }
}