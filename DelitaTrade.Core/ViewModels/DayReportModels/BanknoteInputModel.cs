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

        public decimal TotalOldValue { get; private set; }
        public decimal TotalNewValue { get; private set; }
        public decimal Balance => TotalNewValue - TotalOldValue;

        public DayReportBanknotesViewModel GetCalculatedBanknotes()
        {

            var banknoteModel = new DayReportBanknotesViewModel
            {
                Id = Id,
                Date = Date,
                TotalIncome = TotalIncome,
            };

            TotalOldValue = BanknoteOldValues.Sum(b => b.Key * b.Value);
            
            foreach (var banknote in BanknoteOldValues)
            {

                banknoteModel.Banknotes[banknote.Key] = banknote.Value + Banknotes[banknote.Key];

                if (banknoteModel.Banknotes[banknote.Key] < 0)
                {
                    throw new InvalidOperationException("Banknote count cannot be negative.");
                }
            }

            TotalNewValue = banknoteModel.Banknotes.Sum(b => b.Key * b.Value);

            return banknoteModel;
        }
    }
}
