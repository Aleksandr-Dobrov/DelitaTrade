using Microsoft.Office.Interop.Excel;
using System.Drawing;
using System.Globalization;
using _Excel = Microsoft.Office.Interop.Excel;

namespace DelitaTrade.Core.Exporters.ExcelExporters
{
    public class ExcelWriter
    {
        public void WriteDataToCell(Worksheet ws, string data, int xCell, int yCell)
        {
            ws.Cells[xCell, yCell].Value2 += data;
        }

        public void SetRowsRangeHeight(Worksheet ws, int startRow, int endRow,  int height)
        {
            for (int i = startRow; i <= endRow; i++)
            {
                ws.Rows[i].RowHeight = height;                
            }
        }

        public void SetRowHeight(Worksheet ws, int row, double height)
        {
            ws.Rows[row].RowHeight = height;
        }

        public void WriteDataToCell(Worksheet ws, string data, bool bold, int fontSize, XlHAlign hAlign, XlVAlign vAlign, int xCell, int yCell)
        {            
            _Excel.Range range = GetRange(ws, xCell, yCell, xCell, yCell);
            range.Font.Bold = bold;
            range.HorizontalAlignment = hAlign;
            range.VerticalAlignment = vAlign;
            range.Font.Size = fontSize;
            range.Font.Color = SetColor("#000000");
            WriteDataToCell(ws, data, xCell, yCell);
        }
        public void WriteDataToRange(Worksheet ws, string data, bool bold, int fontSize, bool isDate, XlHAlign hAlign, XlVAlign vAlign, int yCell, int xCell, int toYCell, int toXCell, bool isWrapText = false, bool isItalic = false)
        {
            _Excel.Range range = GetRange(ws, yCell, xCell, toYCell, toXCell);
            range.Merge();
            range.Font.Bold = bold;
            range.HorizontalAlignment = hAlign;
            range.VerticalAlignment = vAlign;            
            range.Font.Size = fontSize;
            range.Font.Italic = isItalic;
            range.WrapText = isWrapText;
            if (isDate)
            {
                range.NumberFormat = CultureInfo.CurrentCulture.DateTimeFormat.LongDatePattern;
            }

            WriteDataToCell(ws, data, yCell, xCell);
        }

        public void WriteDataToRange(Worksheet ws, string data, bool bold, int fontSize, bool isDate, XlHAlign hAlign, XlVAlign vAlign, int yCell, int xCell, int toYCell, int toXCell, string fontFamily, string? fontColor, bool isWrapText = false, bool isItalic = false, XlOrientation orientation = XlOrientation.xlHorizontal)
        {
            _Excel.Range range = GetRange(ws, yCell, xCell, toYCell, toXCell);
            range.Merge();
            range.Font.Bold = bold;
            range.HorizontalAlignment = hAlign;
            range.VerticalAlignment = vAlign;
            range.Font.Size = fontSize;
            range.Font.Name = fontFamily;
            range.Orientation = orientation;
            range.Font.Color = SetColor(fontColor);
            range.Font.Italic = isItalic;
            range.WrapText = isWrapText;
            if (isDate)
            {
                range.NumberFormat = CultureInfo.CurrentCulture.DateTimeFormat.LongDatePattern;
            }

            WriteDataToCell(ws, data, yCell, xCell);
        }

        protected _Excel.Range GetRange(Worksheet ws, int yCell, int xCell, int toYCell, int toXCell)
        {
            return ws.Range[ws.Cells[yCell, xCell], ws.Cells[toYCell, toXCell]];
        }

        protected int SetColor(string? hexColor)
        {
            if (hexColor == default)
            {
                hexColor = "#000000";
            }
            else if (hexColor.Length != 7 || hexColor[0] != '#')
            {
                hexColor = "#000000";
            }
            ColorConverter cc = new();
            int color = ColorTranslator.ToOle((Color)cc.ConvertFromString(hexColor));
            return color;
        }
    }
}
