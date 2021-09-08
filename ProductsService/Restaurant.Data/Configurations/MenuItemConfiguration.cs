using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Restaurant.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurant.Data.Configurations
{
    public class MenuItemConfiguration : IEntityTypeConfiguration<MenuItem>
    {
        public void Configure(EntityTypeBuilder<MenuItem> builder)
        {
            builder.HasIndex(x => x.Name)
                 .IsUnique();

            builder.Property(x => x.Name)
                .HasMaxLength(35)
                .IsRequired();

            builder.Property(x => x.Description)
                .HasMaxLength(350);

            builder.HasOne(x => x.Category)
                .WithMany(x => x.MenuItems)
                .HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

