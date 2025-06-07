using Microsoft.Office.Interop.Excel;

namespace DelitaTrade.Core.Exporters.ExcelExporters.ExcelComponents
{
    public class ExcelAroundBorder
    {
        private List<ExcelBorder> _borders = new();

        private List<XlBordersIndex> _bordersIndex = new()
        {
            XlBordersIndex.xlEdgeTop,
            XlBordersIndex.xlEdgeBottom,
            XlBordersIndex.xlEdgeLeft,
            XlBordersIndex.xlEdgeRight
        };

        public ExcelAroundBorder(XlLineStyle lineStyle, XlBorderWeight borderWeight, string? hexColor = null)
        {
            InitiateBorders(lineStyle, borderWeight, hexColor);
        }

        public IEnumerable<ExcelBorder> Borders => _borders;

        private void InitiateBorders(XlLineStyle lineStyle, XlBorderWeight borderWeight, string? hexColor = null)
        {
            for (int i = 0; i < 4; i++)
            {
                _borders.Add(new ExcelBorder(lineStyle, borderWeight, _bordersIndex[i], null));
            }
        }
    }
}