using DelitaTrade.Core.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelitaTrade.Core.Exporters.ExcelExporters
{
    public class DayReportExporter
    {
        private IDayReportBuilder _dayReportBuilder;

        public DayReportExporter()
        {
            _dayReportBuilder = new ExcelBuilder("../../../Models/Exporters/DayReport.xlsx");
            ExportCompleted += () => { };
            ExportStart += () => { };
            ExportFileCreate += (f) => { };
        }

        public event Action ExportCompleted;
        public event Action ExportStart;
        public event Action<string> ExportFileCreate;

        public async Task ExportDayReport(IExportedDayReport dayReport, string filePath, Func<string, bool> messageToCloseAndContinue)
        {
            try
            {
                ExportStart();
                var t = Task.Run(() =>
                {
                    using (_dayReportBuilder)
                    {
                        _dayReportBuilder.InitializedExporter(dayReport, filePath, messageToCloseAndContinue)
                                         .BuildHeather()
                                         .BuildBody()
                                         .BuildFooter()
                                         .Export();
                    }
                });
                await RiseEventWhenExportCompleted(t);
            }
            catch
            {                
                ExportCompleted();
            }
        }
        private async Task RiseEventWhenExportCompleted(Task task)
        {
            await task;
            ExportCompleted?.Invoke();
            ExportFileCreate?.Invoke(_dayReportBuilder.ExportedFilePath);
        }
    }
}
