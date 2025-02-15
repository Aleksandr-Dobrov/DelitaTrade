using DelitaTrade.Core.Exporters.ExportedModels;
using DelitaTrade.Core.Interfaces;
using DelitaTrade.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DelitaTrade.Core.Factories
{
    public static class DayReportFactory
    {
        public static IExportedDayReport CreateExportedDayReport(this DayReportViewModel dayReport)
        {
            return new DayReportExportedModel(dayReport.Date.Date.ToString("yyyy-MM-dd"),
                                              dayReport.Vehicle.LicensePlate,
                                              dayReport.TotalAmount,
                                              dayReport.TotalIncome,
                                              dayReport.TotalOldInvoice,
                                              dayReport.TotalNotPay,
                                              dayReport.TotalExpense,
                                              dayReport.TransmissionDate.Date.ToString("yyyy-MM-dd"),
                                              dayReport.Banknotes,
                                              dayReport.Invoices.GetInvoices(),
                                              dayReport.User.Name);
        }


        private static IEnumerable<IExportedInvoice> GetInvoices(this IEnumerable<InvoiceViewModel> invoiceViewModels)
        {
            List<InvoiceExportedModel> invoiceToExport = [];
            foreach (var invoice in invoiceViewModels)
            {
                invoiceToExport.Add(new InvoiceExportedModel(invoice.IsPaid,
                                                             invoice.Number,
                                                             $"{invoice.Company.Name} {invoice.Company.Type}",
                                                             invoice.CompanyObject.Name,
                                                             invoice.Amount,
                                                             invoice.Income,
                                                             invoice.PayMethod));
            }
            return invoiceToExport;
        }
    }
}
