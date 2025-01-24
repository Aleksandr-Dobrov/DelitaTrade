using Microsoft.EntityFrameworkCore;
using DelitaTrade.Core.Contracts;
using DelitaTrade.Core.ViewModels;
using DelitaTrade.Infrastructure.Common;
using DelitaTrade.Infrastructure.Data.Models;
using DelitaTrade.Common;

namespace DelitaTrade.Core.Services
{
    public class ReturnProductService(IRepository repo) : IReturnProductService
    {
        public async Task<int> AddProductAsync(ReturnedProductViewModel returnedProduct, int protocolId)
        {
            var protocol = await repo.GetByIdAsync<ReturnProtocol>(protocolId) 
                ?? throw new ArgumentNullException(ExceptionMessages.NotFound(nameof(ReturnProtocol)));

            var product = await repo.All<Product>().FirstOrDefaultAsync(p => p.Name == returnedProduct.Product.Name && p.Unit == returnedProduct.Product.Unit)
                ?? throw new ArgumentNullException(ExceptionMessages.NotFound(nameof(Product)));

            var description = await repo.GetByIdAsync<ReturnedProductDescription>(returnedProduct.Description.Id)
                ?? throw new ArgumentNullException(ExceptionMessages.NotFound(nameof(ReturnedProductDescription)));

            var newProduct = new ReturnedProduct
            {
                Batch = returnedProduct.Batch,
                BestBefore = returnedProduct.BestBefore,
                Quantity = returnedProduct.Quantity,
                Product = product,
                Description = description,
                ReturnProtocolId = protocol.Id
            };
            await repo.AddAsync(newProduct);
            await repo.SaveChangesAsync();
            await repo.ReloadAsync(newProduct);
            return newProduct.Id;            
        }

        public async Task<IEnumerable<ReturnedProductViewModel>> GetAllProductsAsync(int protocolId)
        {
            var returnedProducts = await repo.AllReadonly<ReturnedProduct>()
                .Where(p => p.ReturnProtocolId == protocolId)
                .Include(p => p.Product)
                .Include(p => p.Description)
                .ToListAsync();
            List<ReturnedProductViewModel> outColl = [];
            foreach (var p in returnedProducts)
            {
                outColl.Add(new ReturnedProductViewModel
                {
                    Id = p.Id,
                    Batch = p.Batch,
                    BestBefore = p.BestBefore,
                    Quantity = p.Quantity,
                    Product = new ProductViewModel
                    {
                        Name = p.Product.Name,
                        Unit = p.Product.Unit
                    },
                    Description = new ReturnedProductDescriptionViewModel
                    {
                        Id = p.Description.Id,
                        Description = p.Description.Description,
                    }
                });
            }
            return outColl;
        }

        public async Task DeleteProductAsync(int productId)
        {
            var productToRemove = await repo.GetByIdAsync<ReturnedProduct>(productId) 
                ?? throw new ArgumentNullException(ExceptionMessages.NotFound(nameof(ReturnedProduct)));
            repo.Remove(productToRemove);
            await repo.SaveChangesAsync();
        }
    }
}
