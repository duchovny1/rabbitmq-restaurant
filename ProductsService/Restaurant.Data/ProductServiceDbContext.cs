using Microsoft.EntityFrameworkCore;
using Restaurant.Domain.Models;
using Restaurant.Data.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Restaurant.Data
{
    public class ProductServiceDbContext : DbContext
    {
        public ProductServiceDbContext()
            : base()
        {
        }

        public ProductServiceDbContext(DbContextOptions<ProductServiceDbContext> options)
            : base(options)
        {
        }

        public DbSet<MenuItem> MenuItems { get; set; }

        public DbSet<Category> Categories { get; set; }


        public override int SaveChanges()
            => this.SaveChanges(true);

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(
            bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default)
        {
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new MenuItemConfiguration());
            builder.ApplyConfiguration(new CategoryConfiguration());
        }

    }
}
