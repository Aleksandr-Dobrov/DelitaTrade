using Microsoft.EntityFrameworkCore;
using DelitaTrade.Core.Contracts;
using DelitaTrade.Core.ViewModels;
using DelitaTrade.Infrastructure.Common;
using DelitaTrade.Infrastructure.Data.Models;
using System.ComponentModel;


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

        public async Task<ReturnedProductDescriptionViewModel> AddDescription(ReturnedProductDescriptionViewModel description)
        {
            var dbDescription = await repo.AllReadonly<ReturnedProductDescription>().FirstOrDefaultAsync(d => d.Description == description.Description);
            if (dbDescription == null)
            {
                var newDescription = new ReturnedProductDescription { Description = description.Description };
                await repo.AddAsync(newDescription);
                await repo.SaveChangesAsync();
                await repo.ReloadAsync(newDescription);
                description.Id = newDescription.Id;
                return description;
            }
            else
            {
                description.Id = dbDescription.Id;
                return description;
            }
        }

    }
}
