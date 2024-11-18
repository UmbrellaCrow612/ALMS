using System.ComponentModel.DataAnnotations;

namespace ALMS.API.DTOs.Auth
{
    public class ForgotPasswordDto
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }
    }
}
