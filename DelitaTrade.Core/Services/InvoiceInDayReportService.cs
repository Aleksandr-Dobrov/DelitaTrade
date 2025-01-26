using DelitaTrade.Core.Contracts;
using DelitaTrade.Core.ViewModels;
using DelitaTrade.Infrastructure.Common;
using DelitaTrade.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;
using static DelitaTrade.Common.ExceptionMessages;
using DelitaTrade.Common.Enums;
using DelitaTrade.Core.Extensions;
using System.Linq.Expressions;

namespace DelitaTrade.Core.Services
{
    public class InvoiceInDayReportService(IRepository repo) : IInvoiceIdDayReportService
    {
        public async Task<IEnumerable<InvoiceViewModel>> AllInDayReportAsync(int dayReportId)
        {
            var dayReport = await repo.GetByIdAsync<DayReport>(dayReportId) ?? throw new ArgumentNullException(NotFound(nameof(DayReport)));
            return await repo.AllReadonly<InvoiceInDayReport>()
                .Include(i => i.Invoice)
                .ThenInclude(i => i.CompanyObject)
                .ThenInclude(i => i.Company)
                .Where(i => i.DayReportId == dayReportId)
                .Select(i => MapToDeepViewModel(i))
                .ToArrayAsync();
        }

        public async Task<IEnumerable<InvoiceViewModel>> AllReadonlyAsync()
        {
            return await repo.AllReadonly<Invoice>()
                .Include(i => i.CompanyObject)
                .ThenInclude(i => i.Company)
                .Select(i => MapToViewModel(i))
                .ToArrayAsync();
        }

        /// <summary>
        /// Create new Invoice if it not exists in storage and create new InvoiceInDayReport in day report.
        /// </summary>
        /// <param name="newInvoice">DayReport is required to create invoice</param>
        /// <returns>Return Id of created invoice in DayReport</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<int> CreateAsync(InvoiceViewModel newInvoice)
        {
            if (newInvoice.IdInDayReport != 0) throw new InvalidOperationException(InvalidEntry(newInvoice));
            if (newInvoice.DayReport == null) throw new ArgumentNullException(NotFound(nameof(DayReport)));
            var company = await repo.GetByIdAsync<Company>(newInvoice.Company.Id) ?? throw new ArgumentNullException(NotFound(nameof(Company)));
            var companyObject = await repo.GetByIdAsync<CompanyObject>(newInvoice.CompanyObject.Id) ?? throw new ArgumentNullException(NotFound(nameof(CompanyObject)));
            var dayReport = await repo.GetByIdAsync<DayReport>(newInvoice.DayReport.Id) ?? throw new ArgumentNullException(NotFound(nameof(DayReport)));
            var invoice = await repo.All<Invoice>().
                Where(i => i.Number == newInvoice.Number)
                .FirstOrDefaultAsync();
            if (invoice == null)
            {
                invoice = new Invoice()
                {
                    Number = newInvoice.Number,
                    Company = company,
                    CompanyObject = companyObject,
                    Amount = newInvoice.Amount,
                    Weight = newInvoice.Weight,
                    IsPaid = IsPaid(newInvoice)
                };
                await repo.AddAsync(invoice);
                await repo.SaveChangesAsync();
                await repo.ReloadAsync(invoice);
            }
            else
            {
                await IsPaid(newInvoice, invoice);
            }

            var invoiceInDayReport = new InvoiceInDayReport()
            {
                DayReport = dayReport,
                Invoice = invoice,
                Income = newInvoice.Income,
                PayMethod = newInvoice.PayMethod,
            };
            await repo.AddAsync(invoiceInDayReport);
            await repo.SaveChangesAsync();
            await repo.ReloadAsync(invoiceInDayReport);
            return invoiceInDayReport.Id;
        }

        public async Task DeleteAsync(InvoiceViewModel invoice)
        {
            var invoiceToDelete = await repo.GetByIdAsync<InvoiceInDayReport>(invoice.IdInDayReport) 
                ?? throw new ArgumentNullException(NotFound(nameof(InvoiceInDayReport)));
            repo.Remove(invoiceToDelete);
            await repo.SaveChangesAsync();
        }

