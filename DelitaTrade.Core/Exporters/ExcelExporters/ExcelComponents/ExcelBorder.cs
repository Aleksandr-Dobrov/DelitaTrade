using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelitaTrade.Core.Exporters.ExcelExporters.ExcelComponents
{
    public class ExcelBorder
    {
        private XlLineStyle lineStyle;
        private XlBorderWeight borderWeight;
        private XlBordersIndex bordersIndex;
        private string? hexColor;

        public ExcelBorder()
        {
            lineStyle = XlLineStyle.xlContinuous;
            borderWeight = XlBorderWeight.xlThin;
            bordersIndex = XlBordersIndex.xlEdgeBottom;
            hexColor = null;
        }

        public ExcelBorder(XlLineStyle lineStyle, XlBorderWeight borderWeight, XlBordersIndex bordersIndex, string? hexColor = default)
        {
            this.lineStyle = lineStyle;
            this.borderWeight = borderWeight;
            this.bordersIndex = bordersIndex;
            this.hexColor = hexColor;
        }

        public XlLineStyle LineStyle => lineStyle;
        public XlBorderWeight BorderWeight => borderWeight;
        public XlBordersIndex BordersIndex => bordersIndex;
        public string? HexColor => hexColor;
    }
}
