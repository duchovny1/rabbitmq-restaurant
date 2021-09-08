using Microsoft.EntityFrameworkCore;
using Prices.Data.Configurations;
using Prices.Domain.Models;

namespace Prices.Data
{
    public class PricesContext : DbContext
    {
        public DbSet<Price> Prices { get; set; }

        public DbSet<ChangesHistory> ChangesHistory { get; set;}

        public PricesContext()
            :base()
        {
        }

        public PricesContext(DbContextOptions<PricesContext> optionsBuilder)
            :base(optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ChangesHistoryConfiguration());
            modelBuilder.ApplyConfiguration(new PriceCongifuration());
        }
    }
}
