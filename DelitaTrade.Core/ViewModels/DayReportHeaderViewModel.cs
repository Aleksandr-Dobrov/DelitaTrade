using DelitaTrade.Common.Constants;

namespace DelitaTrade.Core.ViewModels
{
    public class DayReportHeaderViewModel
    {
        public int Id { get; set; }
        public required DateTime Date { get; set; }

        public string? FormattedDate => Date.ToString(FormatConstant.DateTimeFormat.DayReportHeader);

    }
}
