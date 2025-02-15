using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelitaTrade.ViewModels.Interfaces
{
    public interface IExportedDayReport
    {
        string DayReportID { get; }
        string Vehicle { get; }
        decimal TotalAmount { get; }
        decimal TotalIncome { get; }
        decimal TotalOldInvoice { get; }
        decimal TotalNonPay { get; }
        decimal TotalExpenses { get; }
        string TransmissionDate { get; }

        IDictionary<decimal, int> Banknotes { get; }
        ICollection<IExportedInvoice> Invoices { get; }

        decimal BankPayTotal();
        int GetRepeatNumbering(string InvoiceNumber);
    }
}
