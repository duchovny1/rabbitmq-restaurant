using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdersService.Data
{
    public class DbInitializer
    {
        private readonly OrdersDbContext _dbContext;
        public DbInitializer(OrdersDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task InitializeAsync()
        {
            // on the first executing of the application
            // this method will apply first migration that is going to create the database
            await ApplyMigrationsAsync();
        }

        private async Task ApplyMigrationsAsync()
        {

            if (_dbContext.Database.GetPendingMigrations().Any())
            {
                await _dbContext.Database.MigrateAsync();
            }
        }

       
    }
}
