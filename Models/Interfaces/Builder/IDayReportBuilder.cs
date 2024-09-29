namespace DelitaTrade.Models.Interfaces.Builder
{
    public interface IDayReportBuilder : IDisposable
    {
        public string ExportedFilePath { get; }

        public void InitializedExporter(DayReport report);

        public void BuildHeather();

        public void BuildBody();

        public void BuildFooter();

        public void Export();
    }
}
