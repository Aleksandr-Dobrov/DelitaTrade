using DelitaTrade.Core.Interfaces;
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
    public class ExcelCrisisReturnProtocolBuilder : ExcelReturnProtocolBuilder
    {
        private const int _crisisPageSize = 38; 
        private const int _maxProductCountOnPage = 7;

        public ExcelCrisisReturnProtocolBuilder(IConfiguration configuration) : base(configuration)
        {
        }
        public override IReturnProtocolBuilder BuildHeather()
        {
            return base.BuildHeather();
        }
        public override IReturnProtocolBuilder BuildBody()
        {
            BuildBodyHeather();
            int productNumber = 1;
            foreach (var item in _returnProtocol.Products)
            {
                if (productNumber > _maxProductCountOnPage) 
                { 
                    BuildFooter();
                    BuildHeather();
                    BuildBodyHeather();
                    productNumber = 1;
                }

                AddBodyListElement(item, productNumber);
                productNumber++;
            }
            return this;
        }
        public override IReturnProtocolBuilder BuildFooter()
        {
            AddRow(2);
            _writer.WriteDataToRange(_ws, "Приел Склад:", true, 10, false, XlHAlign.xlHAlignRight, XlVAlign.xlVAlignBottom, CurrentRow, 2, CurrentRow, 2);
            _writer.WriteDataToRange(_ws, "Подпис", false, 9, false, XlHAlign.xlHAlignLeft, XlVAlign.xlVAlignTop, CurrentRow + 1, 7, CurrentRow + 1, 7);
            _writer.WriteDataToRange(_ws, "Дата", false, 9, false, XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignTop, CurrentRow + 1, 8, CurrentRow + 1, 8);

            _drawer.BorderDraw(_ws, CurrentRow, 3, CurrentRow, 9, XlLineStyle.xlDot, XlBorderWeight.xlThin, XlBordersIndex.xlEdgeBottom);

            AddRow(2);
            _writer.WriteDataToRange(_ws, "Съгласувано (Драгов / Гаджев):", true, 10, false, XlHAlign.xlHAlignRight, XlVAlign.xlVAlignBottom, CurrentRow, 2, CurrentRow, 2);
            _writer.WriteDataToRange(_ws, "Подпис", false, 9, false, XlHAlign.xlHAlignLeft, XlVAlign.xlVAlignTop, CurrentRow + 1, 7, CurrentRow + 1, 7);
            _writer.WriteDataToRange(_ws, "Дата", false, 9, false, XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignTop, CurrentRow + 1, 8, CurrentRow + 1, 8);

            _drawer.BorderDraw(_ws, CurrentRow, 3, CurrentRow, 9, XlLineStyle.xlDot, XlBorderWeight.xlThin, XlBordersIndex.xlEdgeBottom);

            AddRow(2);
            _writer.WriteDataToRange(_ws, "Издал кредитно известие:", true, 10, false, XlHAlign.xlHAlignRight, XlVAlign.xlVAlignBottom, CurrentRow, 2, CurrentRow, 2);
            _writer.WriteDataToRange(_ws, "Подпис", false, 9, false, XlHAlign.xlHAlignLeft, XlVAlign.xlVAlignTop, CurrentRow + 1, 7, CurrentRow + 1, 7);
            _writer.WriteDataToRange(_ws, "Дата", false, 9, false, XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignTop, CurrentRow + 1, 8, CurrentRow + 1, 8);

            _drawer.BorderDraw(_ws, CurrentRow, 3, CurrentRow, 9, XlLineStyle.xlDot, XlBorderWeight.xlThin, XlBordersIndex.xlEdgeBottom);

            AddRow(1);

            _writer.WriteDataToCell(_ws, "NB!", true, 8, XlHAlign.xlHAlignLeft, XlVAlign.xlVAlignCenter, CurrentRow, 3);
            AddRow(1);
            _writer.WriteDataToRange(_ws, "Кредитно Известие за върната стока се издава в деня на получаването и в склада!", false, 8, false, XlHAlign.xlHAlignLeft, XlVAlign.xlVAlignCenter, CurrentRow, 3, CurrentRow, 9);
            AddRow(1);
            _writer.WriteDataToRange(_ws, "Кредитното Известие се предоставя на клиента в деня след издаването му (с случай че има заявка) или най-късно 2 дни след издаването му, чрез нарочно посещение от страна на търговски представител!"
                , false, 8, false, XlHAlign.xlHAlignLeft, XlVAlign.xlVAlignCenter, CurrentRow, 3, CurrentRow + 1, 9, true);
            AddRow(2);
            _writer.WriteDataToRange(_ws, "!!! ВАЖНО - В СЛУЧАЙ НА КРИЗИСНА СИТУАЦИЯ, МОЛЯ ОБЪРНЕТЕ И ПОПЪЛНЕТЕ ИНФОРМАЦИЯТА ОТ СТР. 2 !!!", true, 8, false, XlHAlign.xlHAlignLeft, XlVAlign.xlVAlignCenter, CurrentRow, 2, CurrentRow, 7);
            _writer.WriteDataToRange(_ws, $"Лист {Math.Ceiling(CurrentPage / 2.0)} | Стр. 1", false, 8, false, XlHAlign.xlHAlignRight, XlVAlign.xlVAlignCenter, CurrentRow, 8, CurrentRow, 9);

            BuildCrisisPage();

            return this;
        } 
        
        private void BuildCrisisPage()
        {
            NewPage(_crisisPageSize);

            AddRow(1);
            _writer.WriteDataToRange(_ws, "ПРЕНАСОЧВАНЕ НА РЕКЛАМАЦИЯ КЪМ КРИЗЕСЕН ЕКИП", true, 14, false, XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter, CurrentRow, 1, CurrentRow + 1, 9);

            AddRow(2);
            _writer.SetRowHeight(_ws, CurrentRow, 25);
            _writer.WriteDataToRange(_ws, "ЗА ДОПЪЛНИТЕЛНА ИНФОРМАЦИЯ ОТ:", true, 10, false, XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter, CurrentRow, 1, CurrentRow, 2, true);
            _drawer.BorderDraw(_ws, CurrentRow, 1, CurrentRow, 9, XlLineStyle.xlContinuous, XlBorderWeight.xlMedium, XlBordersIndex.xlEdgeTop);


            AddRow(1);
            _writer.SetRowHeight(_ws, CurrentRow, 25);
            _writer.WriteDataToRange(_ws, "ЛОГИСТИКА", false, 12, false, XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter, CurrentRow, 2, CurrentRow, 2);
            _drawer.BorderDraw(_ws, CurrentRow, 1, CurrentRow, 9, XlLineStyle.xlContinuous, XlBorderWeight.xlThin, XlBordersIndex.xlEdgeTop);


            AddRow(1);
            _writer.SetRowHeight(_ws, CurrentRow, 25);
            _writer.WriteDataToRange(_ws, "СКЛАД", false, 12, false, XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter, CurrentRow, 2, CurrentRow, 2);
            _drawer.BorderDraw(_ws, CurrentRow, 1, CurrentRow, 9, XlLineStyle.xlContinuous, XlBorderWeight.xlThin, XlBordersIndex.xlEdgeTop);


            AddRow(1);
            _writer.SetRowHeight(_ws, CurrentRow, 25);
            _writer.WriteDataToRange(_ws, "ПРОИЗВОДСТВО", false, 12, false, XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter, CurrentRow, 2, CurrentRow, 2);
            _drawer.BorderDraw(_ws, CurrentRow, 1, CurrentRow, 9, XlLineStyle.xlContinuous, XlBorderWeight.xlThin, XlBordersIndex.xlEdgeTop);
            _drawer.BorderDraw(_ws, CurrentRow, 1, CurrentRow, 9, XlLineStyle.xlContinuous, XlBorderWeight.xlMedium, XlBordersIndex.xlEdgeBottom);
            _drawer.BorderDraw(_ws, CurrentRow - 3, 9, CurrentRow, 9, XlLineStyle.xlContinuous, XlBorderWeight.xlMedium, XlBordersIndex.xlEdgeRight);


            _drawer.BordersAroundDraw(_ws, CurrentRow - 3, 1, CurrentRow, 2, XlLineStyle.xlContinuous, XlBorderWeight.xlMedium, "#000000");

            AddRow(1);
            _writer.SetRowHeight(_ws, CurrentRow, 25);
            _writer.WriteDataToRange(_ws, "ПРЕДАДЕНА    ИНФОРМАЦИЯ ОТ:", true, 11, false, XlHAlign.xlHAlignRight, XlVAlign.xlVAlignCenter, CurrentRow, 2, CurrentRow, 2, true);

            AddRow(1);

            _drawer.BorderDraw(_ws, CurrentRow, 3, CurrentRow, 5, XlLineStyle.xlDot, XlBorderWeight.xlThin, XlBordersIndex.xlEdgeTop);
            _drawer.BorderDraw(_ws, CurrentRow, 6, CurrentRow, 6, XlLineStyle.xlDot, XlBorderWeight.xlThin, XlBordersIndex.xlEdgeTop);
            _drawer.BorderDraw(_ws, CurrentRow, 7, CurrentRow, 7, XlLineStyle.xlDot, XlBorderWeight.xlThin, XlBordersIndex.xlEdgeTop);
            _drawer.BorderDraw(_ws, CurrentRow, 8, CurrentRow, 9, XlLineStyle.xlDot, XlBorderWeight.xlThin, XlBordersIndex.xlEdgeTop);

            _writer.WriteDataToRange(_ws, "Име и Фамилия", false, 10, false, XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter, CurrentRow, 3, CurrentRow, 5, false, true);
            _writer.WriteDataToRange(_ws, "Длъжност", false, 10, false, XlHAlign.xlHAlignRight, XlVAlign.xlVAlignCenter, CurrentRow, 6, CurrentRow, 6, false, true); 
            _writer.WriteDataToRange(_ws, "Дата", false, 10, false, XlHAlign.xlHAlignRight, XlVAlign.xlVAlignCenter, CurrentRow, 7, CurrentRow, 7, false, true);
            _writer.WriteDataToRange(_ws, "Подпис", false, 10, false, XlHAlign.xlHAlignRight, XlVAlign.xlVAlignCenter, CurrentRow, 8, CurrentRow, 8, false, true);

            AddRow(1);
            _writer.SetRowHeight(_ws, CurrentRow, 5);
            
            AddRow(1);
            _writer.SetRowsRangeHeight(_ws, CurrentRow, CurrentRow + 3, 25);
            _writer.WriteDataToRange(_ws, "Подробно описание на рекламация, която може да доведе или води до опасност за човешкото здраве:", 
                true, 10, false, XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter, CurrentRow, 1, CurrentRow + 3, 2, true);
            _drawer.BordersAroundDraw(_ws, CurrentRow, 1, CurrentRow + 3, 2, XlLineStyle.xlContinuous, XlBorderWeight.xlMedium, "#000000");
            _drawer.BorderDraw(_ws, CurrentRow, 1, CurrentRow, 9, XlLineStyle.xlContinuous, XlBorderWeight.xlMedium, XlBordersIndex.xlEdgeTop);
            _drawer.BorderDraw(_ws, CurrentRow + 1, 1, CurrentRow + 1, 9, XlLineStyle.xlContinuous, XlBorderWeight.xlThin, XlBordersIndex.xlEdgeTop);
            _drawer.BorderDraw(_ws, CurrentRow + 2, 1, CurrentRow + 2, 9, XlLineStyle.xlContinuous, XlBorderWeight.xlThin, XlBordersIndex.xlEdgeTop);
            _drawer.BorderDraw(_ws, CurrentRow + 3, 1, CurrentRow + 3, 9, XlLineStyle.xlContinuous, XlBorderWeight.xlThin, XlBordersIndex.xlEdgeTop);
            _drawer.BorderDraw(_ws, CurrentRow + 3, 1, CurrentRow + 3, 9, XlLineStyle.xlContinuous, XlBorderWeight.xlMedium, XlBordersIndex.xlEdgeBottom);
            _drawer.BorderDraw(_ws, CurrentRow, 9, CurrentRow + 3, 9, XlLineStyle.xlContinuous, XlBorderWeight.xlMedium, XlBordersIndex.xlEdgeRight);

            AddRow(4);
            _writer.SetRowHeight(_ws, CurrentRow, 25);
            _writer.WriteDataToRange(_ws, "Уведомил:", true, 12, false, XlHAlign.xlHAlignRight, XlVAlign.xlVAlignCenter, CurrentRow, 2, CurrentRow, 2, true);

            AddRow(1);

            _drawer.BorderDraw(_ws, CurrentRow, 3, CurrentRow, 5, XlLineStyle.xlDot, XlBorderWeight.xlThin, XlBordersIndex.xlEdgeTop);
            _drawer.BorderDraw(_ws, CurrentRow, 6, CurrentRow, 6, XlLineStyle.xlDot, XlBorderWeight.xlThin, XlBordersIndex.xlEdgeTop);
            _drawer.BorderDraw(_ws, CurrentRow, 7, CurrentRow, 7, XlLineStyle.xlDot, XlBorderWeight.xlThin, XlBordersIndex.xlEdgeTop);
            _drawer.BorderDraw(_ws, CurrentRow, 8, CurrentRow, 9, XlLineStyle.xlDot, XlBorderWeight.xlThin, XlBordersIndex.xlEdgeTop);

            _writer.WriteDataToRange(_ws, "Име и Фамилия", false, 10, false, XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter, CurrentRow, 3, CurrentRow, 5);
            _writer.WriteDataToRange(_ws, "Длъжност", false, 10, false, XlHAlign.xlHAlignRight, XlVAlign.xlVAlignCenter, CurrentRow, 6, CurrentRow, 6, false, true);
            _writer.WriteDataToRange(_ws, "Дата", false, 10, false, XlHAlign.xlHAlignRight, XlVAlign.xlVAlignCenter, CurrentRow, 7, CurrentRow, 7, false, true);
            _writer.WriteDataToRange(_ws, "Подпис", false, 10, false, XlHAlign.xlHAlignRight, XlVAlign.xlVAlignCenter, CurrentRow, 8, CurrentRow, 8, false, true);

            AddRow(1);
            _writer.WriteDataToRange(_ws, "Приел", true, 12, false, XlHAlign.xlHAlignRight, XlVAlign.xlVAlignCenter, CurrentRow, 2, CurrentRow, 2, true);
            AddRow(1);
            _writer.WriteDataToRange(_ws, "(Отговорник Кризисен Екип):", true, 10, false, XlHAlign.xlHAlignRight, XlVAlign.xlVAlignCenter, CurrentRow, 2, CurrentRow, 2, true);

            AddRow(1);

            _drawer.BorderDraw(_ws, CurrentRow, 3, CurrentRow, 6, XlLineStyle.xlDot, XlBorderWeight.xlThin, XlBordersIndex.xlEdgeTop);
            _drawer.BorderDraw(_ws, CurrentRow, 7, CurrentRow, 7, XlLineStyle.xlDot, XlBorderWeight.xlThin, XlBordersIndex.xlEdgeTop);
            _drawer.BorderDraw(_ws, CurrentRow, 8, CurrentRow, 9, XlLineStyle.xlDot, XlBorderWeight.xlThin, XlBordersIndex.xlEdgeTop);

            _writer.WriteDataToRange(_ws, "Име и Фамилия", false, 10, false, XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter, CurrentRow, 3, CurrentRow, 6, false, true);
            _writer.WriteDataToRange(_ws, "Дата", false, 10, false, XlHAlign.xlHAlignRight, XlVAlign.xlVAlignCenter, CurrentRow, 7, CurrentRow, 7, false, true);
            _writer.WriteDataToRange(_ws, "Подпис", false, 10, false, XlHAlign.xlHAlignRight, XlVAlign.xlVAlignCenter, CurrentRow, 8, CurrentRow, 8, false, true);

            AddRow(1);
            _writer.SetRowHeight(_ws, CurrentRow, 5);

            AddRow(1);
            _writer.SetRowsRangeHeight(_ws, CurrentRow, CurrentRow + 3, 25);
            _writer.WriteDataToRange(_ws, "ПРЕДПРИЕТИ КОРЕКТИВНИ ДЕЙСТВИЯ:",
                true, 12, false, XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter, CurrentRow, 1, CurrentRow + 3, 2, true);
            _drawer.BordersAroundDraw(_ws, CurrentRow, 1, CurrentRow + 3, 2, XlLineStyle.xlContinuous, XlBorderWeight.xlMedium, "#000000");
            _drawer.BorderDraw(_ws, CurrentRow, 1, CurrentRow, 9, XlLineStyle.xlContinuous, XlBorderWeight.xlMedium, XlBordersIndex.xlEdgeTop);
            _drawer.BorderDraw(_ws, CurrentRow + 1, 1, CurrentRow + 1, 9, XlLineStyle.xlContinuous, XlBorderWeight.xlThin, XlBordersIndex.xlEdgeTop);
            _drawer.BorderDraw(_ws, CurrentRow + 2, 1, CurrentRow + 2, 9, XlLineStyle.xlContinuous, XlBorderWeight.xlThin, XlBordersIndex.xlEdgeTop);
            _drawer.BorderDraw(_ws, CurrentRow + 3, 1, CurrentRow + 3, 9, XlLineStyle.xlContinuous, XlBorderWeight.xlThin, XlBordersIndex.xlEdgeTop);
            _drawer.BorderDraw(_ws, CurrentRow + 3, 1, CurrentRow + 3, 9, XlLineStyle.xlContinuous, XlBorderWeight.xlMedium, XlBordersIndex.xlEdgeBottom);
            _drawer.BorderDraw(_ws, CurrentRow, 9, CurrentRow + 3, 9, XlLineStyle.xlContinuous, XlBorderWeight.xlMedium, XlBordersIndex.xlEdgeRight);

            AddRow(4);
            _writer.SetRowHeight(_ws, CurrentRow, 25);
            _writer.WriteDataToRange(_ws, "ОТГОВОРНИК КРИЗИСЕН ЕКИП:", true, 10, false, XlHAlign.xlHAlignRight, XlVAlign.xlVAlignCenter, CurrentRow, 1, CurrentRow, 2, true);

            AddRow(1);

            _drawer.BorderDraw(_ws, CurrentRow, 3, CurrentRow, 6, XlLineStyle.xlDot, XlBorderWeight.xlThin, XlBordersIndex.xlEdgeTop);
            _drawer.BorderDraw(_ws, CurrentRow, 7, CurrentRow, 7, XlLineStyle.xlDot, XlBorderWeight.xlThin, XlBordersIndex.xlEdgeTop);
            _drawer.BorderDraw(_ws, CurrentRow, 8, CurrentRow, 9, XlLineStyle.xlDot, XlBorderWeight.xlThin, XlBordersIndex.xlEdgeTop);

            _writer.WriteDataToRange(_ws, "Име и Фамилия", false, 10, false, XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter, CurrentRow, 3, CurrentRow, 6, false, true);
            _writer.WriteDataToRange(_ws, "Дата", false, 10, false, XlHAlign.xlHAlignRight, XlVAlign.xlVAlignCenter, CurrentRow, 7, CurrentRow, 7, false, true);
            _writer.WriteDataToRange(_ws, "Подпис", false, 10, false, XlHAlign.xlHAlignRight, XlVAlign.xlVAlignCenter, CurrentRow, 8, CurrentRow, 8, false, true);

            AddRow(1);
            _writer.SetRowHeight(_ws, CurrentRow, 5);

            AddRow(1);
            _writer.SetRowsRangeHeight(_ws, CurrentRow, CurrentRow + 3, 25);
            _writer.WriteDataToRange(_ws, "РЕЗУЛТАТ ОТ КОРЕКТИВНИ ДЕЙСТВИЯ:",
                true, 12, false, XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter, CurrentRow, 1, CurrentRow + 3, 2, true);
            _drawer.BordersAroundDraw(_ws, CurrentRow, 1, CurrentRow + 3, 2, XlLineStyle.xlContinuous, XlBorderWeight.xlMedium, "#000000");
            _drawer.BorderDraw(_ws, CurrentRow, 1, CurrentRow, 9, XlLineStyle.xlContinuous, XlBorderWeight.xlMedium, XlBordersIndex.xlEdgeTop);
            _drawer.BorderDraw(_ws, CurrentRow + 1, 1, CurrentRow + 1, 9, XlLineStyle.xlContinuous, XlBorderWeight.xlThin, XlBordersIndex.xlEdgeTop);
            _drawer.BorderDraw(_ws, CurrentRow + 2, 1, CurrentRow + 2, 9, XlLineStyle.xlContinuous, XlBorderWeight.xlThin, XlBordersIndex.xlEdgeTop);
            _drawer.BorderDraw(_ws, CurrentRow + 3, 1, CurrentRow + 3, 9, XlLineStyle.xlContinuous, XlBorderWeight.xlThin, XlBordersIndex.xlEdgeTop);
            _drawer.BorderDraw(_ws, CurrentRow + 3, 1, CurrentRow + 3, 9, XlLineStyle.xlContinuous, XlBorderWeight.xlMedium, XlBordersIndex.xlEdgeBottom);
            _drawer.BorderDraw(_ws, CurrentRow, 9, CurrentRow + 3, 9, XlLineStyle.xlContinuous, XlBorderWeight.xlMedium, XlBordersIndex.xlEdgeRight);

            AddRow(4);
            _writer.SetRowHeight(_ws, CurrentRow, 25);
            _writer.WriteDataToRange(_ws, "ОТГОВОРНИК КРИЗИСЕН ЕКИП:", true, 10, false, XlHAlign.xlHAlignRight, XlVAlign.xlVAlignCenter, CurrentRow, 1, CurrentRow, 2, true);

            AddRow(1);

            _drawer.BorderDraw(_ws, CurrentRow, 3, CurrentRow, 6, XlLineStyle.xlDot, XlBorderWeight.xlThin, XlBordersIndex.xlEdgeTop);
            _drawer.BorderDraw(_ws, CurrentRow, 7, CurrentRow, 7, XlLineStyle.xlDot, XlBorderWeight.xlThin, XlBordersIndex.xlEdgeTop);
            _drawer.BorderDraw(_ws, CurrentRow, 8, CurrentRow, 9, XlLineStyle.xlDot, XlBorderWeight.xlThin, XlBordersIndex.xlEdgeTop);

            _writer.WriteDataToRange(_ws, "Име и Фамилия", false, 10, false, XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter, CurrentRow, 3, CurrentRow, 6, false, true);
            _writer.WriteDataToRange(_ws, "Дата", false, 10, false, XlHAlign.xlHAlignRight, XlVAlign.xlVAlignCenter, CurrentRow, 7, CurrentRow, 7, false, true);
            _writer.WriteDataToRange(_ws, "Подпис", false, 10, false, XlHAlign.xlHAlignRight, XlVAlign.xlVAlignCenter, CurrentRow, 8, CurrentRow, 8, false, true);

            AddRow(1);
            _writer.WriteDataToRange(_ws, $"Лист {Math.Ceiling(CurrentPage / 2.0)} | Стр. 2", false, 8, false, XlHAlign.xlHAlignRight, XlVAlign.xlVAlignCenter, CurrentRow, 8, CurrentRow, 9);
        }
    }
}
