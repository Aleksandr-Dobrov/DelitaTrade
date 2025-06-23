using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using DelitaTrade.Core.Contracts;
using DelitaTrade.Core.ViewModels;
using DelitaTrade.Infrastructure.Common;
using DelitaTrade.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Mvc;
using DelitaTrade.Common;

namespace DelitaTrade.Core.Services
{
    public class CompanyService(IRepository repo) : ICompanyService
    {
        public async Task<IEnumerable<CompanyViewModel>> GetAllAsync()
        {
            return await repo.AllReadonly<Company>()
                .Where(c => c.IsActive)
                .Select(c => new CompanyViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    Type = c.Type ?? "",
                    Bulstad = c.Bulstad ?? ""
                }).ToArrayAsync();
        }

        public async Task<IEnumerable<CompanyViewModel>> GetFilteredByNameAsync(string arg, int limit = 100)
        {
            return await GetFilteredReadonlyCompany(c => c.IsActive && c.Name.Contains(arg))
                .OrderByDescending(c => EF.Functions.Like(c.Name, $"{arg}%"))
                .ThenBy(c => c.Name)
                .Take(limit)
                .Select(c => new CompanyViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    Type = c.Type ?? "",
                    Bulstad = c.Bulstad ?? ""
                }).ToArrayAsync();
        }
        public async Task<IEnumerable<CompanyViewModel>> GetFilteredAsync(string arg, int limit = 100)
        {            
            return await GetFilteredReadonlyCompany(c => c.IsActive && (c.Name.Contains(arg)
                                         || c.Type.Contains(arg)
                                         || c.Bulstad.Contains(arg)))
                .OrderByDescending(c => EF.Functions.Like(c.Name, $"{arg}%"))
                .ThenBy(c => c.Name)
                .Take(limit)
                .Select(c => new CompanyViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    Type = c.Type ?? "",
                    Bulstad = c.Bulstad ?? ""
                }).ToArrayAsync();
        }

        public async Task<CompanyViewModel> GetDetailedCompanyByIdAsync(int companyId)
        {
            var company = await repo.AllReadonly<Company>()
                .Include(c => c.Objects)
                .ThenInclude(c => c.Trader)
                .Include(c => c.Objects)
                .ThenInclude(o => o.Address)
                .Where(c => c.Id == companyId)
                .FirstOrDefaultAsync() ?? throw new ArgumentNullException(ExceptionMessages.NotFound(nameof(Company)));
                        
            var detailedCompany = new CompanyViewModel
            {
                Id = company.Id,
                Name = company.Name,
                Type = company.Type ?? "",
                Bulstad = company.Bulstad ?? ""
            }; 
            foreach (var obj in company.Objects)
            {
                detailedCompany.CompanyObjects.Add(new CompanyObjectViewModel
                {
                    Id = obj.Id,
                    Name = obj.Name,
                    Address = obj.Address != null ? new AddressViewModel
                    {
                        Id = obj.Address.Id,
                        Town = obj.Address.Town,
                        StreetName = obj.Address.StreetName,
                        Number = obj.Address.Number,
                        GpsCoordinates = obj.Address.GpsCoordinates,
                        Description = obj.Address.Description,
                    } : null,
                    IsBankPay = obj.IsBankPay,
                    Company = detailedCompany,
                    Trader = new TraderViewModel
                    {
                        Id = obj.Trader == null ? DelitaDbConstants.DefaultTraderId : obj.Trader.Id,
                        Name = obj.Trader == null ? DelitaDbConstants.DefaultTraderName : obj.Trader.Name,
                        PhoneNumber = obj.Trader == null || obj.Trader.PhoneNumber == null ? "" : obj.Trader.PhoneNumber
                    }
                });
            }

            return detailedCompany;
        }

        public async Task<int> CreateAsync(CompanyViewModel company)
        {
            if (await repo.All<Company>().FirstOrDefaultAsync(c => c.Name == company.Name) != null) throw new ArgumentException(ExceptionMessages.IsExists(company));

            var newCompany = new Company
            {
                Name = company.Name,
                Type = company.Type,
                Bulstad = company.Bulstad
            };
            await repo.AddAsync(newCompany);
            await repo.SaveChangesAsync();
            await repo.ReloadAsync(newCompany);
            return newCompany.Id;
        }

        public async Task UpdateAsync(CompanyViewModel company)
        {
            var companyToUpdate = await repo.GetByIdAsync<Company>(company.Id) ?? throw new ArgumentNullException(ExceptionMessages.NotFound(nameof(Company)));
            
            companyToUpdate.Name = company.Name;
            companyToUpdate.Type = company.Type;
            companyToUpdate.Bulstad = company.Bulstad;
            await repo.SaveChangesAsync();
        }

        public async Task DeleteSoftAsync(CompanyViewModel company)
        {
            var companyToRemove = await repo.GetByIdAsync<Company>(company.Id) ?? throw new ArgumentNullException(ExceptionMessages.NotFound(nameof(Company)));
            await repo.IncludeCollection(companyToRemove, c => c.Objects);
            foreach (var obj in companyToRemove.Objects)
            {
                obj.IsActive = false;                
            }
            await repo.SaveChangesAsync();
        }

        private IQueryable<Company> GetFilteredReadonlyCompany(Expression<Func<Company, bool>> filter)
        {
            return repo.AllReadonly<Company>().Where(filter);
        }
    }
}
