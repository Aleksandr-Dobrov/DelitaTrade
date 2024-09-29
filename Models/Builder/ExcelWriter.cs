using Microsoft.Office.Interop.Excel;
using System.Drawing;
using System.Globalization;
using _Excel = Microsoft.Office.Interop.Excel;

namespace DelitaTrade.Models.Builder
{
    public class ExcelWriter
    {        
        public void WriteDataToCell(Worksheet ws, string data, int xCell, int yCell)
        {
            ws.Cells[xCell, yCell].Value2 = data;
        }

        public void WriteDataToCell(Worksheet ws, string data, bool bold, int fontSize, XlHAlign hAlign, XlVAlign vAlign, int xCell, int yCell)
        {
            _Excel.Range range = GetRange(ws, xCell, yCell, xCell, yCell);
            range.Font.Bold = bold;
            range.HorizontalAlignment = hAlign;
            range.VerticalAlignment = vAlign;
            range.Font.Size = fontSize;
            WriteDataToCell(ws, data, xCell, yCell);
        }
        public void WriteDataToRange(Worksheet ws, string data, bool bold, int fontSize, bool isDate, XlHAlign hAlign, XlVAlign vAlign, int xCell, int yCell, int toXCell, int toYCell)
        {
            _Excel.Range range = GetRange(ws, xCell, yCell, toXCell, toYCell);
            range.Merge();
            range.Font.Bold = bold;
            range.HorizontalAlignment = hAlign;
            range.VerticalAlignment = vAlign;
            range.Font.Size = fontSize;
            if (isDate) 
            {
                range.NumberFormat = CultureInfo.CurrentCulture.DateTimeFormat.LongDatePattern;
            }

            WriteDataToCell(ws, data, xCell, yCell);
        }

        protected _Excel.Range GetRange(Worksheet ws, int xCell, int yCell, int toXCell, int toYCell)
        {
            return ws.Range[ws.Cells[xCell, yCell], ws.Cells[toXCell, toYCell]];
        }

        protected int SetColor(string hexColor)
        {
            if (hexColor == default)
            {
                hexColor = "#000000";
            }
            ColorConverter cc = new();
            int color = ColorTranslator.ToOle((Color)cc.ConvertFromString(hexColor));
            return color;
        }
    }
}
