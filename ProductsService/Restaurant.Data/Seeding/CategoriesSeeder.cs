using Microsoft.EntityFrameworkCore;
using Restaurant.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Restaurant.Data.Seeding
{
    public class CategoriesSeeder : ISeeder
    {
        public async Task SeedAsync(ProductServiceDbContext _dbContext)
        {
            if(await _dbContext.Categories.AnyAsync())
            {
                return;
            }

            List<Category> categories = new List<Category>()
            {
                 new Category() { Name = "Salad"},
                 new Category() { Name = "SoupAndBreads"},
                 new Category() { Name = "Pizza"},
                 new Category() { Name = "BurgersAndTortillas"},
                 new Category() { Name = "FishAppetizer"},
                 new Category() { Name = "HotAppetizer"},
                 new Category() { Name = "PastaAndRisotto"},
                 new Category() { Name = "FishDishes"},
                 new Category() { Name = "ChickenDishes"},
                 new Category() { Name = "PorkDishes"},
                 new Category() { Name = "AddingsToMainMeal"},
                 new Category() { Name = "Sauces"},
                 new Category() { Name = "Deserts"},
                 new Category() { Name = "Drinks"},
            };

            await _dbContext.Categories.AddRangeAsync(categories);
            await _dbContext.SaveChangesAsync();
        }
    }
}
