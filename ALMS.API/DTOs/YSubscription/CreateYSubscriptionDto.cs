﻿using System.ComponentModel.DataAnnotations;

namespace ALMS.API.DTOs.YSubscription
{
    public class CreateYSubscriptionDto
    {
        [Required]
        public required decimal Amount { get; set; }

        [Required]
        public required string SubscriptionType { get; set; }

        [Required]
        public DateTime EndDate { get; set; }
    }
}
