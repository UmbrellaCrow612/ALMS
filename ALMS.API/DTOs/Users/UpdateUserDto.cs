using ALMS.API.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace ALMS.API.DTOs.Users
{
    public class UpdateUserDto
    {

        [Required]
        public required string FirstName { get; set; }

        [Required]
        public required string LastName { get; set; }

        [Required]
        public required string Address { get; set; }
        [Required]
        public required string PhoneNumber { get; set; }
    }
}
