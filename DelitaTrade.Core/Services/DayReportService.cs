﻿using DelitaTrade.Common;
using DelitaTrade.Core.Contracts;
using DelitaTrade.Core.Extensions;
using DelitaTrade.Core.ViewModels;
using DelitaTrade.Infrastructure.Common;
using DelitaTrade.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using static DelitaTrade.Common.ExceptionMessages;

namespace DelitaTrade.Core.Services
{
    public class DayReportService(IRepository repo) : IDayReportService
    {
        public async Task<DayReportViewModel> CreateAsync(DayReportViewModel dayReport)
        {
            var user = await GetUserAsync(repo, dayReport.User);
            var newDayReport = new DayReport()
            {
                User = user,
                Date = dayReport.Date,
                Banknotes = dayReport.Banknotes,
                TotalCash = dayReport.TotalCash,
                TotalAmount = dayReport.TotalAmount,
                TotalIncome = dayReport.TotalIncome,
                TotalNotPay = dayReport.TotalNotPay,
                TotalOldInvoice = dayReport.TotalOldInvoice,
                TotalExpense = dayReport.TotalExpense,
                TotalWeight = dayReport.TotalWeight                
            };
            await repo.AddAsync(newDayReport);
            await repo.SaveChangesAsync();
            await repo.ReloadAsync(newDayReport);
            dayReport.Id = newDayReport.Id;
            return dayReport;
        }

        public async Task DeleteAsync(UserViewModel userViewModel, int id)
        {
            var user = await GetUserAsync(repo, userViewModel);
            var dayReport = await repo.All<DayReport>()
                .Include(d => d.Invoices)
                .ThenInclude(i => i.Invoice)
                .ThenInclude(i => i.InvoicesInDayReports)
                .FirstOrDefaultAsync(d => d.Id == id) ?? throw new ArgumentNullException(NotFound(nameof(DayReport)));

            if (dayReport.User.Id != dayReport.UserId) throw new InvalidOperationException(NotAuthenticate(userViewModel));

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

        public async Task<IEnumerable<DayReportViewModel>> GetAllDatesAsync(UserViewModel userViewModel)
        {
            var user = await GetUserAsync(repo, userViewModel);
            return await repo.AllReadonly<DayReport>()
                .Where(d => d.UserId == user.Id)
                .Select(d => new DayReportViewModel()
                {
                    Id = d.Id,
                    Date = d.Date,
                    Banknotes = d.Banknotes,
                    User = userViewModel
                }).ToArrayAsync();
        }

        public async Task<IEnumerable<DayReportViewModel>> GetAllFilteredAsync(UserViewModel userViewModel, string filter, int limit)
        {
            var user = await GetUserAsync(repo, userViewModel);
            return await repo.AllReadonly<DayReport>()
                .Where(d => d.UserId == user.Id)
                .Take(limit)
                .Select(d => new DayReportViewModel()
                {
                    Date = d.Date,
                    Banknotes = d.Banknotes,
                    User = userViewModel
                }).ToArrayAsync();
        }

        public async Task<DayReportViewModel> GetByIdAsync(UserViewModel userViewModel, int id)
        {
            var user = await GetUserAsync(repo, userViewModel);
            var dayReport = await repo.All<DayReport>()
                .Include(d => d.Vehicle)
                .FirstOrDefaultAsync(d => d.UserId == user.Id && d.Id == id) ?? throw new ArgumentNullException(NotFound(nameof(DayReport)));
            return new DayReportViewModel() 
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
                User = userViewModel,
                Vehicle = dayReport.Vehicle == null ? null : 
                    new VehicleViewModel() 
                    {
                        Id = dayReport.Vehicle.Id,
                        LicensePlate = dayReport.Vehicle.LicensePlate,
                        Model = dayReport.Vehicle.Model
                    }            
            };  
        }

        public async Task UpdateAsync(DayReportViewModel dayReport)
        {
            var updatedDayReport = await repo.GetByIdAsync<DayReport>(dayReport.Id) ?? throw new ArgumentNullException(NotFound(nameof(DayReport)));
            if(dayReport.Vehicle != null && await repo.GetByIdAsync<Vehicle>(dayReport.Vehicle.Id) == null) throw new ArgumentNullException(NotFound(nameof(Vehicle)));
            
            updatedDayReport.Update(dayReport);
            await repo.SaveChangesAsync();
        }

        private async Task<User> GetUserAsync(IRepository repo, UserViewModel user)
        {
            return await repo.GetByIdAsync<User>(user.Id) ??
                throw new InvalidOperationException(NotAuthenticate(user));
        }
    }
}
