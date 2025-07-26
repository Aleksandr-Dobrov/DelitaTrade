using DelitaTrade.Common;
using DelitaTrade.Common.Enums;
using DelitaTrade.Core.Comparers;
using DelitaTrade.Core.Contracts;
using DelitaTrade.Core.Extensions;
using DelitaTrade.Core.Models.ImportModels;
using DelitaTrade.Core.ViewModels;
using DelitaTrade.Core.ViewModels.DayReportModels;
using DelitaTrade.Core.ViewModels.DeliveryModels;
using DelitaTrade.Infrastructure.Common;
using DelitaTrade.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using static DelitaTrade.Common.Constants.DelitaIdentityConstants.RoleNames;
using static DelitaTrade.Common.Constants.CompanyConstants;
using static DelitaTrade.Common.DelitaDbConstants;
using static DelitaTrade.Common.ExceptionMessages;

namespace DelitaTrade.Core.Services
{
    public class DayReportService(IRepository repo, UserManager<DelitaUser> userManager) : BaseService, IDayReportService
    {
        public async Task<DayReportViewModel> CreateAsync(DayReportViewModel dayReport)
        {
            var user = await GetUserAsync(userManager, dayReport.User);
            var newDayReport = new DayReport()
            {   
                IdentityUser = user,
                IdentityUserId = dayReport.User.Id,
                Date = dayReport.Date,
                Banknotes = dayReport.Banknotes,
                TotalCash = dayReport.TotalCash,
                TotalAmount = dayReport.TotalAmount,
                TotalIncome = dayReport.TotalIncome,
                TotalNotPay = dayReport.TotalNotPay,
                TotalOldInvoice = dayReport.TotalOldInvoice,
                TotalExpense = dayReport.TotalExpense,
                TotalWeight = dayReport.TotalWeight,
                TransmissionDate = dayReport.TransmissionDate,
                VehicleId = dayReport.Vehicle != null && dayReport.Vehicle.Id != 0 ? dayReport.Vehicle.Id : null
            };
            await repo.AddAsync(newDayReport);
            await repo.SaveChangesAsync();
            await repo.ReloadAsync(newDayReport);
            dayReport.Id = newDayReport.Id;
            return dayReport;
        }

        public async Task DeleteAsync(UserViewModel userViewModel, int id)
        {
            var dayReport = await repo.All<DayReport>()
                .Include(d => d.Deliveries)
                .Include(d => d.Invoices)
                .ThenInclude(i => i.Invoice)
                .ThenInclude(i => i.InvoicesInDayReports)
                .FirstOrDefaultAsync(d => d.Id == id) ?? throw new ArgumentNullException(NotFound(nameof(DayReport)));

            if (IsAtLeastInOneRole(userViewModel, Admin) == false && dayReport.IdentityUserId != userViewModel.Id) throw new InvalidOperationException(NotAuthenticate(userViewModel));

            if (dayReport.Invoices.Count > 0)
            {
                foreach (var invoiceInDayReport in dayReport.Invoices)
                {
                    if (invoiceInDayReport.Invoice.InvoicesInDayReports.Count == 0) throw new InvalidDataException($"Inner {nameof(Invoice)} must have at least one {nameof(InvoiceInDayReport)}");
                    
                    if (invoiceInDayReport.Invoice.InvoicesInDayReports.Count > 1)
                    {
                        invoiceInDayReport.Invoice.InvoicesInDayReports.Remove(invoiceInDayReport);
                        repo.Remove(invoiceInDayReport);                        
                    }
                    else
                    {
                        var innerInvoice = invoiceInDayReport.Invoice;
                        repo.Remove(invoiceInDayReport);
                        repo.Remove(innerInvoice);
                    }
                }
            }

            if (dayReport.Deliveries.Count > 0) 
            {
                repo.RemoveRange(dayReport.Deliveries);
            }

            repo.Remove(dayReport);
            await repo.SaveChangesAsync();
        }

