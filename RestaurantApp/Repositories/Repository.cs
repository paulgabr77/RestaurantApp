using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using RestaurantApp.Data;

namespace RestaurantApp.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly IDbContextFactory<RestaurantDbContext> _contextFactory;

        public Repository(IDbContextFactory<RestaurantDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<T> GetByIdAsync(int id)
        {
            using var context = _contextFactory.CreateDbContext();
            return await context.Set<T>().FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            using var context = _contextFactory.CreateDbContext();
            return await context.Set<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            using var context = _contextFactory.CreateDbContext();
            return await context.Set<T>().Where(predicate).ToListAsync();
        }

        public async Task AddAsync(T entity)
        {
            using var context = _contextFactory.CreateDbContext();
            await context.Set<T>().AddAsync(entity);
            await context.SaveChangesAsync();
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            using var context = _contextFactory.CreateDbContext();
            await context.Set<T>().AddRangeAsync(entities);
            await context.SaveChangesAsync();
        }

        public void Update(T entity)
        {
            using var context = _contextFactory.CreateDbContext();
            context.Set<T>().Update(entity);
            context.SaveChanges();
        }

        public void Remove(T entity)
        {
            using var context = _contextFactory.CreateDbContext();
            context.Set<T>().Remove(entity);
            context.SaveChanges();
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            using var context = _contextFactory.CreateDbContext();
            context.Set<T>().RemoveRange(entities);
            context.SaveChanges();
        }
    }

}