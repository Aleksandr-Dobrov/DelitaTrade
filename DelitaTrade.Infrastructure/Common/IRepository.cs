using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace DelitaTrade.Infrastructure.Common
{
    public interface IRepository : IDisposable
    {
        IQueryable<T> All<T>() where T : class;
        
        IQueryable<T> AllReadonly<T>() where T : class;

        Task<T?> GetByIdAsync<T>(object id) where T : class; 

        Task AddAsync<T>(T entry) where T : class;

        Task AddRangeAsync<T>(IEnumerable<T> entries) where T : class;

        void Remove<T>(T entry) where T : class;

        void RemoveRange<T>(IEnumerable<T> entries) where T : class;

        void Update<T>(T entry) where T: class;

        Task ReloadAsync<T>(T entry) where T : class;
        Task Include<T, P>(T entry, Expression<Func<T, IEnumerable<P>>> property) where T : class where P : class;

        Task<int> SaveChangesAsync();
    }
}
