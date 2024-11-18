using System.ComponentModel.DataAnnotations;

namespace ALMS.API.DTOs.Auth
{
    public class ConfirmForgotPasswordDto
    {
        [Required]
        public required string NewPassword { get; set; }
    }
}
