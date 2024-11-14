using System.ComponentModel.DataAnnotations;

namespace ALMS.API.DTOs.Subscription
{
    public class UpdateSubscriptionDto
    {
        [Required]
        public required string Status { get; set; }

        [Required]
        public required DateTime EndDate { get; set; }
    }
}