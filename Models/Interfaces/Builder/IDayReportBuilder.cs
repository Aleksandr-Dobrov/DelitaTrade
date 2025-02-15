using DelitaTrade.ViewModels.Interfaces;

namespace DelitaTrade.Models.Interfaces.Builder
{
    public interface IDayReportBuilder : IDisposable
    {
        public string ExportedFilePath { get; }

        public void InitializedExporter(IExportedDayReport report);

        public void BuildHeather();

        public void BuildBody();

        public void BuildFooter();

        public void Export();
    }
}
