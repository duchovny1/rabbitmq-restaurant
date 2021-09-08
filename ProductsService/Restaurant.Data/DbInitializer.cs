using Microsoft.EntityFrameworkCore;
using Restaurant.Data.Seeding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Data
{
    public class DbInitializer
    {
        private readonly ProductServiceDbContext _dbContext;
        public DbInitializer(ProductServiceDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task InitializeAsync()
        {
            // on the first executing of the application
            // this method will apply first migration that is going to create the database
            await ApplyMigrationsAsync();
            await SeedDataAsync();
        }

        private async Task ApplyMigrationsAsync()
        {

            if (_dbContext.Database.GetPendingMigrations().Any())
            {
                await _dbContext.Database.MigrateAsync();
            }
        }

        private async Task SeedDataAsync()
        {
            await new ApplicationSeeder().SeedAsync(this._dbContext);
        }
    }

}
