using Microsoft.Office.Interop.Excel;
using System.Drawing;
using _Excel = Microsoft.Office.Interop.Excel;

namespace DelitaTrade.Models.Builder
{
    public class ExcelDrawing : ExcelWriter
    {
        public void BackgroundColorRange(Worksheet ws, XlRgbColor color, int xCell, int yCell, int toXCell, int toYCell)
        {
            _Excel.Range range = GetRange(ws, xCell, yCell, toXCell, toYCell);
            range.Interior.Color = color;
        }

        public void BackgroundColorRange(Worksheet ws, string hexColor, int xCell, int yCell, int toXCell, int toYCell)
        {
            _Excel.Range range = GetRange(ws, xCell, yCell, toXCell, toYCell);            
            range.Interior.Color = SetColor(hexColor);
        }

        public void BordersAroundDraw(Worksheet ws, int xCell, int yCell, int toXCell, int toYCell, XlLineStyle lineStyle, XlBorderWeight borderWeight, string hexColor)
        {
            _Excel.Range range = GetRange(ws, xCell, yCell, toXCell, toYCell);

            range.Borders.Color = SetColor(hexColor);            
            range.BorderAround2(lineStyle, borderWeight);
        }

        public void BorderDraw(Worksheet ws, int xCell, int yCell, int toXCell, int toYCell, XlLineStyle lineStyle, XlBorderWeight borderWeight, XlBordersIndex bordersIndex)
        {
            _Excel.Range range = GetRange(ws, xCell, yCell, toXCell, toYCell);
            range.Borders[bordersIndex].Weight = borderWeight;
            range.Borders[bordersIndex].LineStyle = lineStyle;
        }
    }
}
