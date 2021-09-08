using Microsoft.EntityFrameworkCore;
using Restaurant.Data.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Data
{
    public class RepositoryBase<TEntity> : IRepository<TEntity>
          where TEntity : class
    {
        public RepositoryBase(ProductServiceDbContext _context)
        {
            this.Context = _context ?? throw new ArgumentNullException(nameof(_context));
            this.DbSet = this.Context.Set<TEntity>();
        }

        protected DbSet<TEntity> DbSet { get; }

        protected ProductServiceDbContext Context { get; }

        public void Add(TEntity entity)
             => this.DbSet.Add(entity);

        public Task AddAsync(TEntity entity)
            => this.DbSet.AddAsync(entity).AsTask();

        public IQueryable<TEntity> All()
             => this.DbSet;

        public void Remove(TEntity entity)
           => this.DbSet.Remove(entity);

        public void Update(TEntity entity)
        {
            var entry = this.Context.Entry(entity);

            if (entry.State == EntityState.Detached)
            {
                this.DbSet.Attach(entity);
            }

            entry.State = EntityState.Modified;
        }

        public int SaveChanges()
            => this.Context.SaveChanges();

        public Task<int> SaveChangesAsync()
            => this.Context.SaveChangesAsync();
    }
}
