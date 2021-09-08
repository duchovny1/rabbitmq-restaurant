using Microsoft.EntityFrameworkCore;
using PaymentService.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentService.Data
{
    public class PaymentDbContext : DbContext
    {
        public PaymentDbContext() : base()
        {
        }

        public PaymentDbContext(DbContextOptions<PaymentDbContext> options) : base(options)
        {
        }

        public DbSet<Payment> Payments { get; set; }
        public DbSet<User> Users { get; set; }  

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Payment>().Property(x => x.OrderNumber).IsRequired();
            modelBuilder.Entity<Payment>().Property(x => x.PaymentMethod).IsRequired();
            modelBuilder.Entity<Payment>().Property(x => x.Price).HasColumnType("decimal(5,2)");
           

            modelBuilder.Entity<Payment>().HasOne(x => x.User).WithMany(x => x.Payments).HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            
        }
    }
}
