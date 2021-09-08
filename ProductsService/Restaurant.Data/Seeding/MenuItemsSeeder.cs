using Microsoft.EntityFrameworkCore;
using Restaurant.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Data.Seeding
{
    public class MenuItemsSeeder : ISeeder
    {
        public async Task SeedAsync(ProductServiceDbContext _dbContext)
        {
            if(await _dbContext.MenuItems.AnyAsync())
            {
                return;
            }

            await SeedSaladsAsync(_dbContext);
        }

        private async Task SeedSaladsAsync(ProductServiceDbContext _dbContext)
        {
            var categorySaladId = _dbContext.Categories.Where(x => x.Name == "Salad")
                .Select(x => x.Id)
                .FirstOrDefault();

            var critSalad = new MenuItem()
            {
                Name = "Krit Salad",
                Description = "Paksimadi, antoriro, domat, krastavica, luk, maslina, kapersi, rigan, zehtin",
                CategoryId = categorySaladId
            };

            var pancelaSalad = new MenuItem()
            {
                Name = "Salata Pancela",
                Description = "beleni domati, salatna mozarella, chabata, cherven luk, presen bosilek, zehtin",
                CategoryId = categorySaladId
            };

            var ceasearWithChickenAndBacon = new MenuItem()
            {
                Name = "Cesar s pile i bekon",
                Description = "Aisberg, chicken fillet, bacon chips, krutoni, parmezan, Ceasar Sauce",
                CategoryId = categorySaladId
            };

            var cartofenaSalata = new MenuItem()
            {
                Name = "Kartofena Salata",
                Description = "Vareni kartofi, sirene, qica, kiseli krastavichki, maioneza, presen luk",
                CategoryId = categorySaladId
            };

            _dbContext.MenuItems.Add(critSalad);
            _dbContext.MenuItems.Add(pancelaSalad);
            _dbContext.MenuItems.Add(ceasearWithChickenAndBacon);
            _dbContext.MenuItems.Add(cartofenaSalata);

            await _dbContext.SaveChangesAsync();
        }
    }
}
