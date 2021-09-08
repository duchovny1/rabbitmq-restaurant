using Restaurant.ViewModels.Request;
using Restaurant.ViewModels.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Business.Contracts.Services
{
    public interface IProductService
    {
        /// <summary>
        /// Add an entity.
        /// <param name="entity">The entity instance.</param>
        /// </summary>
        Task AddAsync(MenuItemInfoRequest entity);

        /// <summary>
        /// Add a collection of entities
        /// </summary>
        /// <param name="entities">ollection of entities</param>
        /// <returns></returns>
        Task<int> AddRangeAsync(IEnumerable<MenuItemInfoRequest> entities);
        /// <summary>
        /// Update an entity.
        /// <param name="entity">The entity instance.</param>
        /// </summary>
        /// 
        Task UpdateAsync(MenuItemInfoRequest entity);

        /// <summary>
        /// Gets all entities.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<MenuItemInfoResponse>> GetAllByCategoryId(long categoryId);

        /// <summary>
        /// Get entity by identifier.
        /// </summary>
        /// <param name="id"></param>
        Task<MenuItemInfoResponse> GetByIdAsync(long id);

        /// <summary>
        /// Remove an entity.
        /// </summary>
        /// <param name="entity">the entity we are going to delete</param>
        /// <returns></returns>
        Task<bool> RemoveByIdAsync(long id);

        /// <summary>
        /// Retrieve the count of all entities.
        /// </summary>
        Task<int> CountAllAsync();

        /// <summary>
        /// Checks if the menu with Id exists in the db
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> IsMenuItemExistsAsync(long id);

        IEnumerable<CategoriesResponse> GetCategories();
    }
}
