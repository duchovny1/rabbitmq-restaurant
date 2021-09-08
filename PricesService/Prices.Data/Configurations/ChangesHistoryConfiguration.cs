using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Prices.Domain.Models;

namespace Prices.Data.Configurations
{
    public class ChangesHistoryConfiguration : IEntityTypeConfiguration<ChangesHistory>
    {
        public void Configure(EntityTypeBuilder<ChangesHistory> builder)
        {
            builder.Property(x => x.PreviousPrice).HasColumnType("decimal(5,2)").IsRequired();

            builder.Property(x => x.NewPrice).HasColumnType("decimal(5,2)").IsRequired();

            builder.Property(x => x.ProductName).IsRequired();

            builder.Property(x => x.ProductId).IsRequired();
        }
    }
}
