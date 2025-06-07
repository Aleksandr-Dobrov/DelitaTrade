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

            var descriptionCategory = await repo.GetByIdAsync<DescriptionCategory>(returnedProduct.DescriptionCategory.Id)
                ?? throw new ArgumentNullException(ExceptionMessages.NotFound(nameof(DescriptionCategory)));

            ReturnedProductDescription? description = null;
            if (returnedProduct.Description != null)
            {
                description = await repo.GetByIdAsync<ReturnedProductDescription>(returnedProduct.Description.Id);
            }

            var newProduct = new ReturnedProduct
            {
                Batch = returnedProduct.Batch,
                BestBefore = returnedProduct.BestBefore,
                Quantity = returnedProduct.Quantity,
                Product = product,
                Description = description,
                DescriptionCategory = descriptionCategory,
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
                .Include(p => p.DescriptionCategory)
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
                        Unit = p.Product.Unit,
                        Number = p.Product.Number,
                    },
                    Description = p.Description != null ? new ReturnedProductDescriptionViewModel
                    {
                        Id = p.Description.Id,
                        Description = p.Description.Description,
                    } : null,                    
                    DescriptionCategory = new DescriptionCategoryViewModel
                    {
                        Id = p.DescriptionCategory.Id,
                        Name = p.DescriptionCategory.Name,
                    }
                });
            }
            return outColl;
        }

        public async Task UpdateProductAsync(ReturnedProductViewModel returnedProduct)
        {
            var productToUpdate = await repo.GetByIdAsync<ReturnedProduct>(returnedProduct.Id)
                ?? throw new ArgumentNullException(ExceptionMessages.NotFound(nameof(ReturnedProduct)));

            var product = await repo.All<Product>().FirstOrDefaultAsync(p => p.Name == returnedProduct.Product.Name && p.Unit == returnedProduct.Product.Unit)
                ?? throw new ArgumentNullException(ExceptionMessages.NotFound(nameof(Product)));

            var descriptionCategory = await repo.GetByIdAsync<DescriptionCategory>(returnedProduct.DescriptionCategory.Id)
                ?? throw new ArgumentNullException(ExceptionMessages.NotFound(nameof(DescriptionCategory)));

            if (returnedProduct.Description != null)
            {
                var description = await repo.All<ReturnedProductDescription>()
                    .FirstOrDefaultAsync(d => d.Description == returnedProduct.Description.Description);
                productToUpdate.Description = description;
            }
            else
            {
                await repo.Include(productToUpdate, p => p.Description);
                productToUpdate.Description = null;
            }

            productToUpdate.Batch = returnedProduct.Batch;
            productToUpdate.BestBefore = returnedProduct.BestBefore;
            productToUpdate.Quantity = returnedProduct.Quantity;
            productToUpdate.Product = product;
            productToUpdate.DescriptionCategory = descriptionCategory;

            await repo.SaveChangesAsync();
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