        public async Task<IEnumerable<DayReportHeaderViewModel>> GetAllDatesAsync(UserViewModel userViewModel)
        {
            return await repo.AllReadonly<DayReport>()
                .Where(d => d.IdentityUserId == userViewModel.Id)
                .Select(d => new DayReportHeaderViewModel()
                {
                    Id = d.Id,
                    Date = d.Date
                }).ToArrayAsync();
        }

        public async Task<IEnumerable<UserViewModel>> GetAllUsersWhitDayReports(UserViewModel user)
        {
            if (IsAtLeastInOneRole(user, Admin, LogisticsManager) == false)
            {
                throw new UnauthorizedAccessException(NotAuthenticate(user));
            }

            return await repo.AllReadonly<DayReport>()
                .Select(d => d.IdentityUser)
                .Distinct()
                .Select(u => new UserViewModel()
                {
                    Id = u.Id,
                    Name = $"{u.Name} {u.LastName}",
                    UserName = u.UserName,
                }).ToArrayAsync();
        }


        public async Task<IEnumerable<UserViewModel>> GetAllDrivers(UserViewModel user)
        {
            if (IsAtLeastInOneRole(user, Admin, LogisticsManager) == false)
            {
                throw new UnauthorizedAccessException(NotAuthenticate(user));
            }

            var users = await userManager.GetUsersInRoleAsync(Driver);

            return users.Select(u => new UserViewModel
            {
                Id = u.Id,
                Name = $"{u.Name} {u.LastName}",
                UserName= u.UserName
            });
        }

        public async Task<IEnumerable<SimpleDayReportViewModel>> GetSimpleFilteredAsync(UserViewModel user, string? reporterUserName, DateTime? startDate, DateTime? endDate)
        {
            IQueryable<DayReport> query = SetDateInterval(GetFilteredDayReportsQuery(user, reporterUserName), startDate ?? DateTime.MinValue, endDate ?? DateTime.Now);

            return await query.OrderByDescending(d => d.Date)
                .Select(d => new SimpleDayReportViewModel()
                {
                    Id = d.Id,
                    ReporterName = $"{d.IdentityUser.Name} {d.IdentityUser.LastName}",
                    ReportedDate = d.Date,
                    TransmissionDate = d.TransmissionDate,
                    TotalAmount = d.TotalAmount.ToString("C"),
                    TotalIncome = d.TotalIncome.ToString("C"),
                    TotalCash = d.TotalCash.ToString("C"),
                    VehicleLicensePlate = d.Vehicle != null ? d.Vehicle.LicensePlate : null

                }).ToArrayAsync();
        }

        public async Task<DayReportBanknotesViewModel> GetBanknotesReadonlyAsync(UserViewModel user, int id)
        {
            return await repo.AllReadonly<DayReport>()
                .Where(d => d.IdentityUserId == user.Id && d.Id == id)
                .Select(d => new DayReportBanknotesViewModel()
                {
                    Id = id,
                    Date = d.Date,
                    Banknotes = d.Banknotes,
                    TotalIncome = d.TotalIncome
                }).FirstOrDefaultAsync() ?? throw new ArgumentNullException(NotFound(nameof(DayReport)));
        }

        public Task<int?> GetVehicleIdFromDayReportAsync(int dayReportId)
        {
            return repo.AllReadonly<DayReport>()
                .Where(d => d.Id == dayReportId)
                .Select(d => d.VehicleId)
                .FirstOrDefaultAsync();
        }

