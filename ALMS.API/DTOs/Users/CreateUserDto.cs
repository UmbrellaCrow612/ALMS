using System.ComponentModel.DataAnnotations;

namespace ALMS.API.DTOs.Users
{
    public class CreateUserDto
    {
        [Required]
        public required string UserName { get; set; }

        [Required]
        [EmailAddress]
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
