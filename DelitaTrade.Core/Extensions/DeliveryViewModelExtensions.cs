using DelitaTrade.Common.Enums;
using DelitaTrade.Core.Comparers;
using DelitaTrade.Core.ViewModels.DeliveryModels;
using DelitaTrade.Infrastructure.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelitaTrade.Core.Extensions
{
    public static class DeliveryViewModelExtensions
    {
        public static void CalculateTotals(this DeliveryViewModel delivery)
        {
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
                                                           .Sum(o => o.Amount);

            delivery.TotalCard = delivery.Payments.Where(i => i.PayMethod == PayMethod.Card
                                                           || i.PayMethod == PayMethod.OldPayCard)
                                                           .Distinct(new PaymentViewModelEqualComparer())
                                                           .Sum(o => o.Amount);

            delivery.TotalOld = delivery.Payments.Where(i => i.PayMethod == PayMethod.OldPayCash
                                                          || i.PayMethod == PayMethod.OldPayCard)
                                                          .Distinct(new PaymentViewModelEqualComparer())
                                                          .Sum(o => o.Amount);

            delivery.TotalWeight = delivery.Payments.Distinct(new PaymentViewModelEqualComparer())
                                           .Sum(o => o.Weight);
        }
    }
}
