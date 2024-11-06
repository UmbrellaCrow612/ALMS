using ALMS.API.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ALMS.API.Data.Configurations
{
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasIndex(x => x.Id).IsUnique();

            builder.HasOne(x => x.User).WithMany(x => x.Payments).HasForeignKey(x => x.UserId);

            builder.HasMany(x => x.Transactions).WithOne(x => x.Payment).HasForeignKey(x => x.PaymentId);
        }
    }
}
