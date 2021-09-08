using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Restaurant.Data.Seeding
{
    public class ApplicationSeeder : ISeeder
    {

        public async Task SeedAsync(ProductServiceDbContext _dbContext)
        {
            if (_dbContext == null)
            {
                throw new ArgumentNullException(nameof(_dbContext));
            }

            var seeders = new List<ISeeder>()
            {
                new CategoriesSeeder(),
                new MenuItemsSeeder()
            };

            foreach (var seeder in seeders)
            {
                await seeder.SeedAsync(_dbContext);
            }
        }
    }

}
