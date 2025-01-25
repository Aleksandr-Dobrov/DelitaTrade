using Microsoft.EntityFrameworkCore;
using Mysqlx.Crud;
using System.Linq.Expressions;
using DelitaTrade.Core.Contracts;
using DelitaTrade.Core.ViewModels;
using DelitaTrade.Infrastructure.Common;
using DelitaTrade.Infrastructure.Data.Models;
using DelitaTrade.Common;

namespace DelitaTrade.Core.Services
{
    public class CompanyObjectService(IRepository repo) : ICompanyObjectService
    {
        public async Task<IEnumerable<CompanyObjectViewModel>> GetAllAsync()
        {
            return await repo.AllReadonly<CompanyObject>()
                .Where(o => o.IsActive)
                .Include(o => o.Company)
                .Include(o => o.Address)
                .Select(ParseToViewModel()).ToArrayAsync();
        }

        public async Task<IEnumerable<CompanyObjectViewModel>> GetFilteredByNameAsync(string arg, int limit = 100)
        {
            return await GetFilteredReadonlyObjects(o => o.IsActive && o.Name.Contains(arg))
                .Include(o => o.Company)
                .OrderByDescending(c => EF.Functions.Like(c.Name, $"{arg}%"))
                .ThenBy(c => c.Name)
                .Take(limit)
                .Select(ParseToViewModel()).ToArrayAsync();
        }

        public async Task<IEnumerable<CompanyObjectViewModel>> GetFilteredAsync(string arg, int limit = 100)
        {
            return await GetFilteredReadonlyObjects(o => o.IsActive && (o.Name.Contains(arg)
                                                    || o.Address.Town.Contains(arg)
                                                    || o.Address.StreetName.Contains(arg)
                                                    || o.Trader.Name.Contains(arg)
                                                    || o.Company.Name.Contains(arg)))
                .Include(o => o.Company)
                .Include(o => o.Address)
                .OrderByDescending(c => EF.Functions.Like(c.Name, $"{arg}%"))
                .ThenBy(c => c.Name)
                .Take(limit)
                .Select(ParseToViewModel()).ToArrayAsync();
        }

        public async Task<IEnumerable<CompanyObjectViewModel>> GetFilteredAsync(string arg, int companyId, int limit = 100)
        {
            return await GetFilteredReadonlyObjects(o => o.IsActive && o.CompanyId == companyId && (o.Name.Contains(arg)
                                                    || o.Address.Town.Contains(arg)
                                                    || o.Address.StreetName.Contains(arg)
                                                    || o.Trader.Name.Contains(arg)
                                                    || o.Company.Name.Contains(arg)))
                .Include(o => o.Company)
                .Include(o => o.Address)
                .OrderByDescending(c => EF.Functions.Like(c.Name, $"{arg}%"))
                .ThenBy(c => c.Name)
                .Take(limit)
                .Select(ParseToViewModel()).ToArrayAsync();
        }

        public async Task<CompanyObjectDeepViewModel> GetDetailedByIdAsync(int companyObjectId)
        {
            var detailedCompanyObject = await repo.AllReadonly<CompanyObject>()
                .Include(o => o.Company)
                .Include(o => o.Address)
                .Include(o => o.Trader)
                .Where(o => o.Id == companyObjectId)
                .FirstOrDefaultAsync()
                ?? throw new ArgumentNullException(ExceptionMessages.NotFound(nameof(CompanyObject)));

            var detailedCompanyObjectViewModel = new CompanyObjectDeepViewModel
            {
                Id = detailedCompanyObject.Id,
                Name = detailedCompanyObject.Name,
                Address = detailedCompanyObject.Address != null ? new AddressViewModel
                {
                    Id = detailedCompanyObject.Address.Id,
                    Town = detailedCompanyObject.Address.Town,
                    StreetName = detailedCompanyObject.Address.StreetName,
                    Number = detailedCompanyObject.Address.Number,
                    GpsCoordinates = detailedCompanyObject.Address.GpsCoordinates,
                    Description = detailedCompanyObject.Address.Description,
                } : null,
                IsBankPay = detailedCompanyObject.IsBankPay,
                Company = new CompanyViewModel
                {
                    Id = detailedCompanyObject.Company.Id,
                    Name = detailedCompanyObject.Company.Name,
                    Type = detailedCompanyObject.Company.Type ?? "",
                    Bulstad = detailedCompanyObject.Company.Bulstad ?? ""                    
                },
                Trader = new TraderViewModel
                {
                    Id = detailedCompanyObject.Trader.Id,
                    Name = detailedCompanyObject.Trader.Name,
                    PhoneNumber = detailedCompanyObject.Trader.PhoneNumber ?? ""
                }
            };
            return detailedCompanyObjectViewModel;
        }

