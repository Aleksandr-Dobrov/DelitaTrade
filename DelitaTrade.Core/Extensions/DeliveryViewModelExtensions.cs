using DelitaTrade.Common.Enums;
using DelitaTrade.Core.Comparers;
using DelitaTrade.Core.ViewModels.DeliveryModels;

namespace DelitaTrade.Core.Extensions
{
    public static class DeliveryViewModelExtensions
    {
        public static void CalculateTotals(this DeliveryViewModel delivery)
        {
            decimal creditNote = delivery.Payments.Where(p => p.PayMethod == PayMethod.CreditNote).Sum(p => p.Income);
            delivery.TotalAmount = delivery.Payments.Where(i => i.PayMethod == PayMethod.Bank
                                                                 || i.PayMethod == PayMethod.Cash
                                                                 || i.PayMethod == PayMethod.Card
                                                                 || i.PayMethod == PayMethod.ForCreditNote)
                                                                 .Distinct(new PaymentViewModelEqualComparer())
                                                                 .Sum(o => o.Amount);

            delivery.TotalBank = delivery.Payments.Where(i => i.PayMethod == PayMethod.Bank)
                                                  .Distinct(new PaymentViewModelEqualComparer())
                                                  .Sum(o => o.Amount);

            delivery.TotalCash = delivery.Payments.Where(i => i.PayMethod == PayMethod.Cash
                                                           || i.PayMethod == PayMethod.OldPayCash
                                                           || i.PayMethod == PayMethod.ForCreditNote)
                                                           .Distinct(new PaymentViewModelEqualComparer())
                                                           .Sum(o => o.Amount) + creditNote;

            delivery.TotalCard = delivery.Payments.Where(i => i.PayMethod == PayMethod.Card
                                                           || i.PayMethod == PayMethod.OldPayCard)
                                                           .Distinct(new PaymentViewModelEqualComparer())
                                                           .Sum(o => o.Amount) + creditNote;

            delivery.TotalOld = delivery.Payments.Where(i => i.PayMethod == PayMethod.OldPayCash
                                                          || i.PayMethod == PayMethod.OldPayCard)
                                                          .Distinct(new PaymentViewModelEqualComparer())
                                                          .Sum(o => o.Amount) + creditNote;

            delivery.TotalWeight = delivery.Payments.Distinct(new PaymentViewModelEqualComparer())
                                           .Sum(o => o.Weight);
        }
    }
}
