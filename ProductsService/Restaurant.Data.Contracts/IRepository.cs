using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Data.Contracts
{
    public interface IRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// Add an entity.
        /// <param name="entity">The entity instance.</param>
        /// </summary>
        void Add(TEntity entity);

        /// <summary>
        /// Add an entity asynchronous.
        /// <param name="entity">The entity instance.</param>
        /// </summary>
        Task AddAsync(TEntity entity);

        /// <summary>
        /// Gets all entities.
        /// </summary>
        /// <returns></returns>
        IQueryable<TEntity> All();

        /// <summary>
        /// Remove an entity.
        /// </summary>
        /// <param name="entity">the entity we are going to delete</param>
        /// <returns></returns>
        void Remove(TEntity entity);

        /// <summary>
        /// Update an entity
        /// </summary>
        /// <param name="entity">the entity we are going to modify</param>
        void Update(TEntity entity);

        /// <summary>
        /// Save changes to the database.
        /// </summary>
        /// <returns>Number of affected rows</returns>
        int SaveChanges();

        /// <summary>
        /// Save changes to the database asynchronous
        /// </summary>
        /// <returns>Number of affected rows</returns>
        Task<int> SaveChangesAsync();
    }
}
