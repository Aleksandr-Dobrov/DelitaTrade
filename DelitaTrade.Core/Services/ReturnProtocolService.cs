using DelitaTrade.Common;
using DelitaTrade.Core.Contracts;
using DelitaTrade.Core.ViewModels;
using DelitaTrade.Infrastructure.Common;
using DelitaTrade.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq.Expressions;
using System.Threading.Tasks;
using static DelitaTrade.Common.Constants.DelitaIdentityConstants.RoleNames;
using static DelitaTrade.Common.ExceptionMessages;

namespace DelitaTrade.Core.Services
{
    public class ReturnProtocolService(IRepository repo, UserManager<DelitaUser> userManager) : IReturnProtocolService
    {
        public async Task<IEnumerable<ReturnProtocolViewModel>> GetAllAsync(UserViewModel userViewModel)
        {
            return await repo.AllReadonly<ReturnProtocol>()
                .Where(r => r.IdentityUserId == userViewModel.Id)
                .Include(r => r.Trader)
                .Include(r => r.Object)
                .ThenInclude(o => o.Address)
                .Include(r => r.Company)
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
                    User = userViewModel
                }).ToArrayAsync();
        }

        public async Task<IEnumerable<ReturnProtocolViewModel>> GetFilteredAsync(UserViewModel userViewModel, string arg)
        {
            return await GetFilteredReadonlyProtocol(userViewModel.Id,
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
                    User = userViewModel
                }).ToListAsync();
        }

        public async Task<IEnumerable<ReturnProtocolViewModel>> GetFilteredAsync(UserViewModel user, string[] args)
        {
            IQueryable<ReturnProtocol> query = GetFilteredProtocolsQuery(user, args);
            return await query
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
                    User = user
                }).ToListAsync();
        }

        public async Task<IEnumerable<ReturnProtocolViewModel>> GetFilteredAsync(UserViewModel user, string[] arg, DateTime startDate, DateTime endDate)
        {
            IQueryable<ReturnProtocol> query = SetDateInterval(GetFilteredProtocolsQuery(user, arg), startDate, endDate);
            return await query
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
                    User = user
                }).ToListAsync();
        }


        public async Task<IEnumerable<SimpleReturnProtocolViewModel>> GetSimpleFilteredAsync(UserViewModel user, string? trader, string? companyObjectId, DateTime? startDate, DateTime? endDate)
        {
            IQueryable<ReturnProtocol> query = SetDateInterval(GetFilteredProtocolsQuery(user, user.Roles, trader, companyObjectId), startDate ?? DateTime.MinValue, endDate ?? DateTime.Now);
            return await query
                .Select(r => new SimpleReturnProtocolViewModel
                {
                    Id = r.Id,
                    PayMethod = r.PayMethod,
                    ReturnedDate = r.ReturnedDate,
                    TraderName = r.Trader.Name,
                    CompanyObjectName = $"{r.Object.Name} -> {r.Object.Company.Name} {r.Object.Company.Type}",
                    DriverName = $"{r.IdentityUser.Name} {r.IdentityUser.LastName}",
                    ApproverId = r.ApproverId,
                    ApproverName = r.Approver != null ? $"{r.Approver.Name} {r.Approver.LastName}" : null
                })
                .OrderByDescending(r => r.ReturnedDate)
                .ToListAsync();
        }

        public async Task<DetailReturnProtocolViewModel?> GetByIdAsync(UserViewModel user, int id)
        {
            IQueryable<ReturnProtocol> query;

            if (user.Roles.Contains(Admin) || user.Roles.Contains(WarehouseManager))
            {
                query = repo.AllReadonly<ReturnProtocol>();
            }
            else if (user.Roles.Contains(Driver))
            {
                query = repo.AllReadonly<ReturnProtocol>()
                    .Where(r => r.IdentityUserId == user.Id);
            }
            else
            {
                return null; // Unauthorized access
            }

            return await query
                .Where(r => r.Id == id)
                .Select(r => new DetailReturnProtocolViewModel
                {
                    Id = r.Id,
                    DriverName = $"{r.IdentityUser.Name} {r.IdentityUser.LastName}",
                    PayMethod = r.PayMethod,
                    ReturnedDate = r.ReturnedDate,
                    LastChange = r.LastChanged,
                    TraderName = r.Trader.Name,
                    CompanyObjectName = $"{r.Object.Name} -> {r.Object.Company.Name} {r.Object.Company.Type}",
                    ApproverId = r.ApproverId,
                    ApproverName = r.Approver != null ? $"{r.Approver.Name} {r.Approver.LastName}" : null,
                    ReturnedProducts = r.ReturnedProducts
                        .Select(p => new ReturnedProductViewModel
                        {
                            Id = p.Id,
                            Batch = p.Batch,
                            BestBefore = p.BestBefore,
                            Quantity = p.Quantity,
                            DescriptionCategory = new DescriptionCategoryViewModel
                            {
                                Id = p.DescriptionCategory.Id,
                                Name = p.DescriptionCategory.Name
                            },
                            Product = new ProductViewModel
                            {
                                Name = p.Product.Name,
                                Unit = p.Product.Unit,
                                Number = p.Product.Number
                            },
                            Description = p.Description != null ? new ReturnedProductDescriptionViewModel
                            {
                                Id = p.Description.Id,
                                Description = p.Description.Description
                            } : null,
                            IsScrapped = p.IsScrapped ?? false,
                            WarehouseDescription = p.WarehouseDescription
                        }).ToList(),
                }).FirstOrDefaultAsync();
        }

        public async Task<EditableReturnProtocolViewModel?> GetEditableByIdAsync(UserViewModel user, int id)
        {
            IQueryable<ReturnProtocol> query;

            if (user.Roles.Contains(Admin) || user.Roles.Contains(WarehouseManager))
            {
                query = repo.AllReadonly<ReturnProtocol>();
            }
            else if (user.Roles.Contains(Driver))
            {
                query = repo.AllReadonly<ReturnProtocol>()
                    .Where(r => r.IdentityUserId == user.Id);
            }
            else
            {
                return null; // Unauthorized access
            }
            return await query
                .Where(r => r.Id == id)
                .Select(r => new EditableReturnProtocolViewModel
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
                        Company = new CompanyViewModel
                        {
                            Id = r.Object.CompanyId,
                            Name = r.Object.Company.Name,
                            Type = r.Object.Company.Type ?? ""
                        },
                    },
                    ApproverName = r.Approver != null ? $"{r.Approver.Name} {r.Approver.LastName}" : null
                })
                .FirstOrDefaultAsync();
        }

        public async Task<int> CreateProtocolAsync(ReturnProtocolViewModel protocolViewModel)
        {
            var user = await GetUserAsync(userManager, protocolViewModel.User)
                ?? throw new InvalidOperationException(NotAuthenticate(protocolViewModel.User));

            var trader = await repo.GetByIdAsync<Trader>(protocolViewModel.Trader.Id)
                ?? throw new ArgumentNullException(NotFound(nameof(Trader)));

            var company = await repo.GetByIdAsync<Company>(protocolViewModel.CompanyObject.Company.Id)
                ?? throw new ArgumentNullException(NotFound(nameof(Company)));

            var companyObject = await repo.GetByIdAsync<CompanyObject>(protocolViewModel.CompanyObject.Id)
                ?? throw new ArgumentNullException(NotFound(nameof(CompanyObject)));

            var newReturnProtocol = new ReturnProtocol
            {
                LastChanged = DateTime.Now,
                ReturnedDate = protocolViewModel.ReturnedDate,
                PayMethod = protocolViewModel.PayMethod,
                Trader = trader,
                Company = company,
                Object = companyObject,
                IdentityUserId = protocolViewModel.User.Id,
                IdentityUser = user
            };
            await repo.AddAsync(newReturnProtocol);
            await repo.SaveChangesAsync();
            await repo.ReloadAsync(newReturnProtocol);
            return newReturnProtocol.Id;
        }

        public async Task UpdateProtocolAsync(ReturnProtocolViewModel returnProtocol)
        {
            var protocolToUpdate = await repo.GetByIdAsync<ReturnProtocol>(returnProtocol.Id)
                ?? throw new ArgumentNullException(NotFound(nameof(ReturnProtocol)));

            var trader = await repo.GetByIdAsync<Trader>(returnProtocol.Trader.Id)
                ?? throw new ArgumentNullException(NotFound(nameof(Trader)));

            var company = await repo.GetByIdAsync<Company>(returnProtocol.CompanyObject.Company.Id)
                ?? throw new ArgumentNullException(NotFound(nameof(Company)));

            var companyObject = await repo.GetByIdAsync<CompanyObject>(returnProtocol.CompanyObject.Id)
                ?? throw new ArgumentNullException(NotFound(nameof(CompanyObject)));

            if (protocolToUpdate.ApproverId.HasValue)
            {
                throw new InvalidOperationException(ProtocolIsApproved);
            }

            protocolToUpdate.LastChanged = DateTime.Now;
            protocolToUpdate.Trader = trader;
            protocolToUpdate.Company = company;
            protocolToUpdate.Object = companyObject;
            protocolToUpdate.ReturnedDate = returnProtocol.ReturnedDate;
            protocolToUpdate.PayMethod = returnProtocol.PayMethod;
            await repo.SaveChangesAsync();
        }

        public async Task ApproveAsync(ReturnProtocolApproveModel detailReturnProtocol, UserViewModel user)
        {
            if (user.Roles.Contains(WarehouseManager) == false)
            {
                throw new UnauthorizedAccessException(NotAuthenticate(user));
            }
            var protocolToApprove = await repo.GetByIdAsync<ReturnProtocol>(detailReturnProtocol.Id)
                ?? throw new ArgumentNullException(NotFound(nameof(ReturnProtocol)));

            if (IsDateTimeIdentical(detailReturnProtocol.LastChange, protocolToApprove.LastChanged) == false)
            {
                throw new InvalidOperationException(ProtocolIsChanged);
            }

            if (protocolToApprove.ApproverId.HasValue && protocolToApprove.ApproverId.Value != user.Id)
            {
                throw new UnauthorizedAccessException(NotAuthenticate(user));
            }

            await repo.IncludeCollection(protocolToApprove, p => p.ReturnedProducts);
            protocolToApprove.ApproverId = user.Id;

            foreach (var product in protocolToApprove.ReturnedProducts)
            {
                var approveModel = detailReturnProtocol.ReturnedProducts
                    .FirstOrDefault(p => p.Id == product.Id)
                    ?? throw new ArgumentNullException(NotFound(nameof(ReturnedProductApproveModel)));

                product.IsScrapped = approveModel.IsScraped;
                product.WarehouseDescription = approveModel.WarehouseDescription;
            }
            protocolToApprove.LastChanged = DateTime.Now;

            await repo.SaveChangesAsync();
        }

        public async Task DeleteProtocolAsync(int protocolId)
        {
            var protocolToDelete = await repo.GetByIdAsync<ReturnProtocol>(protocolId)
                ?? throw new ArgumentNullException(NotFound(nameof(ReturnProtocol)));

            if (protocolToDelete.ApproverId.HasValue)
            {
                throw new InvalidOperationException(ProtocolIsApproved);
            }

            repo.Remove(protocolToDelete);
            await repo.SaveChangesAsync();
        }

        public async Task<bool> IsApproved(int protocolId)
        {
            var returnProtocol = await repo.AllReadonly<ReturnProtocol>()
                .FirstOrDefaultAsync(p => p.Id == protocolId);
            return returnProtocol?.ApproverId.HasValue ?? false;
        }

        private IQueryable<ReturnProtocol> GetFilteredReadonlyProtocol(Guid userId, Expression<Func<ReturnProtocol, bool>> filter)
        {
            return repo.AllReadonly<ReturnProtocol>()
                .Include(r => r.Object)
                .Include(r => r.Company)
                .Include(r => r.Trader)
                .Where(p => p.IdentityUserId == userId)
                .Where(filter);
        }

        private IQueryable<ReturnProtocol> GetFilteredProtocolsQuery(UserViewModel user, string[] args)
        {
            IQueryable<ReturnProtocol> query = repo.AllReadonly<ReturnProtocol>()
                            .Where(r => r.IdentityUserId == user.Id);
            foreach (var arg in args)
            {
                query = query.Where(p => p.Company.Name.Contains(arg)
                     || p.Object.Name.Contains(arg)
                     || p.Trader.Name.Contains(arg)
                     || p.ReturnedDate.ToString().Contains(arg));
            }

            return query;
        }

        private IQueryable<ReturnProtocol> GetFilteredProtocolsQuery(UserViewModel user, IEnumerable<string> roles, string? trader, string? companyObjectId)
        {
            IQueryable<ReturnProtocol> query;

            if (roles.Contains(Admin) || roles.Contains(WarehouseManager))
            {
                query = repo.AllReadonly<ReturnProtocol>();
            }
            else if (roles.Contains(Driver))
            {
                query = repo.AllReadonly<ReturnProtocol>()
                    .Where(r => r.IdentityUserId == user.Id);
            }
            else
            {
                throw new UnauthorizedAccessException(NotAuthenticate(user));
            }

            return query.Where(r => r.Trader.Name.Contains(trader ?? string.Empty)
                                && r.Object.Name.Contains(companyObjectId ?? string.Empty));
        }

        private IQueryable<ReturnProtocol> SetDateInterval(IQueryable<ReturnProtocol> query, DateTime startDate, DateTime endDate)
        {
            return query.Where(p => p.ReturnedDate.Date >= startDate.Date && p.ReturnedDate <= endDate.Date);
        }

        private async Task<DelitaUser> GetUserAsync(UserManager<DelitaUser> userManager, UserViewModel user)
        {
            return await userManager.FindByIdAsync(user.Id.ToString()) ??
                throw new InvalidOperationException(NotAuthenticate(user));
        }

        private static bool IsDateTimeIdentical(DateTime? first, DateTime? second)
        {
            if (first == null || second == null)
            {
                return true;
            }

            return first.Value.Year == second.Value.Year
                && first.Value.Month == second.Value.Month
                && first.Value.Day == second.Value.Day
                && first.Value.Hour == second.Value.Hour
                && first.Value.Minute == second.Value.Minute
                && first.Value.Second == second.Value.Second;
        }
    }
}
