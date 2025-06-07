using Microsoft.Office.Interop.Excel;
using _Excel = Microsoft.Office.Interop.Excel;

namespace DelitaTrade.Core.Exporters.ExcelExporters
{
    public class ExcelDrawer : ExcelWriter
    {
        public void BackgroundColorRange(Worksheet ws, XlRgbColor color, int yCell, int xCell, int toYCell, int toXCell)
        {
            _Excel.Range range = GetRange(ws, yCell, xCell, toYCell, toXCell);
            range.Interior.Color = color;
        }

        public void BackgroundColorRange(Worksheet ws, string hexColor, int yCell, int xCell, int toYCell, int toXCell)
        {
            _Excel.Range range = GetRange(ws, yCell, xCell, toYCell, toXCell);
            range.Interior.Color = SetColor(hexColor);
        }

        public void BordersAroundDraw(Worksheet ws, int yCell, int xCell, int toYCell, int toXCell, XlLineStyle lineStyle, XlBorderWeight borderWeight, string hexColor)
        {
            _Excel.Range range = GetRange(ws, yCell, xCell, toYCell, toXCell);

            range.Borders.Color = SetColor(hexColor);
            range.BorderAround2(lineStyle, borderWeight);            
        }

        public void BorderDraw(Worksheet ws, int yCell, int xCell, int toYCell, int toXCell, XlLineStyle lineStyle, XlBorderWeight borderWeight, XlBordersIndex bordersIndex, string? color = default)
        {
            _Excel.Range range = GetRange(ws, yCell, xCell, toYCell, toXCell);
            range.Borders[bordersIndex].Weight = borderWeight;
            range.Borders[bordersIndex].LineStyle = lineStyle;
            range.Borders[bordersIndex].Color = SetColor(color);
        }

        public void BorderClear(Worksheet ws, int yCell, int xCell, int toYCell, int toXCell)
        {
            _Excel.Range range = GetRange(ws, yCell, xCell, toYCell, toXCell);
            range.Borders.LineStyle = XlLineStyle.xlLineStyleNone;
        }

        public void BorderClear(Worksheet ws, int yCell, int xCell, int toYCell, int toXCell, XlBordersIndex bordersIndex)
        {
            _Excel.Range range = GetRange(ws, yCell, xCell, toYCell, toXCell);
            range.Borders[bordersIndex].LineStyle = XlLineStyle.xlLineStyleNone;
        }

        public void DrawPictureToRange(Worksheet ws, int yCell, int xCell, int toYCell, int toXCell, float imageWidth, float imageHeight, string path, float leftOfSet = 0, float topOfSet = 0)
        {
            _Excel.Range range = GetRange(ws, yCell, xCell, toYCell, toXCell);
            float left = 0;
            float top = 0;
            
            if(float.TryParse($"{range.Left}", out left) && float.TryParse($"{range.Top}", out top))
            {
                left += leftOfSet;
                top += topOfSet;
                ws.Shapes.AddPicture(path, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue,left, top, imageWidth, imageHeight);
            }
            else
            {
                WriteDataToRange(ws, $"Can`t show image", false, 14, false
                    , XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter
                    , yCell, xCell, toYCell, toXCell, true);
            }                
        }
    }
}
