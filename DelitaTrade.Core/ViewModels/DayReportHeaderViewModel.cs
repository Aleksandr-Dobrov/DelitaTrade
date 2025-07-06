using static DelitaTrade.Common.Constants.FormatConstant.DateTimeFormat;

namespace DelitaTrade.Core.ViewModels
{
    public class DayReportHeaderViewModel
    {
        public int Id { get; set; }
        public required DateTime Date { get; set; }

        public string? FormattedDate => Date.ToString(AppDateFormat);

    }
}
