using DelitaTrade.Core.ConfigurationModels;
using DelitaTrade.Core.Interfaces;
using DelitaTrade.Core.ViewModels;
using DelitaTrade.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Configuration;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DelitaTrade.Core.Exporters.ExcelExporters
{
    public class ExcelReturnProtocolBuilder : IReturnProtocolBuilder
    {
        protected const string _logoFilePath = "\\Components\\ComponentAssets\\delitaTradeLogo.png";
        private const string lightGray = "#EDEDED";
        private const string gray = "#A6A6A6";
        private const int _rowCountOnProtocolPage = 50;

        protected readonly IConfiguration _configuration;

        protected _Application _excel;
        protected Workbook _wb;
        protected Worksheet _ws;

        protected ExcelWriter _writer;
        protected ExcelDrawer _drawer;

        protected IExportReturnProtocol _returnProtocol;

        protected List<ExcelPageModel> _pages = new List<ExcelPageModel>();
        protected IEnumerable<DescriptionCategoryViewModel> _descriptionCategoryViewModels;
        private int _currentRow = 1;

        private string _inputPath;
        protected readonly string _version;
        protected readonly string _versionDate;
        protected readonly string _docNumber;
        private string _path;
        private string _excelPath;

        public ExcelReturnProtocolBuilder(IConfiguration configuration)
        {
            _writer = new ExcelWriter();
            _drawer = new ExcelDrawer();
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            var section = configuration.GetSection("ReturnProtocolConstants");
            string docNumber = section.GetValue(typeof(string), "docNumber") as string ?? "File";
            string version = section.GetValue(typeof(string), "version") as string ?? "not";
            string versionDate = section.GetValue(typeof(string), "versionDate") as string ?? "found";
                        
            _version = version;
            _versionDate = versionDate;
            _docNumber = docNumber;
        }
        public string FilePath
        {
            get => _path;
            set
            {
                _path = value;
            }
        }

        protected int CurrentPage => _pages.Count;

        protected int CurrentRow => _currentRow;

        public string ExportedFilePath => _excelPath;

        public virtual IReturnProtocolBuilder BuildBody()
        {
            BuildBodyHeather();
            int productNumber = 1;
            foreach (var item in _returnProtocol.Products)
            {
                AddBodyListElement(item, productNumber);
                productNumber++;
            }
            return this;
        }

        public virtual IReturnProtocolBuilder BuildFooter()
        {
            _currentRow += 2;
            _writer.WriteDataToRange(_ws, "Приел Склад:", true, 10, false, XlHAlign.xlHAlignRight, XlVAlign.xlVAlignBottom, _currentRow, 2, _currentRow, 2);
            _writer.WriteDataToRange(_ws, "Подпис", false, 9, false, XlHAlign.xlHAlignLeft, XlVAlign.xlVAlignTop, _currentRow + 1, 7, _currentRow + 1, 7);
            _writer.WriteDataToRange(_ws, "Дата", false, 9, false, XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignTop, _currentRow + 1, 8, _currentRow + 1, 8);

            _drawer.BorderDraw(_ws, _currentRow, 3, _currentRow, 9, XlLineStyle.xlDot, XlBorderWeight.xlThin, XlBordersIndex.xlEdgeBottom);

            _currentRow += 2;
            _writer.WriteDataToRange(_ws, "Съгласувано (Драгов / Гаджев):", true, 10, false, XlHAlign.xlHAlignRight, XlVAlign.xlVAlignBottom, _currentRow, 2, _currentRow, 2);
            _writer.WriteDataToRange(_ws, "Подпис", false, 9, false, XlHAlign.xlHAlignLeft, XlVAlign.xlVAlignTop, _currentRow + 1, 7, _currentRow + 1, 7);
            _writer.WriteDataToRange(_ws, "Дата", false, 9, false, XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignTop, _currentRow + 1, 8, _currentRow + 1, 8);

            _drawer.BorderDraw(_ws, _currentRow, 3, _currentRow, 9, XlLineStyle.xlDot, XlBorderWeight.xlThin, XlBordersIndex.xlEdgeBottom);

            _currentRow += 2;
            _writer.WriteDataToRange(_ws, "Издал кредитно известие:", true, 10, false, XlHAlign.xlHAlignRight, XlVAlign.xlVAlignBottom, _currentRow, 2, _currentRow, 2);
            _writer.WriteDataToRange(_ws, "Подпис", false, 9, false, XlHAlign.xlHAlignLeft, XlVAlign.xlVAlignTop, _currentRow + 1, 7, _currentRow + 1, 7);
            _writer.WriteDataToRange(_ws, "Дата", false, 9, false, XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignTop, _currentRow + 1, 8, _currentRow + 1, 8);

            _drawer.BorderDraw(_ws, _currentRow, 3, _currentRow, 9, XlLineStyle.xlDot, XlBorderWeight.xlThin, XlBordersIndex.xlEdgeBottom);

            return this;
        }

        public virtual IReturnProtocolBuilder BuildHeather()
        {
            NewPage(_rowCountOnProtocolPage);

            int yRow = _currentRow + 1;

            _writer.WriteDataToRange(_ws, _returnProtocol.UserName, true, 14, false,
                XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignBottom, yRow, 1, yRow + 1, 5);

            yRow += 2;

            _writer.WriteDataToRange(_ws, "Върнал стоката", false, 9, false,
                XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignBottom, yRow, 1, yRow, 5);
            _writer.WriteDataToRange(_ws, "Подпис", false, 9, false,
                XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignBottom, yRow, 7, yRow, 8);
            
            _drawer.BorderDraw(_ws, yRow, 2, yRow, 8, XlLineStyle.xlContinuous, XlBorderWeight.xlThin, XlBordersIndex.xlEdgeTop);

            yRow++;

            _writer.WriteDataToRange(_ws, _returnProtocol.TraderName, true, 14, false,
                XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignBottom, yRow, 1, yRow + 1, 5);

            yRow += 2;
            
            _writer.WriteDataToRange(_ws, "Одобрил връщането", false, 9, false,
                XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignBottom, yRow, 1, yRow, 5);
            _writer.WriteDataToRange(_ws, "Подпис", false, 9, false,
               XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignBottom, yRow, 7, yRow, 8);

            _drawer.BorderDraw(_ws, yRow, 2, yRow, 8, XlLineStyle.xlContinuous, XlBorderWeight.xlThin, XlBordersIndex.xlEdgeTop);

            yRow += 2;

            _writer.WriteDataToRange(_ws, _returnProtocol.PayMethod, true, 16, false,
                XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter, yRow, 7, yRow + 2, 8);

            _drawer.BordersAroundDraw(_ws, yRow, 7, yRow + 2, 8, XlLineStyle.xlContinuous, XlBorderWeight.xlMedium, default);


            _writer.WriteDataToRange(_ws, _returnProtocol.CompanyObject, true, 16, false
                , XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignBottom
                , yRow, 2, yRow, 6);

            yRow++;

            _writer.WriteDataToRange(_ws, "Обект", false, 9, false
                , XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignTop
                , yRow, 2, yRow, 6);

            _drawer.BorderDraw(_ws, yRow, 2, yRow, 6, XlLineStyle.xlContinuous, XlBorderWeight.xlThin, XlBordersIndex.xlEdgeTop);

            yRow++;

            _writer.WriteDataToRange(_ws, _returnProtocol.Address, true, 14, false
                , XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignBottom
                , yRow, 2, yRow, 6);

            yRow++;

            _writer.WriteDataToRange(_ws, "Адрес", false, 9, false
                , XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignTop
                , yRow, 2, yRow, 6);

            _drawer.BorderDraw(_ws, yRow, 2, yRow, 6, XlLineStyle.xlContinuous, XlBorderWeight.xlThin, XlBordersIndex.xlEdgeTop);
                        
            _currentRow = yRow;
            return this;
        }

        public void Export()
        {
            Save();
            _pages.Clear();
            _currentRow = 1;
        }
        public void Dispose()
        {
            if (_excel != null)
            {
                Close();
            }
        }

        public virtual IReturnProtocolBuilder InitializedExporter(IExportReturnProtocol protocol, string filePath, Func<string, bool> messageToCloseAndContinue, IEnumerable<DescriptionCategoryViewModel> descriptionCategoryViewModels)
        {
            FilePath = filePath;
            if (IsFileNotInUse())
            {
                _returnProtocol = protocol;
                _descriptionCategoryViewModels = descriptionCategoryViewModels;
                _excel = new Application();
                OpenFile();
                CreateSheet();
            }
            else if (MessageToCloseExportFile(messageToCloseAndContinue))
            {
                InitializedExporter(protocol, filePath, messageToCloseAndContinue, descriptionCategoryViewModels);
            }
            else
            {
                throw new InvalidOperationException($"Can not export, because file:{FilePath} is open.");
            }
            return this;
        }

        protected virtual void BuildBodyHeather()
        {
            _currentRow++;

            _writer.WriteDataToRange(_ws, "№", true, 14, false, XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter, _currentRow, 1, _currentRow + 1, 1);
            _writer.WriteDataToRange(_ws, "Артикул", true, 12, false, XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter, _currentRow, 2, _currentRow + 1, 2);
            _writer.WriteDataToRange(_ws, "К-во", true, 12, false, XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter, _currentRow, 3, _currentRow + 1, 3, true);
            _writer.WriteDataToRange(_ws, "Мерни единици", true, 10, false, XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter, _currentRow, 4, _currentRow + 1, 4, true);
            _writer.WriteDataToRange(_ws, "Партида", true, 12, false, XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter, _currentRow, 5, _currentRow + 1, 5);
            _writer.WriteDataToRange(_ws, "Срок на годност", true, 12, false, XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter, _currentRow, 6, _currentRow + 1, 6, true);
            _writer.WriteDataToRange(_ws, "Основание", true, 12, false, XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter, _currentRow, 7, _currentRow + 1, 7, true);
            _writer.WriteDataToRange(_ws, "Бележка от склада", true, 12, false, XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter, _currentRow, 8, _currentRow + 1, 9, true);

            _drawer.BordersAroundDraw(_ws, _currentRow, 1, _currentRow + 1, 9, XlLineStyle.xlContinuous, XlBorderWeight.xlMedium, default);
            _drawer.BackgroundColorRange(_ws, gray, _currentRow, 1, _currentRow + 1, 9);

            _currentRow++;
        }

        protected virtual void AddBodyListElement(IExportedReturnedProduct product, int productNumber)
        {
            _currentRow++;

            int rowHight = _currentRow + 2;

            _writer.WriteDataToRange(_ws, $"{productNumber}", true, 12, false, XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter, _currentRow, 1, rowHight, 1);
            _writer.WriteDataToRange(_ws, product.Name, false, 10, false, XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter,_currentRow, 2, rowHight, 2, true);
            _writer.WriteDataToRange(_ws, product.Quantity.ToString(), false, 10, false, XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter, _currentRow, 3, rowHight, 3);
            _writer.WriteDataToRange(_ws, product.Unit, false, 10, false, XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter, _currentRow, 4, rowHight, 4);
            _writer.WriteDataToRange(_ws, char.ToUpper(product.Batch[0]) == 'L' ? product.Batch : $"L: {product.Batch}", false, 10, false, XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter, _currentRow, 5, rowHight, 5, true);
            _writer.WriteDataToRange(_ws, product.BestBefore, false, 10, false, XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter, _currentRow, 6, rowHight, 6);
            _writer.WriteDataToRange(_ws, $"▪{product.DescriptionCategory}{Environment.NewLine}{product.Description}", false, 10, false, XlHAlign.xlHAlignLeft, XlVAlign.xlVAlignTop, _currentRow, 7, rowHight, 7, true);
            _writer.WriteDataToRange(_ws, "Брак ☐ | Годно ☐", false, 11, false, XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignTop, _currentRow, 8, rowHight, 9);

            _drawer.BordersAroundDraw(_ws, _currentRow, 1, rowHight, 9, XlLineStyle.xlContinuous, XlBorderWeight.xlThin, default);

            if (productNumber % 2 == 0)
            {
                _drawer.BackgroundColorRange(_ws, lightGray, _currentRow, 1, rowHight, 9);
            }
            _drawer.BackgroundColorRange(_ws, gray, _currentRow, 1, rowHight, 1);
            _currentRow += 2;
        }

        protected void AddRow(int row)
        {
            _currentRow += row;
        }

        protected void NewPage(int rowCount)
        {
            int row = 0;
            foreach (var page in _pages)
            {
                row += page.RowsCount;
            }
            row++;
            _pages.Add(new ExcelPageModel(_pages.Count + 1, rowCount, row));
                       
            int yRow = _pages[^1].StartRow;
            _writer.WriteDataToRange(_ws, "ДПП", true, 24, false
                , XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter
                , yRow, 3, yRow + 2, 6, true);

            _writer.WriteDataToRange(_ws, $"ДОК {_docNumber}", false, 12, false
                , XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter
                , yRow, 7, yRow + 2, 7);

            _writer.WriteDataToRange(_ws, $"Версия {_version} Дата:{_versionDate}", false, 11, false
                , XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter
                , yRow, 8, yRow + 2, 9, true);

            _drawer.BordersAroundDraw(_ws, yRow, 3, yRow + 2, 9, XlLineStyle.xlContinuous, XlBorderWeight.xlMedium, default);

            _drawer.DrawPictureToRange(_ws, yRow, 2, yRow + 2, 2, 120, 43, new FileInfo($"../../../{_logoFilePath}").FullName);

            yRow += 3;

            _writer.WriteDataToRange(_ws, "ПРОТОКОЛ РЕКЛАМАЦИИ", true, 22, false
               , XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter
               , yRow, 1, yRow + 1, 5);
            _writer.WriteDataToRange(_ws, $"Номер: {_returnProtocol.Id}", false, 13, false
                , XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter
                , yRow, 6, yRow + 1, 6, true);
            _writer.WriteDataToRange(_ws, "Дата на връщане:", false, 12, false
                , XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter
                , yRow, 7, yRow, 9);
            _writer.WriteDataToRange(_ws, _returnProtocol.ReturnDate, false, 14, true
                , XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter
                , yRow + 1, 7, yRow + 1, 9);

            _drawer.BordersAroundDraw(_ws, yRow, 1, yRow + 1, 5, XlLineStyle.xlContinuous, XlBorderWeight.xlThin, default);
            _drawer.BordersAroundDraw(_ws, yRow, 6, yRow, 9, XlLineStyle.xlContinuous, XlBorderWeight.xlThin, default);
            _drawer.BordersAroundDraw(_ws, yRow, 1, yRow + 1, 9, XlLineStyle.xlContinuous, XlBorderWeight.xlMedium, default);

            yRow++;
            _currentRow = yRow;
        }

        protected bool IsFileNotInUse()
        {
            FileInfo fileInfo = new FileInfo(FilePath);
            try
            {
                using (FileStream stream = fileInfo.Open(FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    stream.Close();
                }
            }
            catch
            {
                return false;
            }
            return true;
        }
        protected void OpenFile()
        {
            CreateExportFile();
            _wb = _excel.Workbooks.Open(_excelPath);
        }

        private void CreateExportFile()
        {
            var exporterSection = _configuration.GetSection("ReturnProtocolBuilder");
            string inputPath = exporterSection.GetValue(typeof(string), "inputPath") as string ?? throw new ArgumentNullException("File path not found");
            string fileName = GetType().Name;
            _inputPath = $"{inputPath}{fileName}.xlsx";

            File.Copy(_inputPath, FilePath, true);
            FileInfo fileInfo = new FileInfo(FilePath);
            _excelPath = fileInfo.FullName;
        }
        private void CreateSheet()
        {
            BuildSheet(1);
        }

        protected virtual void BuildSheet(int sheet)
        {
            double[] columnSize = [2, 24, 6.33, 7.56, 12, 9, 17.9, 12, 2];
            _ws = _wb.Worksheets[sheet];
            for (int i = 1; i <= columnSize.Length; i++)
            {
                _ws.Range[_ws.Cells[1, i], _ws.Cells[1, i]].ColumnWidth = columnSize[i - 1];
            }
        }

        protected bool MessageToCloseExportFile(Func<string, bool> message)
        {
            return message.Invoke($"File {FilePath} is open. Please close it and Press OK");
        }
        
        private void Save()
        {
            _wb.Save();
        }

        private void Close()
        {
            _wb.Close(false, _excelPath, Missing.Value);
            _excel.DisplayAlerts = true;
            _excel.Quit();
            Release(_ws);
            Release(_wb);
            Release(_excel);
            _ws = null!;
            _wb = null!;
            _excel = null!;
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        private void Release(object obj)
        {
            try
            {                
                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(obj);
            }
            catch { }
        }
    }
}
