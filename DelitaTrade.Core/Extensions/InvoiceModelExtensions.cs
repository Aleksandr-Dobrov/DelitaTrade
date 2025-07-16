using DelitaTrade.Core.ViewModels.InvoiceModels;

namespace DelitaTrade.Core.Extensions
{
    public static class InvoiceModelExtensions
    {
        public static InvoiceInDayReportAdvanceInputModel GetInputModel(this InvoiceInDayReportAdvanceViewModel model)
        {
            return new InvoiceInDayReportAdvanceInputModel()
            {
                Id = model.Id,
                DayReportId = model.DayReportId,
                DeliveryId = model.DeliveryId,
                InvoiceNumber = model.InvoiceNumber,
                Amount = model.Amount,
                Balance = model.Balance,
                Paid = model.Paid
            };
        }
    }
}
