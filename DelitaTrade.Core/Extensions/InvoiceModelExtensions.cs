using DelitaTrade.Common.Enums;
using DelitaTrade.Core.ViewModels.DeliveryModels;
using DelitaTrade.Core.ViewModels.InvoiceModels;
using DelitaTrade.Infrastructure.Data.Models;

namespace DelitaTrade.Core.Extensions
{
    public static class InvoiceModelExtensions
    {
        public static InvoiceInDayReportAdvanceInputModel? GetInputModel(this InvoiceInDayReportAdvanceViewModel model)
        {
            switch (model.PayMethod)
            {
                case PayMethod.Cash:
                case PayMethod.Card:
                case PayMethod.Bank:
                    return new InvoiceInDayReportAdvanceInputModel()
                    {
                        Id = model.Id,
                        DayReportId = model.DayReportId,
                        DeliveryId = model.DeliveryId,
                        InvoiceNumber = model.InvoiceNumber,
                        Amount = model.Amount,
                        Balance = model.Balance,
                        Paid = model.Paid,
                        Income = model.Balance,
                        PaymentTypeId = (int)model.PayMethod,
                        PaymentTypes =
                        [
                            new () { InvoiceType =PayMethod.Cash },
                            new () { InvoiceType = PayMethod.Card },
                            new () { InvoiceType = PayMethod.Bank }
                        ],
                        Reasons =
                        [
                            new () { PayMethod = InvoiceAdvancePayMethods.Partial },
                            new () { PayMethod = InvoiceAdvancePayMethods.NotPay },
                            new () { PayMethod = InvoiceAdvancePayMethods.ForCreditNote },
                            new () { PayMethod = InvoiceAdvancePayMethods.Cancelation }
                        ]
                    };
                case PayMethod.OldPayCard:
                case PayMethod.OldPayCash:
                    return new InvoiceInDayReportAdvanceInputModel()
                    {
                        Id = model.Id,
                        DayReportId = model.DayReportId,
                        DeliveryId = model.DeliveryId,
                        InvoiceNumber = model.InvoiceNumber,
                        Amount = model.Amount,
                        Balance = model.Balance,
                        Paid = model.Paid,
                        Income = model.Balance,
                        PaymentTypeId = (int)model.PayMethod,
                        PaymentTypes =
                        [
                            new () { InvoiceType = PayMethod.OldPayCash },
                            new (){ InvoiceType = PayMethod.OldPayCard },
                        ],
                        Reasons =
                        [
                            new () { PayMethod = InvoiceAdvancePayMethods.Partial },
                            new () { PayMethod = InvoiceAdvancePayMethods.NotPay },
                        ]
                    };
                default:
                    return null;
            }
        }
    }
}