        public async Task<DetailDayReportViewModel> GetDetailDayReportByIdAsync(UserViewModel user, int id)
        {
            IQueryable<DayReport> query = repo.AllReadonly<DayReport>()
                .Where(d => d.Id == id);
            if (IsAtLeastInOneRole(user, Admin, LogisticsManager) == false)
            {
                query = query.Where(d => d.IdentityUserId == user.Id);
            }
            var result = await query.Select(d => new DetailDayReportViewModel()
            {
                Id = d.Id,
                ReportedDate = d.Date,
                EmployeeName = $"{d.IdentityUser.Name} {d.IdentityUser.LastName}",
                TotalAmount = d.TotalAmount,
                TotalIncome = d.TotalIncome,
                TotalCash = d.TotalCash,
                DeliveriesCount = d.Deliveries.Count,
                PaymentsCount = d.Invoices.Count(i => i.PayMethod != PayMethod.Expense),
                Deliveries = d.Deliveries
                    .Select(dl => new DeliveryViewModel()
                    {
                        Id = dl.Id,
                        EmployeeName = $"{dl.Employee.Name} {dl.Employee.LastName}",
                        CompanyObjectName = dl.DeliveryAddress.Name,
                        CompanyObjectId = dl.DeliveryAddressId,
                        Address = dl.DeliveryAddress.Address != null ? $"{dl.DeliveryAddress.Address.Town} {dl.DeliveryAddress.Address.StreetName} {dl.DeliveryAddress.Address.Number}" : null,
                        TotalIncome = dl.Payments.Sum(i => i.Income),
                        Payments = dl.Payments
                            .Select(p => new PaymentViewModel()
                            {
                                Id = p.Id,
                                CompanyName = $"{p.Invoice.Company.Name} {p.Invoice.Company.Type}",
                                CompanyObjectName = p.Invoice.CompanyObject.Name,
                                InvoiceNumber = p.Invoice.Number,
                                Amount = p.Invoice.Amount,
                                Weight = p.Invoice.Weight,
                                Income = p.Income,
                                Balance = p.Invoice.Amount - Math.Abs(p.Invoice.InvoicesInDayReports.Sum(i => i.Income)),
                                PayMethod = p.PayMethod,
                                IsBank = p.Invoice.CompanyObject.IsBankPay,
                                IsCompleted = p.IsCompleted
                            })
                    })

            }).FirstOrDefaultAsync() ?? throw new ArgumentNullException(NotFound(nameof(DayReport)));

            foreach (var delivery in result.Deliveries)
            {
                delivery.CalculateTotals();
            }

            return result;
        }

        public async Task<DayReportViewModel> GetByIdAsync(UserViewModel user, int id)
        {
            IQueryable<DayReport> query = repo.AllReadonly<DayReport>()
                .Where(d => d.Id == id);
            if (IsAtLeastInOneRole(user, Admin, LogisticsManager) == false)
            {
                query = query.Where(d => d.IdentityUserId == user.Id);
            }

            var dayReport = await query
                .Include(d => d.IdentityUser)
                .Include(d => d.Vehicle)
                .Include(d => d.Invoices)
                .ThenInclude(i => i.Invoice)
                .ThenInclude(i => i.CompanyObject)
                .ThenInclude(i => i.Company)
                .FirstOrDefaultAsync() ?? throw new ArgumentNullException(NotFound(nameof(DayReport)));
            return MapToDayReportViewModel(dayReport);
        }

        public async Task UpdateAsync(DayReportViewModel dayReport)
        {
            var updatedDayReport = await repo.GetByIdAsync<DayReport>(dayReport.Id) ?? throw new ArgumentNullException(NotFound(nameof(DayReport)));
            
            updatedDayReport.Update(dayReport);
            await repo.SaveChangesAsync();
        }

        public async Task UpdateBanknotesAsync(UserViewModel user, DayReportBanknotesViewModel dayReportBanknotes)
        {
            if(IsAtLeastInOneRole(user, Driver) == false)
            {
                throw new UnauthorizedAccessException(nameof(DelitaUser));
            }

            var updatedDayReport = await repo.GetByIdAsync<DayReport>(dayReportBanknotes.Id) ?? throw new ArgumentNullException(NotFound(nameof(DayReport)));

            if(updatedDayReport.IdentityUserId != user.Id)
            {
                throw new UnauthorizedAccessException(nameof(DelitaUser));
            }

            updatedDayReport.TotalCash = dayReportBanknotes.Banknotes.Sum(b => b.Key * b.Value);
            updatedDayReport.Banknotes = dayReportBanknotes.Banknotes;
            await repo.SaveChangesAsync();
        }

