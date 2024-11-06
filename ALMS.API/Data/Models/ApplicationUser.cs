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
        /// <summary>
        /// Higher privilege will activate a user account / in the system treat this model as account 
        /// </summary>
        public bool IsActive { get; set; } = false;


        /// <summary>
        /// Ef Core Nav Property
        /// </summary>
        public ICollection<BorrowTransaction> BorrowedItems { get; set; } = [];
        /// <summary>
        /// Ef Core Nav Property
        /// </summary>
        public ICollection<Payment> Payments { get; set; } = [];
    }
}
