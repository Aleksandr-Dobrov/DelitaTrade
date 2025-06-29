using DelitaTrade.Core.Contracts;
using DelitaTrade.Core.ViewModels;
using DelitaTrade.Infrastructure.Common;
using DelitaTrade.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DelitaTrade.Common;

namespace DelitaTrade.Core.Services
{
    public class TraderService(IRepository repo) : ITraderService
    {
        public async Task<int> CreateAsync(TraderViewModel trader)
        {
            if (await repo.AllReadonly<Trader>().FirstOrDefaultAsync(t => t.Name == trader.Name) != null) throw new ArgumentException(ExceptionMessages.IsExists(trader));

            var newTrader = new Trader 
            {
                Name = trader.Name,
                PhoneNumber = trader.PhoneNumber
                
            };
            await repo.AddAsync(newTrader);
            await repo.SaveChangesAsync();
            await repo.ReloadAsync(newTrader);
            return newTrader.Id;
        }

        public async Task<IEnumerable<TraderViewModel>> GetAllAsync()
        {
            return await repo.AllReadonly<Trader>()
                .Where(t => t.IsActive)
                .Select(t => new TraderViewModel
            {
                Id = t.Id,
                Name = t.Name,
                PhoneNumber = t.PhoneNumber
            }).ToArrayAsync();
        }

        public async Task UpdateAsync(TraderViewModel traderViewModel)
        {            
            var traderToUpdate = await repo.GetByIdAsync<Trader>(traderViewModel.Id) ?? throw new ArgumentNullException(ExceptionMessages.NotFound(nameof(Trader)));

            traderToUpdate.PhoneNumber = traderViewModel.PhoneNumber;
            await repo.SaveChangesAsync();
        }

        public async Task DeleteSoftAsync(TraderViewModel traderViewModel)
        {
            var traderToDelete = await repo.GetByIdAsync<Trader>(traderViewModel.Id) ?? throw new ArgumentNullException(ExceptionMessages.NotFound(nameof(Trader)));

            traderToDelete.IsActive = false;
            await repo.SaveChangesAsync();
        }

        public async Task<TraderViewModel> GetByIdAsync(int id)
        {
            
            return await repo.AllReadonly<Trader>()
                .Where(t => t.Id == id && t.IsActive)
                .Select(t => new TraderViewModel
                {
                    Id = t.Id,
                    Name = t.Name,
                    PhoneNumber = t.PhoneNumber
                }).FirstOrDefaultAsync() ?? throw new ArgumentNullException(ExceptionMessages.NotFound(nameof(Trader)));
        }
    }
}
