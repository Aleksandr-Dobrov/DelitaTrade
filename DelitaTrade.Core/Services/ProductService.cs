using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;
using DelitaTrade.Core.Contracts;
using DelitaTrade.Core.ViewModels;
using DelitaTrade.Infrastructure.Common;
using DelitaTrade.Infrastructure.Data.Models;

namespace DelitaTrade.Core.Services
{
    public class ProductService(IRepository repo) : IProductService
    {
        public async Task<IEnumerable<ProductViewModel>> GetAllAsync()
        {
            return await repo.AllReadonly<Product>().Select(p => new ProductViewModel
            {
                Name = p.Name,
                Unit = p.Unit
            }).ToArrayAsync();
        }
        public async Task<IEnumerable<ProductViewModel>> GetProductsAsync(string name)
        {
            return await GetFilteredReadonlyProduct(p => p.Name.Contains(name)).Select(p => new ProductViewModel
            {
                Name = p.Name,
                Unit = p.Unit
            }).ToArrayAsync();
        }

        public async Task AddProductAsync(ProductViewModel dtoProduct)
        {
            if (await repo.AllReadonly<Product>().FirstOrDefaultAsync(p => p.Unit == dtoProduct.Unit && p.Name == dtoProduct.Name) == null)
            {
                await repo.AddAsync(new Product { Name = dtoProduct.Name, Unit = dtoProduct.Unit });
                await repo.SaveChangesAsync();
            }
        }

        private IQueryable<Product> GetFilteredReadonlyProduct(Expression<Func<Product, bool>> predicate)
        {
            return repo.AllReadonly<Product>().Where(predicate);
        }
    }
}
