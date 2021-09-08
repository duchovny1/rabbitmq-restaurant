using Microsoft.EntityFrameworkCore;
using Restaurant.Business.Contracts.Services;
using Restaurant.Data.Contracts;
using Restaurant.Domain.Models;
using Restaurant.ViewModels.Request;
using Restaurant.ViewModels.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Business
{
    public class ProductService : IProductService
    {
        private readonly IRepository<MenuItem> _repository;
        private readonly IRepository<Category> _categoryRepository;

        public ProductService(IRepository<MenuItem> repository,
                             IRepository<Category> _categoryRepository)
        {
            this._repository = repository;
            this._categoryRepository = _categoryRepository;
        }

        public async Task AddAsync(MenuItemInfoRequest entity)
        {
            bool isMenuItemExist = await _repository.All()
                .AnyAsync(x => x.Id == entity.Id);

            if (isMenuItemExist)
            {
                throw new ArgumentException();
            }

            MenuItem menuItem = new MenuItem()
            {
                Name = entity.Name,
                Description = entity.Description,
                IsEnabled = entity.IsEnabled == "true" ? true : false,
                PictureURL = entity.Name,
            };

            if (entity.CategoryId != null)
            {
                bool isCategoryExist = await _repository.All().AnyAsync(x => x.CategoryId == entity.CategoryId);

                if (!isCategoryExist)
                {
                    /// send message that category does not exists.
                }
            }

            await this._repository.AddAsync(menuItem);
            await this._repository.SaveChangesAsync();
        }

        public async Task<int> AddRangeAsync(IEnumerable<MenuItemInfoRequest> entities)
        {
            int totalAddedEntities = 0;

            foreach (var entity in entities)
            {
                try
                {
                    await this.AddAsync(entity);
                }
                catch (ArgumentException)
                {
                    continue;
                }

                totalAddedEntities++;
            }

            return totalAddedEntities;
        }

        public async Task<int> CountAllAsync()
         => await this._repository.All().CountAsync();

        public async Task<IEnumerable<MenuItemInfoResponse>> GetAllByCategoryId(long id)
         => await this._repository.All()
                .Where(x => x.CategoryId == id)
                .Select(x => new MenuItemInfoResponse
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    PictureURL = x.PictureURL,
                })
            .ToListAsync();

        public async Task<MenuItemInfoResponse> GetByIdAsync(long id)
        {
            var menuItem = await this._repository.All().FirstOrDefaultAsync(x => x.Id == id);

            if (menuItem == null)
            {
                return null;
            }

            var responseModel = new MenuItemInfoResponse
            {
                Id = menuItem.Id,
                Name = menuItem.Name,
                Description = menuItem.Description,
                PictureURL = menuItem.PictureURL,
            };

            return responseModel;
        }

        public async Task<bool> RemoveByIdAsync(long id)
        {
            var menuItemToRove = await this._repository.All().FirstOrDefaultAsync(x => x.Id == id);

            if (menuItemToRove == null)
            {
                return false;
            }

            this._repository.Remove(menuItemToRove);
            await this._repository.SaveChangesAsync();

            return true;
        }

        public async Task UpdateAsync(MenuItemInfoRequest entity)
        {
            var menuItemToUpdate = await this._repository.All().FirstOrDefaultAsync(x => x.Id == entity.Id);

            //if (menuItemToUpdate == null)
            //{
            //    throw new ArgumentException(
            //        string.Format(ExceptionMessages.MenuItemDoesNotExist, entity.Id));
            //}

            //menuItemToUpdate.Name = entity.Name;
            //menuItemToUpdate.MenuItemType = this.GetMenuItemType(entity.MenuItemType.ToLower());
            //menuItemToUpdate.PictureURL = entity.PictureURL;
            //menuItemToUpdate.Price = entity.Price;
            //menuItemToUpdate.Description = entity.Description;
            //menuItemToUpdate.IsEnabled = entity.IsEnabled == "true" ? true : false;

            //this._repository.Update(menuItemToUpdate);
            this._repository.SaveChanges();
        }

        public Task<bool> IsMenuItemExistsAsync(long id)
          => this._repository.All().AnyAsync(x => x.Id == id);

        public IEnumerable<CategoriesResponse> GetCategories()
        {
            var categories = this._categoryRepository.All().Select(x => new CategoriesResponse { 
                Id = x.Id, 
                Category = x.Name
            }).ToArray();


           return categories;
        }

        //private MenuItemType GetMenuItemType(string type)
        //{
        //    MenuItemType menuItemType;
        //    switch (type)
        //    {
        //        case "salad": menuItemType = MenuItemType.Salad; break;
        //        case "nonalcoholic": menuItemType = MenuItemType.NonAlcholicDrink; break;
        //        case "alcoholic": menuItemType = MenuItemType.AlcoholicDrink; break;
        //        case "junkfood": menuItemType = MenuItemType.JunkFood; break;
        //        case "maindish": menuItemType = MenuItemType.MainDish; break;
        //        case "appetizer": menuItemType = MenuItemType.Appetizer; break;
        //        case "soup": menuItemType = MenuItemType.Soup; break;
        //        case "pizza": menuItemType = MenuItemType.Pizza; break;
        //        case "desert": menuItemType = MenuItemType.Desert; break;
        //        default: menuItemType = MenuItemType.Rest; break;
        //    };

        //    return menuItemType;
        //}
    }
}
