using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Prices.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Prices.Data.Configurations
{
    public class PriceCongifuration : IEntityTypeConfiguration<Price>
    {
        public void Configure(EntityTypeBuilder<Price> builder)
        {
            builder.Property(x => x.ProductName).IsRequired();
            builder.Property(x => x.ProductId).IsRequired();
            builder.Property(x => x.PriceAmount).HasColumnType("decimal(5,2)").IsRequired();
        }
    }
}
