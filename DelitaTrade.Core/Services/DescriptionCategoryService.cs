using DelitaTrade.Core.Contracts;
using DelitaTrade.Common;
using DelitaTrade.Core.ViewModels;
using DelitaTrade.Infrastructure.Common;
using DelitaTrade.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace DelitaTrade.Core.Services
{
    public class DescriptionCategoryService(IRepository repo) : IDescriptionCategoryService
    {
        public async Task<DescriptionCategoryViewModel> AddDescriptionCategoryAsync(DescriptionCategoryViewModel descriptionCategory)
        {
            var dbDescriptionCategory = await repo.AllReadonly<DescriptionCategory>().FirstOrDefaultAsync(d => d.Name == descriptionCategory.Name);
            if (dbDescriptionCategory == null)
            {
                var newDescriptionCategory = new DescriptionCategory { Name = descriptionCategory.Name };
                await repo.AddAsync(newDescriptionCategory);
                await repo.SaveChangesAsync();
                await repo.ReloadAsync(newDescriptionCategory);
                descriptionCategory.Id = newDescriptionCategory.Id;
                return descriptionCategory;
            }
            else
            {
                descriptionCategory.Id = dbDescriptionCategory.Id;
                return descriptionCategory;
            }
        }

        public async Task DeleteCategoryAsync(int id)
        {
            var dbDescriptionCategory = await repo.GetByIdAsync<DescriptionCategory>(id) 
                ?? throw new ArgumentNullException(ExceptionMessages.NotFound(nameof(DescriptionCategory)));
            repo.Remove(dbDescriptionCategory);
            await repo.SaveChangesAsync();
        }

        public async Task<IEnumerable<DescriptionCategoryViewModel>> GetAllAsync()
        {
            return await repo.AllReadonly<DescriptionCategory>().Select(p => new DescriptionCategoryViewModel
            {
                Id = p.Id,
                Name = p.Name
            }).ToArrayAsync();
        }

        public async Task<bool> IsHaveReferences(int id)
        {
            return await repo.AllReadonly<ReturnedProduct>()
                .AnyAsync(p => p.DescriptionCategory.Id == id);
        }

        public async Task UpdateCategoryAsync(DescriptionCategoryViewModel descriptionCategory)
        {
            var dbDescriptionCategory = await repo.GetByIdAsync<DescriptionCategory>(descriptionCategory.Id)
                ?? throw new ArgumentNullException(ExceptionMessages.NotFound(nameof(DescriptionCategory)));

            dbDescriptionCategory.Name = descriptionCategory.Name;
            await repo.SaveChangesAsync();
        }
    }
}
