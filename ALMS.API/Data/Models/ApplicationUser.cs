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
        public bool IsApproved { get; set; } = false;
        public required MembershipStatus MembershipStatus { get; set; } = MembershipStatus.NotActive;
        public required int MaxBorrowLimit { get; set; } = 10;


        /// <summary>
        /// Ef Core Nav Property
        /// </summary>
        public ICollection<BorrowTransaction> BorrowedItems { get; set; } = [];
    }

    public enum MembershipStatus
    {
        NotActive = 0,
        Active = 1,
    }
}
