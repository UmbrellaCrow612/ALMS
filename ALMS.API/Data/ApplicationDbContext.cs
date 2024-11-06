using Microsoft.EntityFrameworkCore;
using ALMS.API.Data.Models;
using System.Reflection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ALMS.API.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
