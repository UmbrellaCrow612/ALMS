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
        /// <summary>
        /// Higher privilege will activate a user account / in the system treat this model as account 
        /// </summary>

        [Required]
        [EnumDataType(typeof(MembershipStatus))]
        public required MembershipStatus MembershipStatus { get; set; }
    }
}
