using Microsoft.EntityFrameworkCore;
using DelitaTrade.Core.Contracts;
using DelitaTrade.Core.ViewModels;
using DelitaTrade.Infrastructure.Common;
using DelitaTrade.Infrastructure.Data.Models;


namespace DelitaTrade.Core.Services
{
    public class ProductDescriptionService(IRepository repo) : IProductDescriptionService
    {
        public async Task<IEnumerable<ReturnedProductDescriptionViewModel>> GetAllAsync()
        {
            return await repo.AllReadonly<ReturnedProductDescription>().Select(p => new ReturnedProductDescriptionViewModel
            {
                Id = p.Id,
                Description = p.Description
            }).ToArrayAsync();
        }

        public async Task<int> AddDescription(ReturnedProductDescriptionViewModel description)
        {
            var dbDescription = await repo.AllReadonly<ReturnedProductDescription>().FirstOrDefaultAsync(d => d.Description == description.Description);
            if (dbDescription == null)
            {
                var newDescription = new ReturnedProductDescription { Description = description.Description };
                await repo.AddAsync(newDescription);
                await repo.SaveChangesAsync();
                await repo.ReloadAsync(newDescription);
                return newDescription.Id;
            }
            else
            {
                return dbDescription.Id;
            }
        }

    }
}
