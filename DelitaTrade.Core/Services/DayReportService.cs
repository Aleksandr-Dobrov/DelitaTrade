﻿using DelitaTrade.Common;
using DelitaTrade.Core.Contracts;
using DelitaTrade.Core.Extensions;
using DelitaTrade.Core.ViewModels;
using DelitaTrade.Core.ViewModels.DayReportModels;
using DelitaTrade.Infrastructure.Common;
using DelitaTrade.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using static DelitaTrade.Common.Constants.DelitaIdentityConstants.RoleNames;
using static DelitaTrade.Common.ExceptionMessages;

namespace DelitaTrade.Core.Services
{
    public class DayReportService(IRepository repo, UserManager<DelitaUser> userManager) : IDayReportService
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
                .Include(d => d.Invoices)
                .ThenInclude(i => i.Invoice)
                .ThenInclude(i => i.InvoicesInDayReports)
                .FirstOrDefaultAsync(d => d.Id == id) ?? throw new ArgumentNullException(NotFound(nameof(DayReport)));

            if (dayReport.IdentityUserId != userViewModel.Id) throw new InvalidOperationException(NotAuthenticate(userViewModel));

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

        public async Task<IEnumerable<UserViewModel>> GetAllUsersWhitReports(UserViewModel user)
        {
            if (user.Roles.Contains(Admin) == false)
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

        public async Task<IEnumerable<SimpleDayReportViewModel>> GetSimpleFilteredAsync(UserViewModel user, string? reporterId, DateTime? startDate, DateTime? endDate)
        {
            IQueryable<DayReport> query = SetDateInterval(GetFilteredDayReportsQuery(user, reporterId), startDate ?? DateTime.MinValue, endDate ?? DateTime.Now);

            return await query.Select(d => new SimpleDayReportViewModel()
            {
                Id = d.Id,
                ReporterName = $"{d.IdentityUser.Name} {d.IdentityUser.LastName}",
                ReportedDate = d.Date,
                TransmissionDate = d.Date,
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
                }).FirstOrDefaultAsync() ?? throw new ArgumentNullException(NotFound(nameof(DayReport)));
        }

        public async Task<DayReportViewModel> GetByIdAsync(UserViewModel userViewModel, int id)
        {
            IQueryable<DayReport> query = repo.AllReadonly<DayReport>()
                .Where(d => d.Id == id);
            if (userViewModel.Roles.Contains(Admin) == false)
            {
                query = query.Where(d => d.IdentityUserId == userViewModel.Id);
            }

            var dayReport = await query
                .Include(d => d.Vehicle)
                .Include(d => d.Invoices)
                .ThenInclude(i => i.Invoice)
                .ThenInclude(i => i.CompanyObject)
                .ThenInclude(i => i.Company)
                .FirstOrDefaultAsync() ?? throw new ArgumentNullException(NotFound(nameof(DayReport)));
            return MapToDayReportViewModel(dayReport, userViewModel);
        }

        public async Task UpdateAsync(DayReportViewModel dayReport)
        {
            var updatedDayReport = await repo.GetByIdAsync<DayReport>(dayReport.Id) ?? throw new ArgumentNullException(NotFound(nameof(DayReport)));
            
            updatedDayReport.Update(dayReport);
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

        private IQueryable<DayReport> GetFilteredDayReportsQuery(UserViewModel user,  string? ReporterId)
        {
            IQueryable<DayReport> query;
            if (user.Roles.Contains(Admin))
            {
                query = repo.AllReadonly<DayReport>();
                if (string.IsNullOrEmpty(ReporterId) == false)
                {
                    var result = Guid.TryParse(ReporterId, out Guid resultId);
                    if (result) 
                    {
                        query = query.Where(d => d.IdentityUserId == resultId);
                    }
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

        private DayReportViewModel MapToDayReportViewModel(DayReport dayReport, UserViewModel userViewModel)
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
                User = userViewModel,
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
