using DelitaTrade.Common.Enums;
using DelitaTrade.Core.Interfaces;
using DelitaTrade.Core.ViewModels;
using DelitaTrade.Infrastructure.Data.Models;

namespace DelitaTrade.Core.Exporters.ExportedModels
{
    public class DayReportExportedModel : IExportedDayReport
    {
        public DayReportExportedModel(string dayReportID, string vehicle, decimal totalAmount, decimal totalIncome, decimal totalOldInvoice, decimal totalNonPay, decimal totalExpenses, string transmissionDate, IDictionary<decimal, int> banknotes, IEnumerable<IExportedInvoice> invoices, string employeeFullName)
        {
            DayReportID = dayReportID;
            EmployeeFullName = employeeFullName;
            Vehicle = vehicle;
            TotalAmount = totalAmount;
            TotalIncome = totalIncome;
            TotalOldInvoice = totalOldInvoice;
            TotalNonPay = totalNonPay;
            TotalExpenses = totalExpenses;
            TransmissionDate = transmissionDate;
            Banknotes = GetBanknotes(banknotes);
            Invoices = invoices;
        }
        public string DayReportID { get; private set; }

        public string EmployeeFullName {  get; private set; }

        public string Vehicle { get; private set; }

        public decimal TotalAmount { get; private set; }

        public decimal TotalIncome { get; private set; }

        public decimal TotalOldInvoice { get; private set; }

        public decimal TotalNonPay { get; private set; }

        public decimal TotalExpenses { get; private set; }

        public string TransmissionDate { get; private set; }

        public IDictionary<decimal, int> Banknotes { get; private set; }

        public IEnumerable<IExportedInvoice> Invoices { get; private set; }

        public decimal BankPayTotal()
        {
            decimal totalBankAmount = 0;
            foreach (var invoice in Invoices)
            {
                if (invoice.PayMethod == PayMethod.Bank)
                {
                    totalBankAmount += invoice.Amount;
                }
            }
            return totalBankAmount;
        }

        public int GetRepeatNumbering(string InvoiceNumber)
        {
            if (Invoices.FirstOrDefault(i => i.Number == InvoiceNumber) == null)
            {
                return 0;
            }
            return Invoices.Where(i => i.Number == InvoiceNumber).Count();
        }

        private IDictionary<decimal, int> GetBanknotes(IDictionary<decimal, int> banknotes)
        {
            IDictionary<decimal, int> newBanknotes = new Dictionary<decimal, int>();
            foreach (var banknote in banknotes)
            {
                newBanknotes[banknote.Key] = banknote.Value;
            }
            return newBanknotes;
        }

    }
}
