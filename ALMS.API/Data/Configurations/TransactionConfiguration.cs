using ALMS.API.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ALMS.API.Data.Configurations
{
    public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasIndex(x => x.Id).IsUnique();

            builder.HasOne(x => x.User).WithMany(x => x.Transactions).HasForeignKey(x => x.UserId);

            builder.HasMany(x=>x.TransactionMediaList).WithOne(x=>x.Transaction).HasForeignKey(x => x.TransactionId);
        }
    }
}
