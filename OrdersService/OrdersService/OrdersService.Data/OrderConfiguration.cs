using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrdersService.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrdersService.Data
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasMany(x => x.Products).WithOne(x => x.Order).HasForeignKey(x => x.OrderId);

            builder.Property(x => x.OrderNumber).ValueGeneratedOnAdd();
        }
    }
}
