using Microsoft.EntityFrameworkCore;
using OrdersService.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrdersService.Data
{
    public class OrdersDbContext : DbContext
    {
        public OrdersDbContext() : base()
        {
        }

        public OrdersDbContext(DbContextOptions<OrdersDbContext> options)
            :base(options)
        {
        }

        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new OrderConfiguration());
        }


    }
}
