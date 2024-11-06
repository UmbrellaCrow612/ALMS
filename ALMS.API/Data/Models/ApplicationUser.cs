using Microsoft.AspNetCore.Identity;

namespace ALMS.API.Data.Models
{
    /// <summary>
    /// ALMS User
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        public required string FirstName { get; set; }

        public required string LastName { get; set; }

        public required string Address { get; set; }
    }
}
