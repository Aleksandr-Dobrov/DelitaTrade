using DelitaTrade.Core.ViewModels;
using DelitaTrade.Infrastructure.Data.Models;

namespace DelitaTrade.Core.Extensions
{
    public static class InvoiceExtensions
    {
        public static void Update(this InvoiceInDayReport invoiceToUpdate, InvoiceViewModel invoice)
        {
            if (invoiceToUpdate.Invoice.Amount != invoice.Amount) invoiceToUpdate.Invoice.Amount = invoice.Amount;
            if (invoiceToUpdate.Invoice.Weight != invoice.Weight) invoiceToUpdate.Invoice.Weight = invoice.Weight;
            if (invoiceToUpdate.Invoice.CompanyId != invoice.Company.Id) invoiceToUpdate.Invoice.CompanyId = invoice.Company.Id;
            if (invoiceToUpdate.Invoice.CompanyObjectId != invoice.CompanyObject.Id) invoiceToUpdate.Invoice.CompanyObjectId = invoice.CompanyObject.Id;
            if (invoiceToUpdate.Income != invoice.Income) invoiceToUpdate.Income = invoice.Income;
            if (invoiceToUpdate.PayMethod != invoice.PayMethod) invoiceToUpdate.PayMethod = invoice.PayMethod;
        }
    }
}
