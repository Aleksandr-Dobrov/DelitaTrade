namespace DelitaTrade.WpfViewModels
{
    public class WpfDayReportViewModel : WpfDayReportIdViewModel
    {
        public Dictionary<decimal, int> Banknotes { get; set; } = new Dictionary<decimal, int>();
    }
}
