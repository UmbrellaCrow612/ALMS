using ALMS.API.Core.Constants;
using ALMS.API.Data.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace ALMS.API.Data.Seeding
{
    public class UserSeeding
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager)
        {
            // Role-specific user data
            var users = new[]
            {
                new { Email = "branchlibrarian@example.com", FirstName = "Default", LastName = "Librarian", Role = UserRoles.BranchLibarian, Password = "SecureP@ssw0rd", MaxBorrowLimit = 10, MembershipStatus = MembershipStatus.Active },
                new { Email = "guest@example.com", FirstName = "Default", LastName = "Guest", Role = UserRoles.Guest, Password = "GuestP@ssw0rd", MaxBorrowLimit = 0, MembershipStatus = MembershipStatus.NotActive },
                new { Email = "librarymember@example.com", FirstName = "Default", LastName = "Member", Role = UserRoles.LibaryMember, Password = "MemberP@ssw0rd", MaxBorrowLimit = 5, MembershipStatus = MembershipStatus.Active },
                new { Email = "callcenteroperator@example.com", FirstName = "Default", LastName = "Operator", Role = UserRoles.CallCenterOperator, Password = "OperatorP@ssw0rd", MaxBorrowLimit = 0, MembershipStatus = MembershipStatus.NotActive },
                new { Email = "accountant@example.com", FirstName = "Default", LastName = "Accountant", Role = UserRoles.Accountant, Password = "AccountP@ssw0rd", MaxBorrowLimit = 0, MembershipStatus = MembershipStatus.NotActive }
            };

            foreach (var user in users)
            {
                var existingUser = await userManager.FindByEmailAsync(user.Email);
                if (existingUser == null)
                {
                    var newUser = new ApplicationUser
                    {
                        UserName = user.Email,
                        Email = user.Email,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Address = "123 Default St",
                        IsApproved = true,
                        MembershipStatus = user.MembershipStatus,
                        MaxBorrowLimit = user.MaxBorrowLimit
                    };

                    var result = await userManager.CreateAsync(newUser, user.Password);

                    if (result.Succeeded)
                    {
                        if (await userManager.IsInRoleAsync(newUser, user.Role))
                        {
                            await userManager.AddToRoleAsync(newUser, user.Role);
                        }
                        else
                        {
                        
                        }
                    }
                    else
                    {
                       
                    }
                }
            }
        }
    }
}