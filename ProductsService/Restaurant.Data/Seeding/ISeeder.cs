using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Data.Seeding
{
    public interface ISeeder
    {
        Task SeedAsync(ProductServiceDbContext _dbContext);
    }
}
