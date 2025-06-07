using DelitaTrade.Core.Contracts;
using DelitaTrade.Core.Exporters.ExcelExporters.ExcelComponents;
using Microsoft.Extensions.Configuration;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelitaTrade.Core.Exporters.ExcelExporters
{
    public class ExcelStupidCardReturnProtocolBuilder : ExcelStupidReturnProtocolBuilder
    {
        public ExcelStupidCardReturnProtocolBuilder(IConfiguration configuration) : base(configuration)
        {
        }

        protected override BaseExcelComponent AddPayMethodComponent(BaseExcelComponent bodyHeaderComponent)
        {
            ExcelElement el = new ExcelElement(6, 1, 1, 14);
            el.AddBorderAround(new ExcelAroundBorder(XlLineStyle.xlContinuous, XlBorderWeight.xlMedium));
            bodyHeaderComponent.AddElement(el);

            el = new ExcelElement(6, 1, 1, 4, "PayMethod", _payMethodColor);
            el.AddContent(new ExcelContent("НАЧИН НА ПЛАЩАНЕ - КЛИЕНТ:", 12, XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter, true, true));
            el.AddBorder(new ExcelBorder(XlLineStyle.xlContinuous, XlBorderWeight.xlMedium, XlBordersIndex.xlEdgeRight));
            bodyHeaderComponent.AddElement(el);

            el = new ExcelElement(6, 5, 1, 1);
            el.AddBorder(new ExcelBorder(XlLineStyle.xlContinuous, XlBorderWeight.xlMedium, XlBordersIndex.xlEdgeRight));
            bodyHeaderComponent.AddElement(el);

            el = new ExcelElement(6, 6, 1, 1);
            el.AddContent(new ExcelContent("ПО БАНКОВ ПЪТ", 11, XlHAlign.xlHAlignLeft, XlVAlign.xlVAlignCenter, true));
            el.AddBorder(new ExcelBorder(XlLineStyle.xlContinuous, XlBorderWeight.xlMedium, XlBordersIndex.xlEdgeRight));
            bodyHeaderComponent.AddElement(el);

            el = new ExcelElement(6, 7, 1, 1);
            el.AddBorder(new ExcelBorder(XlLineStyle.xlContinuous, XlBorderWeight.xlMedium, XlBordersIndex.xlEdgeRight));
            bodyHeaderComponent.AddElement(el);
            
            el = new ExcelElement(6, 8, 1, 1);
            el.AddContent(new ExcelContent("С БАНКОВА КАРТА", 11, XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter, true));
            el.AddBorder(new ExcelBorder(XlLineStyle.xlContinuous, XlBorderWeight.xlMedium, XlBordersIndex.xlEdgeRight));
            bodyHeaderComponent.AddElement(el);

            el = new ExcelElement(6, 9, 1, 1);
            el.AddBorder(new ExcelBorder(XlLineStyle.xlContinuous, XlBorderWeight.xlMedium, XlBordersIndex.xlEdgeRight));
            bodyHeaderComponent.AddElement(el);

            el = new ExcelElement(6, 10, 1, 1);
            el.AddContent(new ExcelContent($"ПРИСПАДНАТО{Environment.NewLine}(В БРОЙ)", 11, XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter, true, false, true));
            el.AddBorder(new ExcelBorder(XlLineStyle.xlContinuous, XlBorderWeight.xlMedium, XlBordersIndex.xlEdgeRight));
            bodyHeaderComponent.AddElement(el);

            el = new ExcelElement(6, 11, 1, 1);
            el.AddBorder(new ExcelBorder(XlLineStyle.xlContinuous, XlBorderWeight.xlMedium, XlBordersIndex.xlEdgeRight));
            bodyHeaderComponent.AddElement(el);

            el = new ExcelElement(6, 12, 1, 3);
            el.AddContent(new ExcelContent($"НЕ Е ПРИСПАДНАТО{Environment.NewLine}(В БРОЙ)", 10, XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter, true, false, true));
            el.AddBorder(new ExcelBorder(XlLineStyle.xlContinuous, XlBorderWeight.xlMedium, XlBordersIndex.xlEdgeRight));
            bodyHeaderComponent.AddElement(el);

            el = new ExcelElement(6, ExportPayMethodCardCategories.PayMethods[_returnProtocol.PayMethod], 1, 1);
            el.AddContent(new ExcelContent("✖", 26, XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter, true, false, false, false, _dataFontFamily, _darkGrayColor));
            bodyHeaderComponent.AddElement(el);

            return bodyHeaderComponent;
        }
    }
}
