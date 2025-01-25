using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using DelitaTrade.Core.Contracts;
using DelitaTrade.Core.ViewModels;
using DelitaTrade.Infrastructure.Common;
using DelitaTrade.Infrastructure.Data.Models;
using DelitaTrade.Common;

namespace DelitaTrade.Core.Services
{
    public class ReturnProtocolService(IRepository repo) : IReturnProtocolService
    {
        public async Task<IEnumerable<ReturnProtocolViewModel>> GetAllAsync(Guid userId)
        {
            return await repo.AllReadonly<ReturnProtocol>()
                .Where(r => r.UserId == userId) 
                .Include(r => r.Trader)
                .Include(r => r.Object)
                .ThenInclude(o => o.Address)
                .Include(r => r.Company)
                .Include(r => r.User)
                .Select(r => new ReturnProtocolViewModel
                {
                    Id = r.Id,
                    ReturnedDate = r.ReturnedDate,
                    PayMethod = r.PayMethod,
                    Trader = new TraderViewModel
                    {
                        Id = r.Trader.Id,
                        Name = r.Trader.Name
                    },
                    CompanyObject = new CompanyObjectViewModel 
                    {
                        Id = r.Object.Id,
                        Name = r.Object.Name,
                        Address = r.Object.Address != null ? new AddressViewModel
                        {
                            Id = r.Object.Address.Id,
                            Town = r.Object.Address.Town,
                            StreetName = r.Object.Address.StreetName,
                            Number = r.Object.Address.Number,
                            GpsCoordinates = r.Object.Address.GpsCoordinates,
                            Description = r.Object.Address.Description,
                        } : null,
                        Company = new CompanyViewModel 
                        {
                            Id = r.Company.Id,
                            Name = r.Company.Name,
                            Type = r.Company.Type ?? ""
                        }
                    },
                    User = new UserViewModel
                    {
                        Id = r.User.Id,
                        Name = r.User.Name
                    }
                }).ToArrayAsync();        
        }

        public async Task<IEnumerable<ReturnProtocolViewModel>> GetFilteredAsync(Guid userId, string arg)
        {
            return await GetFilteredReadonlyProtocol(userId,
                p => p.Company.Name.Contains(arg)
                    || p.Object.Name.Contains(arg) 
                    || p.Trader.Name.Contains(arg)
                    || p.ReturnedDate.ToString().Contains(arg))
                .Select(r => new ReturnProtocolViewModel
                {
                    Id = r.Id,
                    PayMethod = r.PayMethod,
                    ReturnedDate = r.ReturnedDate,
                    Trader = new TraderViewModel
                    {
                        Id = r.Trader.Id,
                        Name = r.Trader.Name
                    },
                    CompanyObject = new CompanyObjectViewModel
                    {
                        Id = r.Object.Id,
                        Name = r.Object.Name,
                        Address = r.Object.Address != null ? new AddressViewModel
                        {
                            Id = r.Object.Address.Id,
                            Town = r.Object.Address.Town,
                            StreetName = r.Object.Address.StreetName,
                            Number = r.Object.Address.Number,
                            GpsCoordinates = r.Object.Address.GpsCoordinates,
                            Description = r.Object.Address.Description,
                        } : null,
                        Company = new CompanyViewModel
                        {
                            Id = r.Object.CompanyId,
                            Name = r.Object.Company.Name,
                            Type = r.Object.Company.Type ?? ""
                        }
                    },
                    User = new UserViewModel
                    {
                        Id = r.User.Id,
                        Name = r.User.Name
                    }
                }).ToListAsync();
        }

        public async Task<int> CreateProtocolAsync(ReturnProtocolViewModel protocolViewModel)
        {
            var user = await repo.GetByIdAsync<User>(protocolViewModel.User.Id) 
                ?? throw new ArgumentNullException(ExceptionMessages.NotFound(nameof(User)));

            var trader = await repo.GetByIdAsync<Trader>(protocolViewModel.Trader.Id) 
                ?? throw new ArgumentNullException(ExceptionMessages.NotFound(nameof(Trader)));

            var company = await repo.GetByIdAsync<Company>(protocolViewModel.CompanyObject.Company.Id) 
                ?? throw new ArgumentNullException(ExceptionMessages.NotFound(nameof(Company)));

            var companyObject = await repo.GetByIdAsync<CompanyObject>(protocolViewModel.CompanyObject.Id) 
                ?? throw new ArgumentNullException(ExceptionMessages.NotFound(nameof(CompanyObject)));

            var newReturnProtocol = new ReturnProtocol
            {
                ReturnedDate = protocolViewModel.ReturnedDate,
                PayMethod = protocolViewModel.PayMethod,
                Trader = trader,
                Company = company,
                Object = companyObject,
                User = user
            };
            await repo.AddAsync(newReturnProtocol);
            await repo.SaveChangesAsync();
            await repo.ReloadAsync(newReturnProtocol);
            return newReturnProtocol.Id;
        }

        public async Task DeleteProtocol(int protocolId)
        {
            var protocolToDelete = await repo.GetByIdAsync<ReturnProtocol>(protocolId)
                ?? throw new ArgumentNullException(ExceptionMessages.NotFound(nameof(ReturnProtocol)));
            repo.Remove(protocolToDelete);
            await repo.SaveChangesAsync();
        }

        private IQueryable<ReturnProtocol> GetFilteredReadonlyProtocol(Guid userId ,Expression<Func<ReturnProtocol, bool>> filter)
        {
            return repo.AllReadonly<ReturnProtocol>()
                .Include(r => r.Object)
                .Include(r => r.Company)
                .Include(r => r.Trader)
                .Where(p => p.UserId == userId)
                .Where(filter);
        }
    }
}
