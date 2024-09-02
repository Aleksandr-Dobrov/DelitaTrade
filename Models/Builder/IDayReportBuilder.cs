using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelitaTrade.Models.Builder
{
    public interface IDayReportBuilder
    {
        public Worksheet BuildSheet(Workbook workbook, int sheet);

        public void BuildHeather(Worksheet ws, DayReport dayReport);

        public void BuildBody(Worksheet ws, DayReport dayReport);

        public void BuildFooter(Worksheet ws, DayReport dayReport);
    }
}
