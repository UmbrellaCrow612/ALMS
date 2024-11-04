using ALMS.API.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ALMS.API.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasIndex(x => x.Id).IsUnique();

            builder.HasOne(x => x.Membership).WithOne(x => x.User).HasForeignKey<User>(x => x.MembershipId);

            builder.HasMany(x => x.Payments).WithOne(x => x.User).HasForeignKey(x => x.UserId);

            builder.HasMany(x => x.Subscriptions).WithOne(x=>x.User).HasForeignKey(x=>x.UserId);

            builder.HasOne(x=>x.Account).WithOne(x=>x.User).HasForeignKey<Account>(x=>x.UserId);
        }
    }
}
