using Microsoft.EntityFrameworkCore;
using ALMS.API.Data.Models;
using System.Reflection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace ALMS.API.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        public DbSet<BorrowTransaction> BorrowTransactions { get; set; }
        public DbSet<InventoryLog> InventoryLogs { get; set; }
        public DbSet<Media> Medias { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<StripeProductEntity> StripeProducts { get; set; }
        public DbSet<StripeSession> StripeSessions { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<ForgotPasswordAttempt> ForgotPasswordAttempts { get; set; }
        public DbSet<YSubscription> YSubscriptions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        }
    }
}
