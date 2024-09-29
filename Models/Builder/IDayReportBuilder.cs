using Microsoft.Office.Interop.Excel;

namespace DelitaTrade.Models.Builder
{
    public interface IDayReportBuilder
    {
        public void InitializedExporter(DayReport report);
        public void BuildHeather();

        public void BuildBody();

        public void BuildFooter();

        public void Export();

        public string ExportedFilePath { get; }
    }
}
