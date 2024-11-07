﻿using System.ComponentModel.DataAnnotations;

namespace ALMS.API.DTOs.Users
{
    public class CreateUserDto
    {
        [Required]
        public required string Username { get; set; }

        [Required]
        public required string Email { get; set; }

        [Required]
        public required string Password { get; set; }

        [Required]
        public required string FirstName { get; set; }

        [Required]
        public required string LastName { get; set; }

        [Required]
        public required string Address { get; set; }

        [Required]
        public required int MaxBorrowLimit { get; set; }
    }
}
