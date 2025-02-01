using DelitaTrade.Common.Enums;
using DelitaTrade.Core.Contracts;
using DelitaTrade.Infrastructure.Common;
using DelitaTrade.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace DelitaTrade.Core.Services
{
    public class InvoicePaymentService(IRepository repo) : IInvoicePaymentService
    {
        public async Task<bool> IsExists(string invoiceNumber)
        {
            return await repo.AllReadonly<Invoice>().FirstOrDefaultAsync(i => i.Number == invoiceNumber) != null;
        }

        public async Task<bool> IsPay(string invoiceNumber)
        {
            var invoice = await repo.AllReadonly<Invoice>().FirstOrDefaultAsync(i => i.Number == invoiceNumber);
            if (invoice == null) return false;
            if (invoice.IsPaid) return true;

            var invoices = await repo.AllReadonly<InvoiceInDayReport>().Where(i => i.InvoiceId == invoice.Id).ToListAsync();
            decimal totalIncome = 0;
            foreach (var item in invoices)
            {
                if (IsNotPayable(item.PayMethod)) return true;
                totalIncome += item.Income;
            }
            if (totalIncome < invoice.Amount) return false;
            return true;
        }
        
        private bool IsNotPayable(PayMethod payMethod)
        {
            return payMethod == PayMethod.Bank || 
                   payMethod == PayMethod.CreditNote || 
                   payMethod == PayMethod.Expense ||
                   payMethod == PayMethod.ForCreditNote ||
                   payMethod == PayMethod.Cancellation;
        }
    }
}