        public async Task<IEnumerable<InvoiceViewModel>> SearchReadonlyAsync(string arg, int limit)
        {
            return await GetFilteredReadonlyObjects(i => i.Invoice.Number.Contains(arg) ||
                                                        i.Invoice.CompanyObject.Name.Contains(arg) ||
                                                        i.Invoice.CompanyObject.Company.Name.Contains(arg))
                .Include(i => i.Invoice)
                .ThenInclude(i => i.CompanyObject)
                .ThenInclude(i => i.Company)
                .Include(i => i.DayReport)                
                .Take(limit)
                .Select(i => MapToDeepViewModel(i))
                .ToArrayAsync();
        }

        public async Task UpdateAsync(InvoiceViewModel invoice)
        {
            var invoiceToUpdate = await repo.AllReadonly<InvoiceInDayReport>()
                .Include(i => i.Invoice)
                .ThenInclude(i => i.CompanyObject)
                .ThenInclude(i => i.Company)
                .Where(i => i.Id == invoice.IdInDayReport)
                .FirstOrDefaultAsync()
                ?? throw new ArgumentNullException(NotFound(nameof(InvoiceInDayReport)));
            invoiceToUpdate.Update(invoice);
            await repo.SaveChangesAsync();
        }

        private InvoiceViewModel MapToViewModel(Invoice i)
        {
            var newCompany = new CompanyViewModel()
            {
                Id = i.CompanyObject.Company.Id,
                Name = i.CompanyObject.Company.Name,
                Type = i.CompanyObject.Company.Type,
            };

            return new InvoiceViewModel()
            {
                Id = i.Id,
                Number = i.Number,
                Company = newCompany,
                CompanyObject = new CompanyObjectViewModel()
                {
                    Id = i.CompanyObject.Id,
                    Name = i.CompanyObject.Name,
                    Company = newCompany,
                    IsBankPay = i.CompanyObject.IsBankPay
                },
                IsPaid = i.IsPaid
            };
        }

        private InvoiceViewModel MapToDeepViewModel(InvoiceInDayReport i)
        {
            var newCompany = new CompanyViewModel()
            {
                Id = i.Invoice.CompanyObject.Company.Id,
                Name = i.Invoice.CompanyObject.Company.Name,
                Type = i.Invoice.CompanyObject.Company.Type,
            };

            return new InvoiceViewModel()
            {
                Id = i.Invoice.Id,
                IdInDayReport = i.Id,
                Company = newCompany,
                CompanyObject = new CompanyObjectViewModel()
                {
                    Id = i.Invoice.CompanyObject.Id,
                    Name = i.Invoice.CompanyObject.Name,
                    Company = newCompany,
                    IsBankPay = i.Invoice.CompanyObject.IsBankPay
                },
                Number = i.Invoice.Number,
                Amount = i.Invoice.Amount,
                PayMethod = i.PayMethod,
                Income = i.Income,
                Weight = i.Invoice.Weight,
                IsPaid = i.Invoice.IsPaid
            };
        }

        private bool IsPaid(InvoiceViewModel invoice)
        {
            if (invoice.PayMethod == PayMethod.Card || invoice.PayMethod == PayMethod.Cash) return invoice.Amount <= invoice.Income;
            else if (invoice.PayMethod == PayMethod.Bank) return true;
            else if (invoice.PayMethod == PayMethod.ForCreditNote ||
                invoice.PayMethod == PayMethod.Cancellation ||
                invoice.PayMethod == PayMethod.CreditNote ||
                invoice.PayMethod == PayMethod.Expense) return true;
            else return false;
        }

        private async Task IsPaid(InvoiceViewModel newInvoice, Invoice invoice)
        {
            decimal totalIncome = newInvoice.Income;
            var invoices = await repo.AllReadonly<InvoiceInDayReport>()
                .Include(i => i.Invoice)
                .Where(i => i.Invoice.Number == newInvoice.Number)
                .ToArrayAsync();
            foreach (var item in invoices)
            {
                item.Income += totalIncome;
            }
            if (totalIncome < newInvoice.Amount) invoice.IsPaid = false;
            else invoice.IsPaid = true;
        }
        private IQueryable<InvoiceInDayReport> GetFilteredReadonlyObjects(Expression<Func<InvoiceInDayReport, bool>> filter)
        {
            return repo.AllReadonly<InvoiceInDayReport>().Where(filter);
        }
    }
}
