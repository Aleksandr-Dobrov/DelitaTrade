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
        public async Task<int> CreateAsync(DayReportViewModel dayReport)
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
            return newDayReport.Id;
        }

        public async Task DeleteAsync(UserViewModel user, int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<DayReportViewModel>> GetAllDatesAsync(UserViewModel userViewModel)
        {
            var user = await GetUserAsync(repo, userViewModel);
            return await repo.AllReadonly<DayReport>()
                .Where(d => d.UserId == user.Id)
                .Select(d => new DayReportViewModel()
                {
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
                .Include(d => d.Invoices)
                .ThenInclude(i => i.Invoice)
                .ThenInclude(i => i.CompanyObject)
                .ThenInclude(o => o.Company)
                .FirstOrDefaultAsync(d => d.UserId == user.Id && d.Id == id) ?? throw new ArgumentNullException(NotFound(nameof(DayReport)));
            List<InvoiceViewModel> invoices = new List<InvoiceViewModel>();
            foreach (var invoice in dayReport.Invoices)
            {
                var newCompanyViewModel = new CompanyViewModel()
                {
                    Id = invoice.Invoice.CompanyObject.Company.Id,
                    Name = invoice.Invoice.CompanyObject.Company.Name,
                    Type = invoice.Invoice.CompanyObject.Company.Type,
                };

                invoices.Add(new InvoiceViewModel()
                {
                    Id = invoice.Invoice.Id,
                    IdInDayReport = invoice.Id,
                    Company = newCompanyViewModel,
                    CompanyObject = new CompanyObjectViewModel()
                        {
                            Id = invoice.Invoice.CompanyObject.Id,
                            Name = invoice.Invoice.CompanyObject.Name,
                            Company = newCompanyViewModel,
                            IsBankPay = invoice.Invoice.CompanyObject.IsBankPay
                        },
                    Number = invoice.Invoice.Number,
                    TotalAmount = invoice.Invoice.Amount,
                    PayMethod = invoice.PayMethod,
                    TotalIncome = invoice.Income,
                    TotalWeight = invoice.Invoice.Weight,
                    IsPaid = invoice.Invoice.IsPaid
                });
            }
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
                    },
                Invoices = invoices                
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
