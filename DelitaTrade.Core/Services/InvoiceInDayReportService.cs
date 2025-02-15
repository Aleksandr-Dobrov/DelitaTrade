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
    public class InvoiceInDayReportService(IRepository repo) : IInvoiceInDayReportService
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

        public async Task<InvoiceViewModel> LoadNotPaidInvoice(string number)
        {
            Invoice baseInvoice = await repo.AllReadonly<Invoice>()
                .Include(i => i.CompanyObject)
                .ThenInclude(i => i.Company)
                .Include(i => i.InvoicesInDayReports)
                .FirstOrDefaultAsync(i => i.Number == number) ?? throw new ArgumentNullException(NotFound(nameof(Invoice)));
            decimal totalIncome = 0;
            foreach (var invoice in baseInvoice.InvoicesInDayReports)
            {
                totalIncome += invoice.Income;
            }
            var nonPayInvoice = MapToLoadedInvoice(baseInvoice);
            nonPayInvoice.Income = baseInvoice.Amount - totalIncome;
            return nonPayInvoice;
        }

        private static InvoiceViewModel MapToLoadedInvoice(Invoice i)
        {
            var newCompany = new CompanyViewModel()
            {
                Id = i.CompanyObject.Company.Id,
                Name = i.CompanyObject.Company.Name,
                Type = i.CompanyObject.Company.Type,
            };
            return new InvoiceViewModel()
            {
                Company = newCompany,
                CompanyObject = new CompanyObjectViewModel()
                {
                    Id = i.CompanyObject.Id,
                    Name = i.CompanyObject.Name,
                    Company = newCompany,
                    IsBankPay = i.CompanyObject.IsBankPay
                },
                Number = i.Number,
                Amount = i.Amount,
                Weight = i.Weight,
            };
        }

        /// <summary>
        /// Create new Invoice if it not exists in storage and create new InvoiceInDayReport in day report.
        /// </summary>
        /// <param name="newInvoice">DayReport is required to create invoice</param>
        /// <returns>Return view model with Id of created invoice in DayReport</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<InvoiceViewModel> CreateAsync(InvoiceViewModel newInvoice)
        {
            if (newInvoice.IdInDayReport != 0) throw new InvalidOperationException(InvalidEntry(newInvoice));
            if (newInvoice.DayReport == null) throw new ArgumentNullException(NotFound(nameof(DayReport)));
            var company = await repo.GetByIdAsync<Company>(newInvoice.Company.Id) ?? throw new ArgumentNullException(NotFound(nameof(Company)));
            var companyObject = await repo.GetByIdAsync<CompanyObject>(newInvoice.CompanyObject.Id) ?? throw new ArgumentNullException(NotFound(nameof(CompanyObject)));
            var dayReport = await repo.All<DayReport>()
                .Include(d => d.Invoices)
                .ThenInclude(i => i.Invoice)
                .FirstOrDefaultAsync(d => d.Id == newInvoice.DayReport.Id) ?? throw new ArgumentNullException(NotFound(nameof(DayReport)));

            var invoice = await repo.All<Invoice>()
                .Where(i => i.Number == newInvoice.Number)
                .FirstOrDefaultAsync();
            if (invoice == null)
            {
                if (newInvoice.Income > newInvoice.Amount) throw new InvalidOperationException(IncomeNotGreatestAmount());

                invoice = new Invoice()
                {
                    Number = newInvoice.Number,
                    Company = company,
                    CompanyObject = companyObject,
                    Amount = newInvoice.Amount,
                    Weight = newInvoice.Weight,
                    IsPaid = SetIsPaid(newInvoice)
                };
                await repo.AddAsync(invoice);
                await repo.SaveChangesAsync();
                await repo.ReloadAsync(invoice);
            }
            else
            {
                await SetIsPaid(newInvoice, invoice);
            }

            var invoiceInDayReport = new InvoiceInDayReport()
            {
                DayReport = dayReport,
                Invoice = invoice,
                Income = newInvoice.Income,
                PayMethod = newInvoice.PayMethod,
            };
            dayReport.AddInvoiceToTotals(invoiceInDayReport);
            await repo.AddAsync(invoiceInDayReport);

            await repo.SaveChangesAsync();
            await repo.ReloadAsync(invoiceInDayReport);

            newInvoice.Id = invoiceInDayReport.Invoice.Id;
            newInvoice.IdInDayReport = invoiceInDayReport.Id;
            newInvoice.IsPaid = invoiceInDayReport.Invoice.IsPaid;

            return newInvoice;
        }

        public async Task DeleteAsync(InvoiceViewModel invoice)
        {
            if (invoice.DayReport == null) throw new ArgumentNullException(NotFound(nameof(DayReport)));
            
            InvoiceInDayReport? invoiceToDelete = await repo.GetByIdAsync<InvoiceInDayReport>(invoice.IdInDayReport)                
                ?? throw new ArgumentNullException(NotFound(nameof(InvoiceInDayReport)));
            await repo.Include(invoiceToDelete, i => i.Invoice);
            
            var dayReport = await repo.All<DayReport>()
                .Include(d => d.Invoices)
                .ThenInclude(i => i.Invoice)
                .FirstOrDefaultAsync(d => d.Id == invoice.DayReport.Id) ?? throw new ArgumentNullException(NotFound(nameof(DayReport)));
           
            Invoice? invoiceInDb = await repo.All<Invoice>()
                .Include(i => i.InvoicesInDayReports)
                .FirstOrDefaultAsync(i => i.Number == invoiceToDelete.Invoice.Number) ?? throw new InvalidOperationException(NotFound(nameof(Invoice)));

            repo.Remove(invoiceToDelete);
            invoiceInDb.InvoicesInDayReports.Remove(invoiceToDelete);

            if (invoiceInDb?.InvoicesInDayReports.Count == 0)
            {
                repo.Remove(invoiceInDb);
            }
            else
            {
                SetIsPaid(invoiceInDb!);
            }
            
            dayReport.RemoveInvoiceFromTotals(invoiceToDelete);

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
            if (invoice.DayReport == null) throw new ArgumentNullException(NotFound(nameof(DayReport)));

            var invoiceToUpdate = await repo.All<InvoiceInDayReport>()
                .Include(i => i.Invoice)
                .ThenInclude(i => i.CompanyObject)
                .ThenInclude(i => i.Company)
                .Where(i => i.Id == invoice.IdInDayReport)
                .FirstOrDefaultAsync()
                ?? throw new ArgumentNullException(NotFound(nameof(InvoiceInDayReport)));

            var dayReport = await repo.All<DayReport>()
                .Include(d => d.Invoices)
                .ThenInclude(i => i.Invoice)
                .FirstOrDefaultAsync(d => d.Id == invoice.DayReport.Id) ?? throw new ArgumentNullException(NotFound(nameof(DayReport)));

            dayReport.RemoveInvoiceFromTotals(invoiceToUpdate);
            dayReport.Invoices.Remove(invoiceToUpdate);

            invoiceToUpdate.Update(invoice);

            dayReport.AddInvoiceToTotals(invoiceToUpdate);
            dayReport.Invoices.Add(invoiceToUpdate);

            await repo.SaveChangesAsync();
            
            SetIsPaid(await repo.All<Invoice>().Include(i => i.InvoicesInDayReports).FirstAsync(i => i.Number == invoice.Number));            

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
                Amount = i.Amount,
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

        private bool SetIsPaid(InvoiceViewModel invoice)
        {
            if (invoice.PayMethod == PayMethod.Card || invoice.PayMethod == PayMethod.Cash) return invoice.Amount <= invoice.Income;
            else if (invoice.PayMethod == PayMethod.Bank) return true;
            else if (invoice.PayMethod == PayMethod.ForCreditNote ||
                invoice.PayMethod == PayMethod.Cancellation ||
                invoice.PayMethod == PayMethod.CreditNote ||
                invoice.PayMethod == PayMethod.Expense) return true;
            else return false;
        }

        private async Task SetIsPaid(InvoiceViewModel newInvoice, Invoice invoice)
        {
            decimal totalIncome = newInvoice.Income;
            var invoices = await repo.All<InvoiceInDayReport>()
                .Include(i => i.Invoice)
                .Where(i => i.Invoice.Number == newInvoice.Number)
                .ToArrayAsync();
            foreach (var item in invoices)
            {
                totalIncome += item.Income;
            }
            if (totalIncome < newInvoice.Amount) invoice.IsPaid = false;
            else if (totalIncome > newInvoice.Amount) throw new InvalidOperationException(IncomeNotGreatestAmount());
            else invoice.IsPaid = true;
        }

        private void SetIsPaid(Invoice invoice)
        {
            decimal totalIncome = 0;
            
            foreach (var item in invoice.InvoicesInDayReports)
            {
                totalIncome += item.Income;
            }
            if (totalIncome < invoice.Amount) invoice.IsPaid = false;
            else if (totalIncome > invoice.Amount) throw new InvalidOperationException(IncomeNotGreatestAmount());
            else invoice.IsPaid = true;
        }
        private IQueryable<InvoiceInDayReport> GetFilteredReadonlyObjects(Expression<Func<InvoiceInDayReport, bool>> filter)
        {
            return repo.AllReadonly<InvoiceInDayReport>().Where(filter);
        }
    }
}
