namespace DelitaTrade.Core.ViewModels.DayReportModels
{
    public class BanknoteInputModel : DayReportBanknotesViewModel
    {
        public Dictionary<decimal, int> BanknoteOldValues { get; set; } = new Dictionary<decimal, int>
        {
            { 0.01m, 0 },
            { 0.02m, 0 },
            { 0.05m, 0 },
            { 0.1m, 0 },
            { 0.2m, 0 },
            { 0.5m, 0 },
            { 1.0m, 0 },
            { 2.0m, 0 },
            { 5.0m, 0 },
            { 10.0m, 0 },
            { 20.0m, 0 },
            { 50.0m, 0 },
            { 100.0m, 0 },
        };
    }
}
