using DelitaTrade.Core.Contracts;
using DelitaTrade.Core.Exporters.ExcelExporters.ExcelComponents;
using DelitaTrade.Core.Interfaces;
using DelitaTrade.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelitaTrade.Core.Exporters.ExcelExporters
{
    public class ExcelStupidReturnProtocolBuilder : ExcelReturnProtocolBuilder
    {
        protected const string _darkGrayColor = "#595959";
        protected const string _dataFontFamily = "Segoe Print";
        protected const string _bodyHeaderColor = "#99CCFF";
        protected const string _payMethodColor = "#DCE6F1";
        protected const string _xIconFilePath = "\\Components\\ComponentAssets\\Exporter\\xIcon.png";

        public ExcelStupidReturnProtocolBuilder(IConfiguration configuration) : base(configuration)
        {
        }

        public override IReturnProtocolBuilder BuildHeather()
        {
            BaseExcelComponent header = CreateHeader(CurrentRow);
            header.WriteContent(_ws);
            AddRow(header.RowsCount);
            return this;
        }

        public override IReturnProtocolBuilder BuildBody()
        {
            BuildBodyHeather();
            int productNumber = 1;
            foreach (var item in _returnProtocol.Products)
            {
                if (productNumber > 5)
                {
                    BuildFooter();
                    BuildHeather();
                    BuildBodyHeather();
                    productNumber = 1;
                }

                AddBodyListElement(item, productNumber);
                productNumber++;
            }
            if (productNumber <= 5) 
            {
                for (int i = productNumber; i <= 5; i++)
                {
                    AddBodyListElement(null, i);
                }
            }
            return this;
        }

        public override IReturnProtocolBuilder BuildFooter()
        {
            BaseExcelComponent footer = CreateFooter(CurrentRow);
            footer.WriteContent(_ws);
            AddRow(footer.RowsCount);

            BaseExcelComponent header = CreateHeader(CurrentRow);
            header.WriteContent(_ws);
            AddRow(header.RowsCount);

            BaseExcelComponent crisisPage = CreateCrisisPage(CurrentRow);
            crisisPage.WriteContent(_ws);
            AddRow(crisisPage.RowsCount);

            return this;
        }

        protected override void AddBodyListElement(IExportedReturnedProduct? product, int productNumber)
        {
            var bodyElement = CreateBodyComponent(CurrentRow, productNumber, product);
            bodyElement.WriteContent(_ws);
            AddRow(bodyElement.RowsCount);

            if(productNumber != 5)
            {
                BaseExcelComponent space = new BaseExcelComponent(CurrentRow, 1, 14, 7.5);
                ExcelElement el = new ExcelElement(1, 1, 1, 14);
                el.AddBorder(new(XlLineStyle.xlContinuous, XlBorderWeight.xlThin, XlBordersIndex.xlEdgeLeft));
                space.AddElement(el);
                space.WriteContent(_ws);
                AddRow(space.RowsCount);
            }
        }

        protected override void BuildBodyHeather()
        {
            BaseExcelComponent bodyHeader = CreateBodyHeaderComponent(CurrentRow);
            bodyHeader.WriteContent(_ws);
            AddRow(bodyHeader.RowsCount);

            BaseExcelComponent space = new BaseExcelComponent(CurrentRow, 1, 14, 1);
            space.WriteContent(_ws);
            AddRow(space.RowsCount);
        }

        private void CreateSheet()
        {
            BuildSheet(1);
        }


        protected override void BuildSheet(int sheet)
        {
            double[] columnSize = [3.48, 5.75, 4.66, 23.39, 4.66, 22.66, 4.66, 22.66, 4.66, 22.66, 4.66, 8.48, 4.66, 8.48];
            _ws = _wb.Worksheets[sheet];
            for (int i = 1; i <= columnSize.Length; i++)
            {
                _ws.Range[_ws.Cells[1, i], _ws.Cells[1, i]].ColumnWidth = columnSize[i - 1];
            }
        }

        private BaseExcelComponent CreateHeader(int row)
        {
            BaseExcelComponent header = new BaseExcelComponent(row, 1, 14, 35, 9, 33.5, 9);
            var el = new ExcelElement(1, 5, 1, 5);

            el.AddContent(new ExcelContent("ДПП", 26, XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter, true));
            el.AddBorder(new ExcelBorder(XlLineStyle.xlContinuous, XlBorderWeight.xlMedium, XlBordersIndex.xlEdgeBottom));
            el.AddBorder(new ExcelBorder(XlLineStyle.xlContinuous, XlBorderWeight.xlMedium, XlBordersIndex.xlEdgeLeft));
            el.AddBorder(new ExcelBorder(XlLineStyle.xlContinuous, XlBorderWeight.xlThin, XlBordersIndex.xlEdgeRight));
            header.AddElement(el);

            el = new ExcelElement(3, 5, 1, 5);
            el.AddContent(new ExcelContent("ПРОТОКОЛ РЕКЛАМАЦИИ", 24, XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter, true));
            header.AddElement(el);

            el = new ExcelElement(1, 10, 1, 1);
            el.AddContent(new ExcelContent($"ДОК {_docNumber}", 13, XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter, true));
            el.AddBorder(new ExcelBorder(XlLineStyle.xlContinuous, XlBorderWeight.xlMedium, XlBordersIndex.xlEdgeBottom));
            el.AddBorder(new ExcelBorder(XlLineStyle.xlContinuous, XlBorderWeight.xlThin, XlBordersIndex.xlEdgeRight));
            header.AddElement(el);

            el = new ExcelElement(1, 11, 1, 4);
            el.AddContent(new ExcelContent($"Версия {_version}{Environment.NewLine}Дата:{_versionDate}", 13, XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter, true, false, true));
            el.AddBorder(new ExcelBorder(XlLineStyle.xlContinuous, XlBorderWeight.xlMedium, XlBordersIndex.xlEdgeBottom));
            el.AddBorder(new ExcelBorder(XlLineStyle.xlContinuous, XlBorderWeight.xlMedium, XlBordersIndex.xlEdgeRight));
            header.AddElement(el);

            el = new ExcelElement(3, 10, 1, 1);
            el.AddContent(new ExcelContent($"ДАТА:", 15, XlHAlign.xlHAlignRight, XlVAlign.xlVAlignCenter, true));
            header.AddElement(el);

            el = new ExcelElement(3, 11, 1, 4);
            el.AddContent(new ExcelContent(_returnProtocol.ReturnDate, 11, XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter, true, false, true, true, _dataFontFamily, _darkGrayColor));
            el.AddBorder(new ExcelBorder());
            header.AddElement(el);

            el = new ExcelElement(1, 1, 4, 14);
            el.AddBorderAround(new ExcelAroundBorder(XlLineStyle.xlContinuous, XlBorderWeight.xlMedium));
            header.AddElement(el);

            el = new ExcelElement(1, 1, 4, 4);
            el.AddBorderAround(new ExcelAroundBorder(XlLineStyle.xlContinuous, XlBorderWeight.xlMedium));
            el.AddPicture(new ExcelPicture(156, 74, new FileInfo($"../../../{_logoFilePath}").FullName, 24, 5));
            header.AddElement(el);

            return header;
        }        

        private BaseExcelComponent CreateBodyHeaderComponent(int row)
        {
            BaseExcelComponent bodyHeader = new BaseExcelComponent(row, 1, 14, 38, 38, 38, 38, 12, 55.5, 13.5, 51);
            bodyHeader = AddProtocolInformation(bodyHeader);
            var el = new ExcelElement(8, 1, 1, 14, "BodyHeader", _bodyHeaderColor);
            el.AddBorder(new ExcelBorder(XlLineStyle.xlContinuous, XlBorderWeight.xlMedium, XlBordersIndex.xlEdgeLeft));
            el.AddBorder(new ExcelBorder(XlLineStyle.xlContinuous, XlBorderWeight.xlMedium, XlBordersIndex.xlEdgeTop));
            el.AddBorder(new ExcelBorder(XlLineStyle.xlContinuous, XlBorderWeight.xlMedium, XlBordersIndex.xlEdgeRight));
            el.AddBorder(new ExcelBorder(XlLineStyle.xlContinuous, XlBorderWeight.xlThin, XlBordersIndex.xlEdgeBottom));
            bodyHeader.AddElement(el);

            el = new ExcelElement(8, 1, 1, 1);
            el.AddContent(new ExcelContent("N:", 12, XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter, true));
            el.AddBorder(new ExcelBorder(XlLineStyle.xlContinuous, XlBorderWeight.xlThin, XlBordersIndex.xlEdgeRight));
            bodyHeader.AddElement(el);

            el = new ExcelElement(8, 2, 1, 4);
            el.AddContent(new ExcelContent("НАИМЕНОВАНИЕ АРТИКУЛ", 12, XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter, true));
            el.AddBorder(new ExcelBorder(XlLineStyle.xlContinuous, XlBorderWeight.xlThin, XlBordersIndex.xlEdgeRight));
            bodyHeader.AddElement(el);

            el = new ExcelElement(8, 6, 1, 1);
            el.AddContent(new ExcelContent("ВЪРНАТО КОЛИЧЕСТВО", 12, XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter, true, false, true));
            el.AddBorder(new ExcelBorder(XlLineStyle.xlContinuous, XlBorderWeight.xlThin, XlBordersIndex.xlEdgeRight));
            bodyHeader.AddElement(el);

            el = new ExcelElement(8, 7, 1, 2);
            el.AddContent(new ExcelContent("ПАРТИДА НОМЕР", 12, XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter, true));
            el.AddBorder(new ExcelBorder(XlLineStyle.xlContinuous, XlBorderWeight.xlThin, XlBordersIndex.xlEdgeRight));
            bodyHeader.AddElement(el);

            el = new ExcelElement(8, 9, 1, 2);
            el.AddContent(new ExcelContent("СРОК НА ГОДНОСТ", 12, XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter, true));
            el.AddBorder(new ExcelBorder(XlLineStyle.xlContinuous, XlBorderWeight.xlMedium, XlBordersIndex.xlEdgeRight));
            bodyHeader.AddElement(el);

            el = new ExcelElement(8, 11, 1, 4);
            el.AddContent(new ExcelContent("БЕЛЕЖКИ СКАД", 12, XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter, true));
            bodyHeader.AddElement(el);

            return bodyHeader;
        }

        private BaseExcelComponent CreateBodyComponent(int row, int productNumber, IExportedReturnedProduct? product)
        {
            BaseExcelComponent bodyElement = new BaseExcelComponent(row, 1, 14, 36.5, 7.5, 21.5, 21.5, 24.5);
            var el = new ExcelElement(1, 1, 5, 14, "Main");
            if (productNumber == 1)
            {
                el.AddBorder(new ExcelBorder(XlLineStyle.xlContinuous, XlBorderWeight.xlThin, XlBordersIndex.xlEdgeTop));
            }
            else 
            {
                el.AddBorder(new ExcelBorder(XlLineStyle.xlContinuous, XlBorderWeight.xlMedium, XlBordersIndex.xlEdgeTop));
            }
            el.AddBorder(new ExcelBorder(XlLineStyle.xlContinuous, XlBorderWeight.xlMedium, XlBordersIndex.xlEdgeLeft));
            el.AddBorder(new ExcelBorder(XlLineStyle.xlContinuous, XlBorderWeight.xlMedium, XlBordersIndex.xlEdgeRight));
            el.AddBorder(new ExcelBorder(XlLineStyle.xlContinuous, XlBorderWeight.xlMedium, XlBordersIndex.xlEdgeBottom));
            bodyElement.AddElement(el);

            el = new ExcelElement(1, 1, 5, 1);
            el.AddContent(new ExcelContent(productNumber.ToString(), 14, XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter, true));
            el.AddBorder(new ExcelBorder(XlLineStyle.xlContinuous, XlBorderWeight.xlMedium, XlBordersIndex.xlEdgeRight));
            bodyElement.AddElement(el);

            el = new ExcelElement(3, 2, 3, 1);
            el.AddContent(new ExcelContent($"ОСНОВАНИЕ{Environment.NewLine}ЗА ВРЪЩАНЕ", 8, XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter, true, false, true, false, "Arial", default, XlOrientation.xlUpward));
            bodyElement.AddElement(el);
            
            el = new ExcelElement(1, 11, 1, 1);
            el.AddBorder(new ExcelBorder(XlLineStyle.xlContinuous, XlBorderWeight.xlMedium, XlBordersIndex.xlEdgeRight)); 
            bodyElement.AddElement(el);

            el = new ExcelElement(1, 12, 1, 1);
            el.AddContent(new ExcelContent($"ГОДНО{Environment.NewLine}ЗА ПРОД.", 9, XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter, true, false, true, false));
            el.AddBorder(new ExcelBorder(XlLineStyle.xlContinuous, XlBorderWeight.xlMedium, XlBordersIndex.xlEdgeRight));
            bodyElement.AddElement(el);

            el = new ExcelElement(1, 13, 1, 1);
            el.AddBorder(new ExcelBorder(XlLineStyle.xlContinuous, XlBorderWeight.xlMedium, XlBordersIndex.xlEdgeRight));
            bodyElement.AddElement(el);

            el = new ExcelElement(1, 14, 1, 1);
            el.AddContent(new ExcelContent("БРАК", 10, XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter, true, false, true, false));
            bodyElement.AddElement(el);

            el = new ExcelElement(2, 2, 1, 13);
            el.AddBorderAround(new ExcelAroundBorder(XlLineStyle.xlContinuous, XlBorderWeight.xlMedium));
            bodyElement.AddElement(el);

            int startColumn = 3;
            int startRow = 3;
            foreach (var category in _descriptionCategoryViewModels)
            {
                int elWidth = 1;
                int elHeight = 1;
                el = new ExcelElement(startRow, startColumn, 1, 1);
                el.AddBorderAround(new ExcelAroundBorder(XlLineStyle.xlContinuous, XlBorderWeight.xlMedium));
                if(product != null && product.DescriptionCategory == category.Name)
                {
                    el.AddContent(new ExcelContent("✖", 14, XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignTop, true, false, false, false, _dataFontFamily, _darkGrayColor));
                }
                bodyElement.AddElement(el);

                if (startColumn == 11)
                {
                    elWidth = 3;
                }

                el = new ExcelElement(startRow, startColumn + 1, elHeight, elWidth);
                el.AddContent(new ExcelContent(category.Name.ToUpper(), 10, XlHAlign.xlHAlignLeft, XlVAlign.xlVAlignCenter, true));
                el.AddBorder(new ExcelBorder(XlLineStyle.xlContinuous, XlBorderWeight.xlThin, XlBordersIndex.xlEdgeBottom));
                bodyElement.AddElement(el);
                if(startColumn + 1 + elWidth > 14)
                {
                    startColumn = 1;
                    startRow++;
                } 
                
                startColumn += 2;
            }

            el = new ExcelElement(5, 3, 1, 1);
            el.AddBorderAround(new ExcelAroundBorder(XlLineStyle.xlContinuous, XlBorderWeight.xlMedium));
            if (product != null && string.IsNullOrEmpty(product.Description) == false)
            {
                el.AddContent(new ExcelContent("✖", 14, XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignTop, true, false, false, false, _dataFontFamily, _darkGrayColor));
            }
            bodyElement.AddElement(el);

            el = new ExcelElement(5, 4, 1, 3);
            el.AddContent(new ExcelContent("ДРУГО - ПОДРОБНО ОПИСАНИЕ:", 10, XlHAlign.xlHAlignLeft, XlVAlign.xlVAlignCenter, true));
            bodyElement.AddElement(el);
            if (product == null)
            {
                el = new ExcelElement(1, 2, 1, 4, "Name");
                el.AddBorder(new ExcelBorder(XlLineStyle.xlContinuous, XlBorderWeight.xlThin, XlBordersIndex.xlEdgeRight));
                bodyElement.AddElement(el);

                el = new ExcelElement(1, 6, 1, 1, "Quantity");
                el.AddBorder(new ExcelBorder(XlLineStyle.xlContinuous, XlBorderWeight.xlThin, XlBordersIndex.xlEdgeRight));
                bodyElement.AddElement(el);

                el = new ExcelElement(1, 7, 1, 2, "Batch");
                el.AddBorder(new ExcelBorder(XlLineStyle.xlContinuous, XlBorderWeight.xlThin, XlBordersIndex.xlEdgeRight));
                bodyElement.AddElement(el);

                el = new ExcelElement(1, 9, 1, 2, "BestBefore");
                el.AddBorder(new ExcelBorder(XlLineStyle.xlContinuous, XlBorderWeight.xlMedium, XlBordersIndex.xlEdgeRight));
                bodyElement.AddElement(el);

                var mainElement = bodyElement.Elements.First(e => e.Name == "Main");
                mainElement.AddPicture(new ExcelPicture(824, 109, new FileInfo($"../../../{_xIconFilePath}").FullName, 10, 1));
            }
            else
            {
                el = new ExcelElement(1, 2, 1, 4, "Name");
                el.AddContent(new ExcelContent(product.Name, 9, XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter, true, false, true, false, _dataFontFamily, _darkGrayColor));
                el.AddBorder(new ExcelBorder(XlLineStyle.xlContinuous, XlBorderWeight.xlThin, XlBordersIndex.xlEdgeRight));
                bodyElement.AddElement(el);

                el = new ExcelElement(1, 6, 1, 1, "Quantity");
                el.AddContent(new ExcelContent($"{product.Quantity} {product.Unit}", 11, XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter, true, false, true, false, _dataFontFamily, _darkGrayColor));
                el.AddBorder(new ExcelBorder(XlLineStyle.xlContinuous, XlBorderWeight.xlThin, XlBordersIndex.xlEdgeRight));
                bodyElement.AddElement(el);

                el = new ExcelElement(1, 7, 1, 2, "Batch");
                el.AddContent(new ExcelContent(char.ToUpper(product.Batch[0]) == 'L' ? product.Batch : $"L:{product.Batch}", 
                   11, XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter, true, false, true, false, _dataFontFamily, _darkGrayColor));
                el.AddBorder(new ExcelBorder(XlLineStyle.xlContinuous, XlBorderWeight.xlThin, XlBordersIndex.xlEdgeRight));
                bodyElement.AddElement(el);

                el = new ExcelElement(1, 9, 1, 2, "BestBefore");
                el.AddContent(new ExcelContent(product.BestBefore, 11, XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter, true, false, true, false, _dataFontFamily, _darkGrayColor));
                el.AddBorder(new ExcelBorder(XlLineStyle.xlContinuous, XlBorderWeight.xlMedium, XlBordersIndex.xlEdgeRight));
                bodyElement.AddElement(el);

                el = new ExcelElement(5, 7, 1, 8);
                el.AddContent(new ExcelContent(product.Description, 10, XlHAlign.xlHAlignLeft, XlVAlign.xlVAlignCenter, true, false, true, false, _dataFontFamily, _darkGrayColor));
                bodyElement.AddElement(el);
            }              

            return bodyElement;
        }

        private BaseExcelComponent CreateFooter(int row)
        {
            BaseExcelComponent footer = new BaseExcelComponent(row, 1, 14, 18, 25.5, 18, 25.5, 18, 25.5, 18, 24.5, 18, 15.5, 15.5, 13, 15.5, 13, 13.5);

            //first row
            ExcelElement el = new ExcelElement(2, 3, 1, 3);
            el.AddContent(new ExcelContent("Приел СКЛАД:", 14, XlHAlign.xlHAlignRight, XlVAlign.xlVAlignBottom, true));  
            footer.AddElement(el);

            el = new ExcelElement(2, 6, 1, 3);
            el.AddBorder(new ExcelBorder());
            footer.AddElement(el);

            el = new ExcelElement(2, 10, 1, 1);
            el.AddContent(new ExcelContent("подпис:", 14, XlHAlign.xlHAlignRight, XlVAlign.xlVAlignBottom, true, true));
            footer.AddElement(el);

            el = new ExcelElement(2, 11, 1, 4);
            el.AddBorder(new ExcelBorder());
            footer.AddElement(el);

            //second row
            el = new ExcelElement(4, 1, 1, 5);
            el.AddContent(new ExcelContent("Съгласувано (Драгов / Гаджев):", 14, XlHAlign.xlHAlignRight, XlVAlign.xlVAlignBottom, true));
            footer.AddElement(el);

            el = new ExcelElement(4, 6, 1, 3);
            el.AddBorder(new ExcelBorder());
            footer.AddElement(el);

            el = new ExcelElement(4, 10, 1, 1);
            el.AddContent(new ExcelContent("подпис:", 14, XlHAlign.xlHAlignRight, XlVAlign.xlVAlignBottom, true, true));
            footer.AddElement(el);

            el = new ExcelElement(4, 11, 1, 4);
            el.AddBorder(new ExcelBorder());
            footer.AddElement(el);

            //third row
            el = new ExcelElement(6, 1, 1, 5);
            el.AddContent(new ExcelContent("Издал КРЕДИТНО ИЗВЕСТИЕ:", 14, XlHAlign.xlHAlignRight, XlVAlign.xlVAlignBottom, true));
            footer.AddElement(el);

            el = new ExcelElement(6, 6, 1, 3);
            el.AddBorder(new ExcelBorder());
            footer.AddElement(el);

            el = new ExcelElement(6, 10, 1, 1);
            el.AddContent(new ExcelContent("подпис:", 14, XlHAlign.xlHAlignRight, XlVAlign.xlVAlignBottom, true, true));
            footer.AddElement(el);

            el = new ExcelElement(6, 11, 1, 4);
            el.AddBorder(new ExcelBorder());
            footer.AddElement(el);


            //description
            el = new ExcelElement(8, 3, 1, 1);
            el.AddContent(new ExcelContent("NB!", 12, XlHAlign.xlHAlignRight, XlVAlign.xlVAlignBottom, true));
            footer.AddElement(el);

            el = new ExcelElement(8, 10, 1, 1);
            el.AddContent(new ExcelContent("ДАТА:", 14, XlHAlign.xlHAlignRight, XlVAlign.xlVAlignBottom, true));
            footer.AddElement(el);

            el = new ExcelElement(8, 11, 1, 4);
            el.AddBorder(new ExcelBorder());
            footer.AddElement(el);

            el = new ExcelElement(9, 3, 1, 7);
            el.AddContent(new ExcelContent("Кредитно Известие за върната стока се издава в деня на получаването и в склада!", 12, XlHAlign.xlHAlignLeft, XlVAlign.xlVAlignCenter, true));
            footer.AddElement(el);

            el = new ExcelElement(10, 3, 1, 12);
            el.AddContent(new ExcelContent("Кредитното Известие се предоставя на клиента в деня след издаването му (в случай че има заявка) или най-късно 2 дни",
                12, XlHAlign.xlHAlignLeft, XlVAlign.xlVAlignCenter, true));
            footer.AddElement(el);

            el = new ExcelElement(11, 3, 1, 7);
            el.AddContent(new ExcelContent("след издаването му, чрез нарочно посещение от страна на търговски представител!", 12, XlHAlign.xlHAlignLeft, XlVAlign.xlVAlignCenter, true));
            footer.AddElement(el);

            el = new ExcelElement(13, 2, 1, 11);
            el.AddContent(new ExcelContent("!!! ВАЖНО - В СЛУЧАЙ НА КРИЗИСНА СИТУАЦИЯ, МОЛЯ ОБЪРНЕТЕ И ПОПЪЛНЕТЕ ИНФОРМАЦИЯТА НА СТР. 2 !!!",
                12, XlHAlign.xlHAlignLeft, XlVAlign.xlVAlignCenter, true, false, false, false, "Ariel", "#FF0000"));
            footer.AddElement(el);

            el = new ExcelElement(13, 14, 1, 1);
            el.AddContent(new ExcelContent("стр. 1", 10, XlHAlign.xlHAlignRight, XlVAlign.xlVAlignBottom, true));
            footer.AddElement(el);

            return footer;
        }

        private BaseExcelComponent AddProtocolInformation(BaseExcelComponent bodyHeaderComponent)
        {
            //first row
            ExcelElement el = new ExcelElement(1, 1, 1, 4);
            el.AddContent(new ExcelContent("Обект: ", 18, XlHAlign.xlHAlignRight, XlVAlign.xlVAlignCenter, true, false, true));
            bodyHeaderComponent.AddElement(el);

            el = new ExcelElement(1, 5, 1, 10);
            el.AddContent(new ExcelContent(_returnProtocol.CompanyObject, 14, XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignBottom, true, false, true, false, _dataFontFamily, _darkGrayColor));
            el.AddBorder(new ExcelBorder());
            bodyHeaderComponent.AddElement(el);

            //second row
            el = new ExcelElement(2, 1, 1, 4);
            el.AddContent(new ExcelContent("Адрес:", 18, XlHAlign.xlHAlignRight, XlVAlign.xlVAlignCenter, true, false, true));
            bodyHeaderComponent.AddElement(el);

            el = new ExcelElement(2, 5, 1, 10);
            el.AddContent(new ExcelContent(_returnProtocol.Address, 14, XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignBottom, true, false, true, false, _dataFontFamily, _darkGrayColor));
            el.AddBorder(new ExcelBorder());
            bodyHeaderComponent.AddElement(el);

            //third row
            el = new ExcelElement(3, 1, 1, 4);
            el.AddContent(new ExcelContent($"Върнал стоката:                         (шофьор)", 14, XlHAlign.xlHAlignRight, XlVAlign.xlVAlignCenter, true, false, true));
            bodyHeaderComponent.AddElement(el);

            el = new ExcelElement(3, 5, 1, 5);
            el.AddContent(new ExcelContent(_returnProtocol.UserName, 14, XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignBottom, true, false, true, false, _dataFontFamily, _darkGrayColor));
            el.AddBorder(new ExcelBorder());
            bodyHeaderComponent.AddElement(el);

            el = new ExcelElement(3, 10, 1, 1);
            el.AddContent(new ExcelContent("Подпис:", 14, XlHAlign.xlHAlignRight, XlVAlign.xlVAlignBottom, true, true));
            bodyHeaderComponent.AddElement(el);

            el = new ExcelElement(3, 11, 1, 4);
            el.AddBorder(new ExcelBorder());
            bodyHeaderComponent.AddElement(el);

            //four row
            el = new ExcelElement(4, 1, 1, 4);
            el.AddContent(new ExcelContent($"Одобрил протокола: (търговски представител)", 14, XlHAlign.xlHAlignRight, XlVAlign.xlVAlignCenter, true, false, true));
            bodyHeaderComponent.AddElement(el);

            el = new ExcelElement(4, 5, 1, 5);
            el.AddContent(new ExcelContent(_returnProtocol.TraderName, 14, XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignBottom, true, false, true, false, _dataFontFamily, _darkGrayColor));
            el.AddBorder(new ExcelBorder());
            bodyHeaderComponent.AddElement(el);

            el = new ExcelElement(4, 10, 1, 1);
            el.AddContent(new ExcelContent("Подпис:", 14, XlHAlign.xlHAlignRight, XlVAlign.xlVAlignBottom, true, true));
            bodyHeaderComponent.AddElement(el);

            el = new ExcelElement(4, 11, 1, 4);
            el.AddBorder(new ExcelBorder());
            bodyHeaderComponent.AddElement(el);

            //pay method row

            bodyHeaderComponent = AddPayMethodComponent(bodyHeaderComponent);

            return bodyHeaderComponent;
        }

        protected virtual BaseExcelComponent AddPayMethodComponent(BaseExcelComponent bodyHeaderComponent)
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

            el = new ExcelElement(6, 8, 1, 3);
            el.AddContent(new ExcelContent("ПРИСПАДНАТО  (В БРОЙ)", 11, XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter, true));
            el.AddBorder(new ExcelBorder(XlLineStyle.xlContinuous, XlBorderWeight.xlMedium, XlBordersIndex.xlEdgeRight));
            bodyHeaderComponent.AddElement(el);

            el = new ExcelElement(6, 11, 1, 1);
            el.AddBorder(new ExcelBorder(XlLineStyle.xlContinuous, XlBorderWeight.xlMedium, XlBordersIndex.xlEdgeRight));
            bodyHeaderComponent.AddElement(el);

            el = new ExcelElement(6, 12, 1, 3);
            el.AddContent(new ExcelContent($"НЕ Е ПРИСПАДНАТО{Environment.NewLine}(В БРОЙ)", 10, XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter, true, false, true));
            el.AddBorder(new ExcelBorder(XlLineStyle.xlContinuous, XlBorderWeight.xlMedium, XlBordersIndex.xlEdgeRight));
            bodyHeaderComponent.AddElement(el);

            el = new ExcelElement(6, ExportPayMethodCategories.PayMethods[_returnProtocol.PayMethod], 1, 1);
            el.AddContent(new ExcelContent("✖", 26, XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter, true, false, false, false, _dataFontFamily, _darkGrayColor));
            bodyHeaderComponent.AddElement(el);

            return bodyHeaderComponent;
        }

        private BaseExcelComponent CreateCrisisPage(int row)
        {
            BaseExcelComponent crisisPage = new BaseExcelComponent(row, 1, 14,
                11, 40, 14, 40, 40, 40, 40, 14, 40, 18.5, 15.5, 40, 40, 40, 40, 14, 40, 18.5, 40, 18.5, 14, 40, 40, 40, 40, 14, 40, 18.5, 14, 40, 40, 40, 40, 14, 40, 15, 15, 15, 15);

            ExcelElement el = new ExcelElement(2, 1, 1, 14);
            el.AddContent(new ExcelContent("ПРЕНАСОЧВАНЕ НА РЕКЛАМАЦИЯ КЪМ КРИЗИСЕН ЕКИП", 20, XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter, true, true));
            crisisPage.AddElement(el);

            //first component
            el = new ExcelElement(4, 1, 4, 14);
            el.AddBorderAround(new ExcelAroundBorder(XlLineStyle.xlContinuous, XlBorderWeight.xlMedium));
            crisisPage.AddElement(el);

            el = new ExcelElement(4, 1, 1, 4);
            el.AddBorder(new ExcelBorder(XlLineStyle.xlContinuous, XlBorderWeight.xlThin, XlBordersIndex.xlEdgeBottom));
            el.AddBorder(new ExcelBorder(XlLineStyle.xlContinuous, XlBorderWeight.xlMedium, XlBordersIndex.xlEdgeRight));
            el.AddContent(new ExcelContent("ЗА ДОПЪЛНИТЕЛНА ИНФОРМАЦИЯ ОТ:", 14, XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter, true, true, true));
            crisisPage.AddElement(el);

            el = new ExcelElement(4, 5, 1, 10);
            el.AddBorder(new ExcelBorder());
            crisisPage.AddElement(el);

            //
            el = new ExcelElement(5, 1, 1, 2);
            el.AddBorder(new ExcelBorder());
            crisisPage.AddElement(el);

            el = new ExcelElement(5, 3, 1, 2);
            el.AddBorder(new ExcelBorder(XlLineStyle.xlContinuous, XlBorderWeight.xlThin, XlBordersIndex.xlEdgeLeft));
            el.AddBorder(new ExcelBorder(XlLineStyle.xlContinuous, XlBorderWeight.xlThin, XlBordersIndex.xlEdgeBottom));
            el.AddBorder(new ExcelBorder(XlLineStyle.xlContinuous, XlBorderWeight.xlMedium, XlBordersIndex.xlEdgeRight));
            el.AddContent(new ExcelContent("ЛОГИСТИКА", 14, XlHAlign.xlHAlignLeft, XlVAlign.xlVAlignCenter, true, true));
            crisisPage.AddElement(el);

            el = new ExcelElement(5, 5, 1, 10);
            el.AddBorder(new ExcelBorder());
            crisisPage.AddElement(el);

            //
            el = new ExcelElement(6, 1, 1, 2);
            el.AddBorder(new ExcelBorder());
            crisisPage.AddElement(el);

            el = new ExcelElement(6, 3, 1, 2);
            el.AddBorder(new ExcelBorder(XlLineStyle.xlContinuous, XlBorderWeight.xlThin, XlBordersIndex.xlEdgeLeft));
            el.AddBorder(new ExcelBorder(XlLineStyle.xlContinuous, XlBorderWeight.xlThin, XlBordersIndex.xlEdgeBottom));
            el.AddBorder(new ExcelBorder(XlLineStyle.xlContinuous, XlBorderWeight.xlMedium, XlBordersIndex.xlEdgeRight));
            el.AddContent(new ExcelContent("СКЛАД", 14, XlHAlign.xlHAlignLeft, XlVAlign.xlVAlignCenter, true, true));
            crisisPage.AddElement(el);

            el = new ExcelElement(6, 5, 1, 10);
            el.AddBorder(new ExcelBorder());
            crisisPage.AddElement(el);

            //
            el = new ExcelElement(7, 3, 1, 2);
            el.AddBorder(new ExcelBorder(XlLineStyle.xlContinuous, XlBorderWeight.xlThin, XlBordersIndex.xlEdgeLeft));
            el.AddBorder(new ExcelBorder(XlLineStyle.xlContinuous, XlBorderWeight.xlMedium, XlBordersIndex.xlEdgeRight));
            el.AddContent(new ExcelContent("ПРОИЗВОДСТВО", 14, XlHAlign.xlHAlignLeft, XlVAlign.xlVAlignCenter, true, true));
            crisisPage.AddElement(el);

            //
            el = new ExcelElement(9, 1, 1, 4);
            el.AddContent(new ExcelContent("ПРЕДАДЕНА ИНФ. ОТ:", 14, XlHAlign.xlHAlignRight, XlVAlign.xlVAlignCenter, true));
            crisisPage.AddElement(el);

            ////
            el = new ExcelElement(9, 6, 1, 1);
            el.AddBorder(new ExcelBorder());
            crisisPage.AddElement(el);

            el = new ExcelElement(10, 6, 1, 1);
            el.AddContent(new ExcelContent("Име и Фамилия", 10, XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter, true, true));
            crisisPage.AddElement(el);

            ////
            el = new ExcelElement(9, 8, 1, 1);
            el.AddBorder(new ExcelBorder());
            crisisPage.AddElement(el);

            el = new ExcelElement(10, 8, 1, 1);
            el.AddContent(new ExcelContent("Длъжност", 10, XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter, true, true));
            crisisPage.AddElement(el);

            ////
            el = new ExcelElement(9, 10, 1, 1);
            el.AddBorder(new ExcelBorder());
            crisisPage.AddElement(el);

            el = new ExcelElement(10, 10, 1, 1);
            el.AddContent(new ExcelContent("Дата", 10, XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter, true, true));
            crisisPage.AddElement(el);

            ////
            el = new ExcelElement(9, 12, 1, 3);
            el.AddBorder(new ExcelBorder());
            crisisPage.AddElement(el);

            el = new ExcelElement(10, 12, 1, 3);
            el.AddContent(new ExcelContent("Подпис", 10, XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter, true, true));
            crisisPage.AddElement(el);

            //second component
            el = new ExcelElement(12, 1, 4, 14);
            el.AddBorderAround(new ExcelAroundBorder(XlLineStyle.xlContinuous, XlBorderWeight.xlMedium));
            crisisPage.AddElement(el);

            //
            el = new ExcelElement(12, 1, 4, 4);
            el.AddBorder(new ExcelBorder(XlLineStyle.xlContinuous, XlBorderWeight.xlThin, XlBordersIndex.xlEdgeRight));
            el.AddContent(new ExcelContent("Подробно описание на рекламация, която може да доведе или води до опасност за човешкото здраве:",
                14, XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter, true, true, true));
            crisisPage.AddElement(el);

            el = new ExcelElement(12, 5, 1, 10);
            el.AddBorder(new ExcelBorder());
            crisisPage.AddElement(el); 
            
            el = new ExcelElement(13, 5, 1, 10);
            el.AddBorder(new ExcelBorder());
            crisisPage.AddElement(el);

            el = new ExcelElement(14, 5, 1, 10);
            el.AddBorder(new ExcelBorder());
            crisisPage.AddElement(el);

            //
            el = new ExcelElement(17, 1, 1, 4);
            el.AddContent(new ExcelContent("УВЕДОМИЛ:", 14, XlHAlign.xlHAlignRight, XlVAlign.xlVAlignCenter, true));
            crisisPage.AddElement(el);

            ////
            el = new ExcelElement(17, 6, 1, 1);
            el.AddBorder(new ExcelBorder());
            crisisPage.AddElement(el);

            el = new ExcelElement(18, 6, 1, 1);
            el.AddContent(new ExcelContent("Име и Фамилия", 10, XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter, true, true));
            crisisPage.AddElement(el);

            ////
            el = new ExcelElement(17, 8, 1, 1);
            el.AddBorder(new ExcelBorder());
            crisisPage.AddElement(el);

            el = new ExcelElement(18, 8, 1, 1);
            el.AddContent(new ExcelContent("Длъжност", 10, XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter, true, true));
            crisisPage.AddElement(el);

            ////
            el = new ExcelElement(17, 10, 1, 1);
            el.AddBorder(new ExcelBorder());
            crisisPage.AddElement(el);

            el = new ExcelElement(18, 10, 1, 1);
            el.AddContent(new ExcelContent("Дата", 10, XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter, true, true));
            crisisPage.AddElement(el);

            ////
            el = new ExcelElement(17, 12, 1, 3);
            el.AddBorder(new ExcelBorder());
            crisisPage.AddElement(el);

            el = new ExcelElement(18, 12, 1, 3);
            el.AddContent(new ExcelContent("Подпис", 10, XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter, true, true));
            crisisPage.AddElement(el);

            //
            el = new ExcelElement(19, 1, 1, 4);
            el.AddContent(new ExcelContent($"ПРИЕЛ{Environment.NewLine}(Отговорник Кризисен Екип):", 13, XlHAlign.xlHAlignRight, XlVAlign.xlVAlignCenter, true, false,true));
            crisisPage.AddElement(el);

            ////
            el = new ExcelElement(19, 6, 1, 3);
            el.AddBorder(new ExcelBorder());
            crisisPage.AddElement(el);

            el = new ExcelElement(20, 6, 1, 3);
            el.AddContent(new ExcelContent("Име и Фамилия", 10, XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter, true, true));
            crisisPage.AddElement(el);

            ////
            el = new ExcelElement(19, 10, 1, 1);
            el.AddBorder(new ExcelBorder());
            crisisPage.AddElement(el);

            el = new ExcelElement(20, 10, 1, 1);
            el.AddContent(new ExcelContent("Дата", 10, XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter, true, true));
            crisisPage.AddElement(el);

            ////
            el = new ExcelElement(19, 12, 1, 3);
            el.AddBorder(new ExcelBorder());
            crisisPage.AddElement(el);

            el = new ExcelElement(20, 12, 1, 3);
            el.AddContent(new ExcelContent("Подпис", 10, XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter, true, true));
            crisisPage.AddElement(el);


            //third component
            el = new ExcelElement(22, 1, 4, 14);
            el.AddBorderAround(new ExcelAroundBorder(XlLineStyle.xlContinuous, XlBorderWeight.xlMedium));
            crisisPage.AddElement(el);

            //
            el = new ExcelElement(22, 1, 4, 4);
            el.AddBorder(new ExcelBorder(XlLineStyle.xlContinuous, XlBorderWeight.xlThin, XlBordersIndex.xlEdgeRight));
            el.AddContent(new ExcelContent("ПРЕДПРИЕТИ КОРЕКТИВНИ ДЕЙСТВИЯ:",
                14, XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter, true, true, true));
            crisisPage.AddElement(el);

            el = new ExcelElement(22, 5, 1, 10);
            el.AddBorder(new ExcelBorder());
            crisisPage.AddElement(el);

            el = new ExcelElement(23, 5, 1, 10);
            el.AddBorder(new ExcelBorder());
            crisisPage.AddElement(el);

            el = new ExcelElement(24, 5, 1, 10);
            el.AddBorder(new ExcelBorder());
            crisisPage.AddElement(el);

            //
            el = new ExcelElement(27, 1, 1, 4);
            el.AddContent(new ExcelContent($"ОТГОВОРНИК КРИЗИСЕН ЕКИП:", 12, XlHAlign.xlHAlignRight, XlVAlign.xlVAlignCenter, true, false, false));
            crisisPage.AddElement(el);

            ////
            el = new ExcelElement(27, 6, 1, 3);
            el.AddBorder(new ExcelBorder());
            crisisPage.AddElement(el);

            el = new ExcelElement(28, 6, 1, 3);
            el.AddContent(new ExcelContent("Име и Фамилия", 10, XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter, true, true));
            crisisPage.AddElement(el);

            ////
            el = new ExcelElement(27, 10, 1, 1);
            el.AddBorder(new ExcelBorder());
            crisisPage.AddElement(el);

            el = new ExcelElement(28, 10, 1, 1);
            el.AddContent(new ExcelContent("Дата", 10, XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter, true, true));
            crisisPage.AddElement(el);

            ////
            el = new ExcelElement(27, 12, 1, 3);
            el.AddBorder(new ExcelBorder());
            crisisPage.AddElement(el);

            el = new ExcelElement(28, 12, 1, 3);
            el.AddContent(new ExcelContent("Подпис", 10, XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter, true, true));
            crisisPage.AddElement(el);


            //fourth component
            el = new ExcelElement(30, 1, 4, 14);
            el.AddBorderAround(new ExcelAroundBorder(XlLineStyle.xlContinuous, XlBorderWeight.xlMedium));
            crisisPage.AddElement(el);

            //
            el = new ExcelElement(30, 1, 4, 4);
            el.AddBorder(new ExcelBorder(XlLineStyle.xlContinuous, XlBorderWeight.xlThin, XlBordersIndex.xlEdgeRight));
            el.AddContent(new ExcelContent("РЕЗУЛТАТ ОТ КОРЕКТИВНИ ДЕЙСТВИЯ:",
                14, XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter, true, true, true));
            crisisPage.AddElement(el);

            el = new ExcelElement(30, 5, 1, 10);
            el.AddBorder(new ExcelBorder());
            crisisPage.AddElement(el);

            el = new ExcelElement(31, 5, 1, 10);
            el.AddBorder(new ExcelBorder());
            crisisPage.AddElement(el);

            el = new ExcelElement(32, 5, 1, 10);
            el.AddBorder(new ExcelBorder());
            crisisPage.AddElement(el);

            //
            el = new ExcelElement(35, 1, 1, 4);
            el.AddContent(new ExcelContent($"ОТГОВОРНИК КРИЗИСЕН ЕКИП:", 12, XlHAlign.xlHAlignRight, XlVAlign.xlVAlignCenter, true, false, false));
            crisisPage.AddElement(el);

            ////
            el = new ExcelElement(35, 6, 1, 3);
            el.AddBorder(new ExcelBorder());
            crisisPage.AddElement(el);

            el = new ExcelElement(36, 6, 1, 3);
            el.AddContent(new ExcelContent("Име и Фамилия", 10, XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter, true, true));
            crisisPage.AddElement(el);

            ////
            el = new ExcelElement(35, 10, 1, 1);
            el.AddBorder(new ExcelBorder());
            crisisPage.AddElement(el);

            el = new ExcelElement(36, 10, 1, 1);
            el.AddContent(new ExcelContent("Дата", 10, XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter, true, true));
            crisisPage.AddElement(el);

            ////
            el = new ExcelElement(35, 12, 1, 3);
            el.AddBorder(new ExcelBorder());
            crisisPage.AddElement(el);

            el = new ExcelElement(36, 12, 1, 3);
            el.AddContent(new ExcelContent("Подпис", 10, XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter, true, true));
            crisisPage.AddElement(el);

            el = new ExcelElement(39, 14, 1, 1);
            el.AddContent(new ExcelContent("стр. 2", 10, XlHAlign.xlHAlignRight, XlVAlign.xlVAlignBottom, true));
            crisisPage.AddElement(el);

            return crisisPage;
        }
    }
}