        private async Task<DelitaUser> GetUserAsync(UserManager<DelitaUser> userManager, UserViewModel user)
        {
            return await userManager.FindByIdAsync(user.Id.ToString()) ??
                throw new InvalidOperationException(NotAuthenticate(user));
        }

        private IQueryable<DayReport> SetDateInterval(IQueryable<DayReport> query, DateTime startDate, DateTime endDate)
        {
            return query.Where(p => p.Date >= startDate.Date && p.Date <= endDate.Date);
        }

        private IQueryable<DayReport> GetFilteredDayReportsQuery(UserViewModel user,  string? reporterUserName)
        {
            IQueryable<DayReport> query;
            if (IsAtLeastInOneRole(user, Admin, LogisticsManager))
            {
                query = repo.AllReadonly<DayReport>();
                if (string.IsNullOrEmpty(reporterUserName) == false)
                {   
                    query = query.Where(d => d.IdentityUser.UserName == reporterUserName);
                }
            }
            else if (user.Roles.Contains(Driver))
            {
                query = repo.AllReadonly<DayReport>()
                    .Where(d => d.IdentityUserId == user.Id);
            }
            else
            {
                throw new UnauthorizedAccessException(NotAuthenticate(user));
            }
            return query;
        }

        private DayReportViewModel MapToDayReportViewModel(DayReport dayReport)
        {
            var newDayReport = new DayReportViewModel()
            {
                Id = dayReport.Id,
                Date = dayReport.Date,
                TotalAmount = dayReport.TotalAmount,
                TotalExpense = dayReport.TotalExpense,
                TotalIncome = dayReport.TotalIncome,
                TotalNotPay = dayReport.TotalNotPay,
                TotalOldInvoice = dayReport.TotalOldInvoice,
                TotalWeight = dayReport.TotalWeight,
                Banknotes = dayReport.Banknotes,
                TotalCash = dayReport.TotalCash,
                TransmissionDate = dayReport.TransmissionDate == null ? DateTime.Now : (DateTime)dayReport.TransmissionDate,
                User = new UserViewModel
                {
                    Id = dayReport.IdentityUserId,
                    Name = $"{dayReport.IdentityUser.Name} {dayReport.IdentityUser.LastName}",
                    UserName = dayReport.IdentityUser.UserName
                },
                Vehicle = dayReport.Vehicle == null ? null :
                    new VehicleViewModel()
                    {
                        Id = dayReport.Vehicle.Id,
                        LicensePlate = dayReport.Vehicle.LicensePlate,
                        Model = dayReport.Vehicle.Model
                    }
            };
            var invoices = dayReport.Invoices.Select(i => new InvoiceViewModel()
            {
                Company = new CompanyViewModel()
                {
                    Id = i.Invoice.CompanyObject.Company.Id,
                    Name = i.Invoice.CompanyObject.Company.Name,
                    Type = i.Invoice.CompanyObject.Company.Type,
                },
                CompanyObject = new CompanyObjectViewModel()
                {
                    Id = i.Invoice.CompanyObject.Id,
                    Name = i.Invoice.CompanyObject.Name,
                    Company = new CompanyViewModel()
                    {
                        Id = i.Invoice.CompanyObject.Company.Id,
                        Name = i.Invoice.CompanyObject.Company.Name,
                        Type = i.Invoice.CompanyObject.Company.Type,
                    }
                },
                Id = i.Invoice.Id,
                IdInDayReport = i.Id,
                DayReport = newDayReport,
                Number = i.Invoice.Number,
                Amount = i.Invoice.Amount,
                Income = i.Income,
                PayMethod = i.PayMethod,
                IsPaid = i.Invoice.IsPaid,
                Weight = i.Invoice.Weight
            }).ToList();

            newDayReport.Invoices = invoices;
            return newDayReport;
        }
    }
}
