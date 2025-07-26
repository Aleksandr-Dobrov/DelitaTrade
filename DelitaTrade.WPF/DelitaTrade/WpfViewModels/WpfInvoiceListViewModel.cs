using DelitaTrade.Core.ViewModels;
using static DelitaTrade.Common.GlobalVariables;
using DelitaTrade.Common.Enums.EnumTranslators;
using DelitaTrade.ViewModels;

namespace DelitaTrade.WpfViewModels
{
    public class WpfInvoiceListViewModel : ViewModelBase
    {
        private readonly Core.ViewModels.InvoiceViewModel _invoice;

        public WpfInvoiceListViewModel(Core.ViewModels.InvoiceViewModel invoiceViewModel)
        {
            _invoice = invoiceViewModel;
        }

        public Core.ViewModels.InvoiceViewModel InvoiceViewModel => _invoice;

        public int Id => _invoice.IdInDayReport;
        public string CompanyName => $"{_invoice.Company.Name} {_invoice.Company.Type}";
        public string ObjectName => $"{_invoice.CompanyObject.Name}";
        public string InvoiceID => _invoice.Number;
        public decimal Weight => _invoice.Weight;
        public string PayMethod => _invoice.PayMethod.GetStringValue(Language);
        public string StringAmount => $"{_invoice.Amount:C}";
        public string StringIncome => $"{_invoice.Income:C}";

        public override int GetHashCode()
        {
            return Id;
        }

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (obj is WpfInvoiceListViewModel invoice)
            {
                return invoice.Id == Id;
            }
            return false;   
        }

        public void OnViewModelChange()
        {
            OnPropertyChange(nameof(CompanyName));
            OnPropertyChange(nameof(ObjectName));
            OnPropertyChange(nameof(Weight));
            OnPropertyChange(nameof(PayMethod));
            OnPropertyChange(nameof(StringAmount));
            OnPropertyChange(nameof(StringIncome));
        }
    }
}
