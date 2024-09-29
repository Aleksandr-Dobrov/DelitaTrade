using Microsoft.Office.Interop.Excel;
using System.IO;
using _Excel = Microsoft.Office.Interop.Excel;

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
            _builder.InitializedExporter(dayReport);
            _builder.BuildHeather();
            _builder.BuildBody();
            _builder.BuildFooter();
            _builder.Export();
        }

        public string ExportedFilePath => _builder.ExportedFilePath;
    }
}
