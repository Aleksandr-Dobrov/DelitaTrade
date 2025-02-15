using DelitaTrade.Models.Interfaces.Builder;

namespace DelitaTrade.Models.Builder
{
    public class DayReportBuilder
    {
        private readonly IDayReportBuilder _builder;

        public DayReportBuilder(string inputPath, string path)
        {
            _builder = new ExcelBuilder(inputPath, path);  
        }

        public void CreateDayReport(DayReport dayReport)
        {
            using (_builder)
            { 
                //_builder.InitializedExporter(dayReport);
                _builder.BuildHeather();
                _builder.BuildBody();
                _builder.BuildFooter();
                _builder.Export();
            }
        }

        public string ExportedFilePath => _builder.ExportedFilePath;
    }
}
