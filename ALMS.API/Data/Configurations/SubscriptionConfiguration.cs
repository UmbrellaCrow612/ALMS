using ALMS.API.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ALMS.API.Data.Configurations
{
    public class SubscriptionConfiguration : IEntityTypeConfiguration<Subscription>
    {
        public void Configure(EntityTypeBuilder<Subscription> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasIndex(x => x.Id).IsUnique();

            builder.HasOne(x=>x.User).WithMany(x => x.Subscriptions).HasForeignKey(x => x.UserId);
        }
    }
}
