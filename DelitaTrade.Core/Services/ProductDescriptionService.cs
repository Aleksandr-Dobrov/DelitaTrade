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

        public async Task<ReturnedProductDescriptionViewModel> AddDescriptionAsync(ReturnedProductDescriptionViewModel description)
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

        public async Task<IEnumerable<ReturnedProductDescriptionViewModel>> GetFilteredDescriptions(string[] args)
        {
            IQueryable<ReturnedProductDescription> query = repo.AllReadonly<ReturnedProductDescription>();

            foreach (var arg in args)
            {
                query = query.Where(d => d.Description.Contains(arg));
            }
            string orderArg = string.Empty;

            if (args.Length > 0)
            {
                orderArg = args[0];
            }

            query = query.OrderByDescending(d => EF.Functions.Like(d.Description, $"{orderArg}%"))
                .ThenBy(d => d.Description)
                .Take(10);

            return await query
                .Select(d => new ReturnedProductDescriptionViewModel 
                { 
                    Id = d.Id,
                    Description = d.Description
                }).ToArrayAsync();
        }
    }
}
