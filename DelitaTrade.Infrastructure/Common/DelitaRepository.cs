using DelitaTrade.Infrastructure.Data;
using DelitaTrade.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace DelitaTrade.Infrastructure.Common
{
    public class DelitaRepository(DelitaDbContext dbContext) : IRepository
    {
        protected DbSet<T> DbSet<T>() where T : class
        {
            return dbContext.Set<T>();
        }

        public async Task AddAsync<T>(T entry) where T : class
        {
            await DbSet<T>()
                .AddAsync(entry);
        }

        public async Task AddRangeAsync<T>(IEnumerable<T> entries) where T : class
        {
            await DbSet<T>()
                .AddRangeAsync(entries);
        }

        public IQueryable<T> All<T>() where T : class
        {
            return DbSet<T>();                
        }

        public IQueryable<T> AllReadonly<T>() where T : class
        {
            return DbSet<T>()
                .AsNoTracking();
        }

        public void Remove<T>(T entry) where T : class
        {
            DbSet<T>().Remove(entry);
        }

        public void RemoveRange<T>(IEnumerable<T> entries) where T : class
        {
            DbSet<T>().RemoveRange(entries);
        }

        public async Task<T?> GetByIdAsync<T>(object id) where T : class
        {
            return await DbSet<T>()
                .FindAsync(id);
        }

        public void Update<T>(T entry) where T : class
        {
            DbSet<T>().Update(entry);
        }

        public Task<int> SaveChangesAsync()
        {
            return dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            dbContext.Dispose();
        }

        public async Task ReloadAsync<T>(T entry) where T : class
        {
            await dbContext.Entry(entry).ReloadAsync();
        }

        public async Task IncludeCollection<TEntry, TReference>(TEntry entry, Expression<Func<TEntry, IEnumerable<TReference>>> property) where TEntry : class where TReference : class
        {
            await dbContext.Entry(entry)
                .Collection(property)
                .LoadAsync();
        }

        public async Task Include<T, P>(T entry, Expression<Func<T,P?>> property)
            where T : class
            where P : class
        {
            await dbContext.Entry(entry)
                .Reference(property)
                .LoadAsync();
        }
    }
}
