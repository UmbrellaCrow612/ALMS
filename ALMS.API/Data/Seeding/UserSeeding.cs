using ALMS.API.Core.Constants;
using ALMS.API.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace ALMS.API.Data.Seeding
{
    public class UserSeeding
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager)
        {
            var branchLibrarianEmail = "branchlibrarian@example.com";

            // Check if the BranchLibarian user already exists
            var librarian = await userManager.FindByEmailAsync(branchLibrarianEmail);
            if (librarian == null)
            {
                // Create a new ApplicationUser for BranchLibarian
                var newLibrarian = new ApplicationUser
                {
                    UserName = branchLibrarianEmail,
                    Email = branchLibrarianEmail,
                    FirstName = "Default",
                    LastName = "Librarian",
                    Address = "123 Library St",
                    IsApproved = true,
                    MembershipStatus = MembershipStatus.Active,
                    MaxBorrowLimit = 10 // Set a default borrow limit
                };

                // Create the user with a default password
                var result = await userManager.CreateAsync(newLibrarian, "SecureP@ssw0rd");

                if (result.Succeeded)
                {
                    // Assign the BranchLibarian role to the new user
                    await userManager.AddToRoleAsync(newLibrarian, UserRoles.BranchLibarian);
                }
                else
                {
                    // Log errors if the user creation fails
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine($"Error creating BranchLibarian: {error.Description}");
                    }
                }
            }
        }
    }
}
