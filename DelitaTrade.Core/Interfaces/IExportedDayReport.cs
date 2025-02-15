using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelitaTrade.Core.Interfaces
{
    public interface IExportedDayReport
    {
        string DayReportID { get; }
        string EmployeeFullName { get; }
        string Vehicle { get; }
        decimal TotalAmount { get; }
        decimal TotalIncome { get; }
        decimal TotalOldInvoice { get; }
        decimal TotalNonPay { get; }
        decimal TotalExpenses { get; }
        string TransmissionDate { get; }

        IDictionary<decimal, int> Banknotes { get; }
        IEnumerable<IExportedInvoice> Invoices { get; }

        decimal BankPayTotal();
        int GetRepeatNumbering(string InvoiceNumber);
    }
}
