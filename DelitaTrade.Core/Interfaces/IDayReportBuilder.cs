namespace DelitaTrade.Core.Interfaces
{
    public interface IDayReportBuilder : IDisposable
    {
        public string ExportedFilePath { get; }

        public IDayReportBuilder InitializedExporter(IExportedDayReport report, string filePath, Func<string, bool> messageToCloseAndContinue);

        public IDayReportBuilder BuildHeather();

        public IDayReportBuilder BuildBody();

        public IDayReportBuilder BuildFooter();

        public void Export();
    }
}
