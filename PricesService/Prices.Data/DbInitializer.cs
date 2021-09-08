using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Prices.Data
{
    public class DbInitializer
    {
        private readonly PricesContext _dbContext;
        public DbInitializer(PricesContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task InitializeAsync()
        {
            // on the first executing of the application
            // this method will apply first migration which is going to create the database
            await ApplyMigrationsAsync();
            //await SeedDataAsync();
        }

        private async Task ApplyMigrationsAsync()
        {
            if (_dbContext.Database.GetPendingMigrations().Any())
            {
                await _dbContext.Database.MigrateAsync();
            }
        }

        //private async Task SeedDataAsync()
        //{
        //    await new ApplicationSeeder().SeedAsync(this._dbContext);
        //}
    }
}
