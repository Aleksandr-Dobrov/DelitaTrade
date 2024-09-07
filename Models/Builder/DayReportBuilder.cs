using Microsoft.Office.Interop.Excel;
using System.IO;
using _Excel = Microsoft.Office.Interop.Excel;

namespace DelitaTrade.Models.Builder
{
    public class DayReportBuilder
    {
        private readonly IDayReportBuilder _builder;

        private DayReport _dayReport;

        private _Application excel;
        private Workbook wb;
        private Worksheet ws;

        private string _inputPath;
        private string _path;
        private string _excelPath;

        public DayReportBuilder(string inputPath, string path)
        {
            excel = new _Excel.Application();
            _builder = new ExcelBuilder();            
            _inputPath = inputPath;
            Path = path;
        }

        public string Path
        {
            get => _path;
            set
            {
                _path = value;
            }
        }

        public string ExportedFilePath => _excelPath;

        private void Save()
        {
            wb.Save();
            wb.Close();
        }

        private void OpenFile()
        {
            CreateExportFile();
            wb = excel.Workbooks.Open(_excelPath);            
        }

        private void CreateSheet()
        {
           ws = _builder.BuildSheet(wb, 1);
        }

        private void CreateExportFile()
        {            
            File.Copy(_inputPath, Path, true);           
            FileInfo fileInfo = new FileInfo(Path);
            _excelPath = fileInfo.FullName;
        }

        public void CreateDayReport(DayReport dayReport)
        {
            _dayReport = dayReport;
            OpenFile();
            CreateSheet();
            _builder.BuildHeather(ws,_dayReport);
            _builder.BuildBody(ws,_dayReport);
            _builder.BuildFooter(ws,_dayReport);
            Export();
        }

        public void Export()
        {
            Save();
        }

        public void Dispose()
        {
            wb?.Close();
            File.Delete(_excelPath);
            if (File.Exists(_excelPath))
            {
                throw new InvalidOperationException($"Export failed!!! Can not delete export file - {_excelPath}. Try to end Excel process in Task Manager");
            }
        }
    }
}
