using ALMS.API.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ALMS.API.Data.Configurations
{
    public class MembershipConfiguration : IEntityTypeConfiguration<Membership>
    {
        public void Configure(EntityTypeBuilder<Membership> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasIndex(x => x.Id).IsUnique();

            builder.HasOne(x => x.User).WithOne(x => x.Membership).HasForeignKey<User>(x => x.MembershipId);
        }
    }
}
