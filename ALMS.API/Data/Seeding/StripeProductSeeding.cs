using ALMS.API.Core.Constants;
using ALMS.API.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ALMS.API.Data.Seeding
{
    public class StripeProductSeeding
    {
   
        public static async Task SeedAsync(ApplicationDbContext dbContext)
        {
            // Role-specific user data
            var stripeProducts = new[]
            {
                new StripeProductEntity
                {
                    Id = "1",
                    Product = "Advance Libary System Subscription (Basic)",
                    Rate = "£4.99",
                    Quanity = 1
                }
            };

            foreach (var product in stripeProducts)
            {
                
                if(await dbContext.StripeProducts.FirstOrDefaultAsync(x => x.Id == product.Id) is null)
                {
                    dbContext.StripeProducts.Add(product);
                } 
                
                await dbContext.SaveChangesAsync();
              
            }
        }
    }
}
