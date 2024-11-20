using System.ComponentModel.DataAnnotations;

namespace ALMS.API.DTOs.Users
{
    public class SearchUserQuery
    {
        public string? FirstName { get; set; } = null;
        public string? LastName { get; set; } = null;
        public string? Email { get; set; } = null;
        public string? Address { get; set; } = null;
        public string? PhoneNumber { get; set; } = null;
        public string? UserName { get; set; } = null;
    }
}
