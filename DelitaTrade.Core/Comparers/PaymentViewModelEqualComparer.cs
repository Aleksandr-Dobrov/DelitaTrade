using DelitaTrade.Core.ViewModels.DeliveryModels;
using System.Diagnostics.CodeAnalysis;

namespace DelitaTrade.Core.Comparers
{
    public class PaymentViewModelEqualComparer : IEqualityComparer<PaymentViewModel>
    {
        public bool Equals(PaymentViewModel? x, PaymentViewModel? y)
        {
            return x != null && y != null && x.InvoiceNumber == y.InvoiceNumber;
        }

        public int GetHashCode([DisallowNull] PaymentViewModel obj)
        {
            return obj.InvoiceNumber.GetHashCode();
        }
    }
}
