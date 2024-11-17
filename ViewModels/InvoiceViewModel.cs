using DelitaTrade.Models;

namespace DelitaTrade.ViewModels
{
    public class InvoiceViewModel
    {
        private readonly Invoice _invoice;

        public InvoiceViewModel(Invoice invoice)
        {
            _invoice = invoice;
        }
        public DayReport DayReport => _invoice.DayReport;
        public int Id => _invoice.Id;
        public string CompanyName => _invoice.CompanyFullName;
        public string ObjectName => _invoice.ObjectName;
        public string InvoiceID => _invoice.InvoiceID;
        public decimal Amount => _invoice.Amount;
        public decimal Income => _invoice.Income;
        public double Weight => _invoice.Weight;
        public string PayMethod => _invoice.PayMethod;
        public string StringAmount => $"{_invoice.Amount:C}";
        public string StringIncome => $"{_invoice.Income:C}";
    }
}
