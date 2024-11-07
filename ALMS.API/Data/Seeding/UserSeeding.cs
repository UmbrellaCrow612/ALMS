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
            var branchLibrarianEmail = "branchlibrarian@example.com";
            var guestEmail = "guest@example.com";

            // Seed BranchLibrarian user
            var librarian = await userManager.FindByEmailAsync(branchLibrarianEmail);
            if (librarian == null)
            {
                var newLibrarian = new ApplicationUser
                {
                    UserName = branchLibrarianEmail,
                    Email = branchLibrarianEmail,
                    FirstName = "Default",
                    LastName = "Librarian",
                    Address = "123 Library St",
                    IsApproved = true,
                    MembershipStatus = MembershipStatus.Active,
                    MaxBorrowLimit = 10
                };

                var result = await userManager.CreateAsync(newLibrarian, "SecureP@ssw0rd");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(newLibrarian, UserRoles.BranchLibarian);
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine($"Error creating BranchLibrarian: {error.Description}");
                    }
                }
            }

            // Seed Guest user
            var guest = await userManager.FindByEmailAsync(guestEmail);
            if (guest == null)
            {
                var newGuest = new ApplicationUser
                {
                    UserName = guestEmail,
                    Email = guestEmail,
                    FirstName = "Default",
                    LastName = "Guest",
                    Address = "456 Guest Rd",
                    IsApproved = true,
                    MembershipStatus = MembershipStatus.NotActive,
                    MaxBorrowLimit = 0 // Set a default borrow limit for guests
                };

                var guestResult = await userManager.CreateAsync(newGuest, "GuestP@ssw0rd");

                if (guestResult.Succeeded)
                {
                    await userManager.AddToRoleAsync(newGuest, UserRoles.Guest);
                }
                else
                {
                    foreach (var error in guestResult.Errors)
                    {
                        Console.WriteLine($"Error creating Guest: {error.Description}");
                    }
                }
            }
        }
    }
}
