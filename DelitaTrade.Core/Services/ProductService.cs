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
                Unit = p.Unit,
                Number = p.Number,
            }).ToArrayAsync();
        }
        public async Task<IEnumerable<ProductViewModel>> GetProductsAsync(string name)
        {
            return await GetFilteredReadonlyProduct(p => p.Name.Contains(name)).Select(p => new ProductViewModel
            {
                Name = p.Name,
                Unit = p.Unit,
                Number = p.Number,
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

        public async Task<IEnumerable<ProductViewModel>> GetFilteredProductsAsync(string[] args, int limit)
        {
            IQueryable<Product> query = repo.AllReadonly<Product>()
                .Where(p => p.Number != null);

            foreach (var arg in args)
            {
                query = query.Where(p => p.Name.Contains(arg)
                    || p.Unit.Contains(arg)
                    || (p.Number != null && p.Number.Contains(arg)));
            }
            string orderArg = string.Empty;
            
            if (args.Length > 0)
            {
                orderArg = args[0];
            }

            query = query.OrderByDescending(p => EF.Functions.Like(p.Name, $"{orderArg}%"))
                .ThenBy(p => p.Name)
                .Take(limit);

            return await query
                .Select(p => new ProductViewModel
                {
                    Name = p.Name,
                    Unit = p.Unit,
                    Number = p.Number,
                }).ToArrayAsync();
        }

        public async Task<int> AddRangeProductAsync(IEnumerable<ProductViewModel> dtoProducts)
        {
            foreach (var dtoProduct in dtoProducts) 
            {
                var product = await repo
                    .All<Product>()
                    .FirstOrDefaultAsync(p => p.Unit == dtoProduct.Unit && p.Name == dtoProduct.Name);
                if (product == null)
                {
                    await repo.AddAsync(new Product { Name = dtoProduct.Name, Unit = dtoProduct.Unit, Number = dtoProduct.Number });
                }
                else if (product.Number == null)
                {
                    product.Number = dtoProduct.Number;
                }

            }

            return await repo.SaveChangesAsync();
        }

        private IQueryable<Product> GetFilteredReadonlyProduct(Expression<Func<Product, bool>> predicate)
        {
            return repo.AllReadonly<Product>().Where(predicate);
        }

    }
}