        public async Task<int> CreateAsync(CompanyObjectDeepViewModel companyObject)
        {
            if (await GetFilteredReadonlyObjects(o => o.Name == companyObject.Name && o.Company.Name == companyObject.Company.Name)
                .Include(o => o.Company)
                .FirstOrDefaultAsync() != null) throw new ArgumentException(ExceptionMessages.IsExists(companyObject));

            var company = await repo.GetByIdAsync<Company>(companyObject.Company.Id)
                ?? throw new ArgumentNullException(ExceptionMessages.NotFound(nameof(Company)));

            var trader = await repo.GetByIdAsync<Trader>(companyObject.Trader.Id) 
                ?? throw new ArgumentNullException(ExceptionMessages.NotFound(nameof(Trader)));

            var newCompanyObject = new CompanyObject
            {
                Name = companyObject.Name,
                Address = companyObject.Address != null ? new Address
                {
                    Id = companyObject.Address.Id,
                    Town = companyObject.Address.Town,
                    StreetName = companyObject.Address.StreetName,
                    Number = companyObject.Address.Number,
                    GpsCoordinates = companyObject.Address.GpsCoordinates,
                    Description = companyObject.Address.Description,
                } : null,
                IsBankPay = companyObject.IsBankPay,
                Company = company,
                Trader = trader
            };
            await repo.AddAsync(newCompanyObject);
            await repo.SaveChangesAsync();
            await repo.ReloadAsync(newCompanyObject);
            return newCompanyObject.Id;
        }

        public async Task UpdateAsync(CompanyObjectViewModel companyObject)
        {
            var objectToUpdate = await repo.All<CompanyObject>()
                .Include(o => o.Address)
                .Include(o => o.Trader)
                .FirstOrDefaultAsync()
                ?? throw new ArgumentNullException(ExceptionMessages.NotFound(nameof(CompanyObject)));
            var trader = await repo.GetByIdAsync<Trader>(companyObject.Trader!.Id) ?? throw new ArgumentNullException(ExceptionMessages.NotFound(nameof(Trader)));
            if (companyObject.Address != null)
            {
                if (objectToUpdate.Address == null)
                {
                    objectToUpdate.Address = new Address
                    {
                        Town = companyObject.Address.Town,
                        StreetName = companyObject.Address?.StreetName,
                        Number = companyObject.Address?.Number,
                        GpsCoordinates= companyObject.Address?.GpsCoordinates,
                        Description = companyObject.Address?.Description,
                    };
                }
                else
                {
                    objectToUpdate.Address.Town = companyObject.Address.Town;
                    objectToUpdate.Address.StreetName = companyObject.Address.StreetName;
                    objectToUpdate.Address.Number = companyObject.Address.Number;
                    objectToUpdate.Address.GpsCoordinates = companyObject.Address.GpsCoordinates;
                    objectToUpdate.Address.Description = companyObject.Address.Description;
                }
            }
            if (companyObject.Trader.Id != objectToUpdate.Trader.Id)
            {
                objectToUpdate.Trader = trader;
            }

            objectToUpdate.IsBankPay = companyObject.IsBankPay;

            await repo.SaveChangesAsync();
        }

        public async Task DeleteSoftAsync(CompanyObjectViewModel companyObjectId)
        {
            var objectToRemove = await repo.GetByIdAsync<CompanyObject>(companyObjectId.Id) ?? throw new ArgumentNullException(ExceptionMessages.NotFound(nameof(CompanyObject)));

            objectToRemove.IsActive = false;
            await repo.SaveChangesAsync();
        }

        private IQueryable<CompanyObject> GetFilteredReadonlyObjects(Expression<Func<CompanyObject, bool>> filter)
        {
            return repo.AllReadonly<CompanyObject>().Where(filter);
        }

        private static Expression<Func<CompanyObject, CompanyObjectViewModel>> ParseToViewModel()
        {
            return o => new CompanyObjectViewModel
            {
                Id = o.Id,
                Name = o.Name,
                IsBankPay = o.IsBankPay,
                Address = o.Address != null ? new AddressViewModel
                {
                    Id = o.Address.Id,
                    Town = o.Address.Town,
                    StreetName = o.Address.StreetName,
                    Number = o.Address.Number,
                    GpsCoordinates = o.Address.GpsCoordinates,
                    Description = o.Address.Description,
                } : null,
                Company = new CompanyViewModel
                {
                    Id = o.Company.Id,
                    Name = o.Company.Name,
                    Type = o.Company.Type ?? "",
                    Bulstad = o.Company.Bulstad ?? ""
                }
            };
        }
    }
}
