using Microsoft.Office.Interop.Excel;
using System.IO;
using _Excel = Microsoft.Office.Interop.Excel;
using System.IO;

namespace DelitaTrade.Models.Builder
{
    public class ExcelBuilder : IDayReportBuilder
    {
        private const int rowsOnPage = 51;

        private _Application excel;
        private Workbook wb;
        private Worksheet ws;
        private string _inputPath;
        private string _path;
        private string _excelPath;

        private ExcelWriter _writer;
        private ExcelDrawing _drawing;
        private DayReport dayReport;

        private List<InvoiceMarker> _invoiceMarker;

        private decimal _totalSum = 0;
        private int _startRow;
        private int _row;
        private int _counter = 1;
        private int _pageCount = 0;        

        public ExcelBuilder(string inputPath, string path)
        {
            _writer = new ExcelWriter();
            _drawing = new ExcelDrawing();
            _invoiceMarker = new List<InvoiceMarker>();
            _inputPath = inputPath;
            Path = path;
        }
        private void Save()
        {
            wb.Save();
            wb.Close();
        }

        private void OpenFile()
        {
            CreateExportFile();
            wb = excel.Workbooks.Open(_excelPath);
        }

        private void CreateSheet()
        {
            BuildSheet(1);
        }
        public void InitializedExporter(DayReport report)        
        {
            dayReport = report;
            excel = new _Excel.Application();
            OpenFile();
            CreateSheet();
        }

        private void CreateExportFile()
        {
            File.Copy(_inputPath, Path, true);
            FileInfo fileInfo = new FileInfo(Path);
            _excelPath = fileInfo.FullName;
        }
        public void Export()
        {
            Save();
        }
        public string Path
        {
            get => _path;
            set
            {
                _path = value;
            }
        }

        

        public string ExportedFilePath => _excelPath;

        private void InitializeData()
        {
            _totalSum = 0;
            _row = _startRow;
        }

        private void BuildBodyHeather()
        {
            InitializeData();
            _writer.WriteDataToRange(ws, "№", true, 14, false, XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter, _row, 1, _row + 1, 1);
            _writer.WriteDataToRange(ws, "Име на фирма", true, 12, false, XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter, _row, 2, _row + 1, 2);
            _writer.WriteDataToRange(ws, "Име на обект", true, 12, false, XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter, _row, 3, _row + 1, 4);
            _writer.WriteDataToRange(ws, "Документ №", true, 12, false, XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter, _row, 5, _row + 1, 5);
            _writer.WriteDataToRange(ws, "Тотал лв.", true, 12, false, XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter, _row, 6, _row + 1, 6);
            _writer.WriteDataToRange(ws, "Платено лв.", true, 12, false, XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter, _row, 7, _row + 1, 8);
            _drawing.BordersAroundDraw(ws, _row, 1, _row + 1, 8, XlLineStyle.xlContinuous, XlBorderWeight.xlMedium, default);
            _row += 2;
        }

        private void SetBodyComponentColor()
        {
            if(_counter % 2 == 0)
            {
                _drawing.BackgroundColorRange(ws, "#ededed", _row, 1, _row, 8);
            }
        }

        private void SetBanknoteComponentColor(int column, int counter)
        {
            if (counter % 2 == 0)
            {
                _drawing.BackgroundColorRange(ws, "#ededed", _row, column, _row, column + 3);
            }
        }

        private void SetNewBodyPage()
        {
            _pageCount++;
            _startRow = _row;
            BuildBodyHeather();
        }

        private void BuildBodyComponent(string name, ref bool isInitialized, ref bool nonPayExists, Invoice invoice, List<string> nonPayInvoices, Func<Invoice, bool> isNonPayInvoice, Func<string, int> repeatInvoiceCount)
        {
            if (isInitialized && IsNewPage(1))
            {
                _drawing.BordersAroundDraw(ws, _startRow + 2, 1, _row - 1, 8, XlLineStyle.xlContinuous, XlBorderWeight.xlMedium, "#A6A6A6");
                _row++;
                SetNewBodyPage();
            }
            else if (IsNewPage())
            {
                SetNewBodyPage();
            }

            if (isInitialized && (name == "Плащане в брой" && isNonPayInvoice(invoice)) == false)
            {
                isInitialized = false;
                _drawing.BackgroundColorRange(ws, XlRgbColor.rgbLightGrey, _row, 1, _row, 8);
                _writer.WriteDataToRange(ws, name, true, 11, false, XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter, _row, 1, _row, 8);
                _row++;
                _counter = 1;
            }

            switch (name)
            {
                case "Плащане по банка":
                    _writer.WriteDataToCell(ws, invoice.Amount.ToString("C"), _row, 6);
                    _writer.WriteDataToCell(ws, invoice.PayMethod, _row, 7);
                    break;
                case "Плащане с карта":
                    _writer.WriteDataToCell(ws, invoice.Amount.ToString("C"), _row, 6);
                    _writer.WriteDataToCell(ws, invoice.Income.ToString("C"), _row, 7);
                    break;
                case "Плащане в брой":
                    if (isNonPayInvoice(invoice))
                    {
                        nonPayExists = true;
                        nonPayInvoices.Add($"{invoice.Amount:C} -- {invoice.Income:C} -- {invoice.CompanyName} -- {invoice.PayMethod} -- {invoice.InvoiceID}");
                        return;
                    }
                    else
                    {
                        _writer.WriteDataToCell(ws, invoice.Amount.ToString("C"), _row, 6);
                        _writer.WriteDataToCell(ws, invoice.Income.ToString("C"), _row, 7);
                    }
                    break;
                case "Кредитни известия":
                    if (invoice.Income == 0)
                    {
                        _writer.WriteDataToCell(ws, invoice.PayMethod, _row, 7);
                        _writer.WriteDataToCell(ws, invoice.Amount.ToString("C"), _row, 6);
                    }
                    else
                    {
                        _writer.WriteDataToCell(ws, invoice.PayMethod, _row, 6);
                        _writer.WriteDataToCell(ws, invoice.Income.ToString("C"), _row, 7);
                    }
                    break;
                case "Разходи":
                    _writer.WriteDataToCell(ws, invoice.PayMethod, _row, 6);
                    _writer.WriteDataToCell(ws, invoice.Income.ToString("C"), _row, 7);
                    break;
                case "Стари сметки":
                    _writer.WriteDataToCell(ws, invoice.Amount.ToString("C"), _row, 6);
                    _writer.WriteDataToCell(ws, invoice.Income.ToString("C"), _row, 7);
                    break;
            }

            _writer.WriteDataToCell(ws, _counter.ToString(), _row, 1);
            if (repeatInvoiceCount(invoice.InvoiceID) > 1)
            {
                InvoiceMarker marker;

                if (_invoiceMarker.Count == 0)
                {
                    marker = new InvoiceMarker(invoice.InvoiceID);
                    _invoiceMarker.Add(marker);
                }
                else if (_invoiceMarker.FirstOrDefault(m => m.InvoiceId == invoice.InvoiceID) == null)
                {
                    marker = new InvoiceMarker(_invoiceMarker[^1]);
                    _invoiceMarker.Add(new InvoiceMarker(marker));
                }
                else 
                {
                    marker = _invoiceMarker.FirstOrDefault(m => m.InvoiceId == invoice.InvoiceID);
                }
                _writer.WriteDataToCell(ws, $"{marker}{invoice.CompanyName}", _row, 2);
            }
            else 
            {
                _writer.WriteDataToCell(ws, $"{invoice.CompanyName}", _row, 2);
            }
            _writer.WriteDataToRange(ws, invoice.ObjectName, false, 11, false, XlHAlign.xlHAlignLeft, XlVAlign.xlVAlignTop, _row, 3, _row, 4);
            _writer.WriteDataToCell(ws, invoice.InvoiceID, false, 11, XlHAlign.xlHAlignLeft, XlVAlign.xlVAlignTop, _row, 5);

            SetBodyComponentColor();

            _row++;
            _counter++;
        }

        private static bool IsNonPayInvoice(Invoice invoice)
        {
            return invoice.Amount > invoice.Income;
        }

        private void PrintNonPayInvoice(List<string> nonPayInvoices, ref bool nonPayExists)
        {
            bool toNextPage = false;
            if (nonPayExists)
            {
                if (IsNewPage(1))
                {
                    _drawing.BordersAroundDraw(ws, _startRow + 2, 1, _row - 1, 8, XlLineStyle.xlContinuous, XlBorderWeight.xlMedium, "#A6A6A6");
                    _row++;
                    SetNewBodyPage();
                }
                else if (IsNewPage())
                {
                    _drawing.BordersAroundDraw(ws, _startRow + 2, 1, _row - 1, 8, XlLineStyle.xlContinuous, XlBorderWeight.xlMedium, "#A6A6A6");
                    SetNewBodyPage();
                }

                List<string> orderedList = nonPayInvoices.OrderBy(i => i.Split(" -- ", StringSplitOptions.RemoveEmptyEntries)[3] == "За кредитно" ? 1 :
                                            i.Split(" -- ", StringSplitOptions.RemoveEmptyEntries)[3] == "В брой" ? 2 :
                                            i.Split(" -- ", StringSplitOptions.RemoveEmptyEntries)[3] == "За анулиране" ? 3 :
                                            4).ToList();
                _drawing.BackgroundColorRange(ws, XlRgbColor.rgbLightGrey, _row, 1, _row, 8);
                _writer.WriteDataToRange(ws, "Неплатени", true, 11, false, XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter, _row, 1, _row, 8);
                _counter = 1;
                _row++;
               
                foreach (string invoice in orderedList)
                {
                    string[] amountIncome = invoice.Split(" -- ", StringSplitOptions.RemoveEmptyEntries);
                    if (toNextPage)
                    {
                        SetNewBodyPage();
                        toNextPage = false;
                    }

                    _writer.WriteDataToCell(ws, _counter.ToString(), _row, 1);
                    _writer.WriteDataToCell(ws, amountIncome[2], _row, 2);
                    _writer.WriteDataToCell(ws, amountIncome[0], _row, 6);
                    _writer.WriteDataToCell(ws, amountIncome[1], _row, 7);
                    _writer.WriteDataToRange(ws, amountIncome[3], false, 11, false, XlHAlign.xlHAlignLeft, XlVAlign.xlVAlignTop, _row, 3, _row, 4);
                    _writer.WriteDataToCell(ws, amountIncome[4], false, 11, XlHAlign.xlHAlignLeft, XlVAlign.xlVAlignTop, _row, 5);

                    SetBodyComponentColor();

                    _row++;
                    _counter++;
                    if (IsNewPage())
                    {
                        _drawing.BordersAroundDraw(ws, _startRow + 2, 1, _row - 1, 8, XlLineStyle.xlContinuous, XlBorderWeight.xlMedium, "#A6A6A6");
                        toNextPage = true;
                    }
                }
            }
        }

        private bool IsNewPage()
        {
            return _row % (rowsOnPage + 1) == 0;
        }

        private bool IsNewPage(int nextRow)
        {
            return (_row + nextRow) % (rowsOnPage + 1) == 0;
        }

        private void BuildBanknotesFooter(IEnumerable<KeyValuePair<decimal, int>> filteredBanknotes)
        {
            _writer.WriteDataToRange(ws, "Отчет на парите", true, 14, false
                , XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter
                , _row, 3, _row, 8);
            _row++;

            int banknotesCount = filteredBanknotes.Count();

            int rowCount = 0;
            if (banknotesCount % 2 == 0)
            {
                rowCount = banknotesCount / 2;
            }
            else
            {
                rowCount = (banknotesCount / 2) + 1;
            }

            int rowStart = _row;

            int column = 2;
            _totalSum = 0;
            int counter = 1;

            foreach (var banknote in filteredBanknotes)
            {
                _totalSum += banknote.Key * banknote.Value;
                _writer.WriteDataToCell(ws, $"{banknote.Value} -", false, 11,
                                        XlHAlign.xlHAlignRight, XlVAlign.xlVAlignBottom, _row, column);
                _writer.WriteDataToCell(ws, banknote.Key.ToString("C"), _row, column + 1);
                _writer.WriteDataToCell(ws, (banknote.Value * banknote.Key).ToString("C"), _row, column + 2);

                SetBanknoteComponentColor(column, counter);

                counter++;
                _row++;
                rowCount--;
                if (rowCount == 0)
                {
                    column += 3;
                    counter = 1;
                    _row = rowStart;
                }
            }

            _writer.WriteDataToCell(ws, "Тотал:", true, 11,
                XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter, _row, 6);
            _writer.WriteDataToRange(ws, _totalSum.ToString("C"), true, 11, false
                ,XlHAlign.xlHAlignLeft, XlVAlign.xlVAlignCenter, _row, 7, _row, 8);
            _drawing.BordersAroundDraw(ws, rowStart - 1, 3, _row, 8, XlLineStyle.xlContinuous, XlBorderWeight.xlMedium, default);
            _row++;
            
            _writer.WriteDataToCell(ws, "Преброена сума", false, 11,
                XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter, _row, 2);            
            _writer.WriteDataToCell(ws, "Разлика", false, 11,
                XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter, _row + 2, 2);
            _drawing.BordersAroundDraw(ws, _row, 2, _row + 2, 2, XlLineStyle.xlContinuous, XlBorderWeight.xlMedium, default);
            _drawing.BordersAroundDraw(ws, _row + 2, 2, _row + 3, 2, XlLineStyle.xlContinuous, XlBorderWeight.xlMedium, default);
        }

        private void BuildVehicleFooter()
        {
            _drawing.WriteDataToRange(ws, "Превозно средство №:", false, 12, false
                , XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter
                , _row + 5, 2, _row + 5, 2);
            _drawing.WriteDataToRange(ws, dayReport.Vehicle, true, 12, false
                , XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter
                , _row + 6, 2, _row + 6, 2);
            _drawing.BordersAroundDraw(ws, _row + 5, 2, _row + 6, 2, XlLineStyle.xlContinuous, XlBorderWeight.xlMedium, default);
        }

        private void BuildTotalsFooter()
        {
            _writer.WriteDataToRange(ws, "Общо приходи от доставки: ", false, 12, false
                , XlHAlign.xlHAlignRight, XlVAlign.xlVAlignCenter
                , _row, 3, _row, 5);
            _writer.WriteDataToRange(ws, dayReport.TotalAmaunt.ToString("C"), false, 12, false
                , XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter
                , _row, 6, _row, 8);

            _writer.WriteDataToRange(ws, "Общо приходи от стари сметки: ", false, 12, false
                , XlHAlign.xlHAlignRight, XlVAlign.xlVAlignCenter
                , _row + 1, 3, _row + 1, 5);
            _writer.WriteDataToRange(ws, dayReport.TotalOldInvoice.ToString("C"), false, 12, false
                , XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter
                , _row + 1, 6, _row + 1, 8);

            _writer.WriteDataToRange(ws, "Сума от неплатени сметки: ", false, 12, false
                , XlHAlign.xlHAlignRight, XlVAlign.xlVAlignCenter
                , _row + 2, 3, _row + 2, 5);
            _writer.WriteDataToRange(ws, dayReport.TotalNonPay.ToString("C"), false, 12, false
                , XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter
                , _row + 2, 6, _row + 2, 8);

            _writer.WriteDataToRange(ws, "Сума от документи по банка: ", false, 12, false
                , XlHAlign.xlHAlignRight, XlVAlign.xlVAlignCenter
                , _row + 3, 3, _row + 3, 5);
            _writer.WriteDataToRange(ws, dayReport.BankPayTotal().ToString("C"), false, 12, false
                , XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter
                , _row + 3, 6, _row + 3, 8);

            _writer.WriteDataToRange(ws, "Разходи + КИ: ", false, 12, false
                , XlHAlign.xlHAlignRight, XlVAlign.xlVAlignCenter
                , _row + 4, 3, _row + 4, 5);
            _writer.WriteDataToRange(ws, dayReport.TotalExpenses.ToString("C"), false, 12, false
                , XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter
                , _row + 4, 6, _row + 4, 8);

            _writer.WriteDataToRange(ws, "Тотал - Отчетена сума: ", true, 14, false
                , XlHAlign.xlHAlignRight, XlVAlign.xlVAlignCenter
                , _row + 5, 3, _row + 5, 5);
            _writer.WriteDataToRange(ws, dayReport.TotalIncome.ToString("C"), true, 14, false
                , XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter
                , _row + 5, 6, _row + 5, 8);

            if (_totalSum - dayReport.TotalIncome < 0)
            {
                _writer.WriteDataToCell(ws, "Задължение:", false, 12, XlHAlign.xlHAlignRight, XlVAlign.xlVAlignCenter, _row + 6, 5);
            }
            else
            {
                _writer.WriteDataToCell(ws, "Ресто:", false, 12, XlHAlign.xlHAlignRight, XlVAlign.xlVAlignCenter, _row + 6, 5);
            }

            _writer.WriteDataToRange(ws, Math.Abs(_totalSum - dayReport.TotalIncome).ToString("C"), false, 12, false
                , XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter, _row + 6, 6, _row + 6, 8);

            _drawing.BordersAroundDraw(ws, _row, 6, _row + 6, 8, XlLineStyle.xlContinuous, XlBorderWeight.xlMedium, default);
        }

        private void BuildPersonalInformationFooter()
        {   
            _writer.WriteDataToRange(ws, "Съставил отчета / Предал:", false, 11, false
                , XlHAlign.xlHAlignLeft, XlVAlign.xlVAlignBottom
                , _row + 7, 2, _row + 7, 2);
            _writer.WriteDataToRange(ws, "Приел отчета:", false, 11, false
                , XlHAlign.xlHAlignLeft, XlVAlign.xlVAlignBottom
                , _row + 7, 4, _row + 7, 5);

            _writer.WriteDataToRange(ws, "Александър Сергеевич Добров", true, 16, false
                , XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignBottom
                , _row + 8, 2, _row + 9, 3);

            _writer.WriteDataToRange(ws, "(име, фамилия, подпис)", false, 10, false
                , XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignBottom
                , _row + 10, 2, _row + 10, 3);
            _drawing.BorderDraw(ws, _row + 10, 2, _row + 10, 3, XlLineStyle.xlContinuous, XlBorderWeight.xlThin, XlBordersIndex.xlEdgeTop);

            _writer.WriteDataToRange(ws, "(име, фамилия, подпис)", false, 10, false
                , XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignBottom
                , _row + 10, 5, _row + 10, 8);
            _drawing.BorderDraw(ws, _row + 10, 5, _row + 10, 8, XlLineStyle.xlContinuous, XlBorderWeight.xlThin, XlBordersIndex.xlEdgeTop);

            _writer.WriteDataToRange(ws, "Обработил отчета:", false, 11, false
               , XlHAlign.xlHAlignLeft, XlVAlign.xlVAlignBottom
               , _row + 11, 4, _row + 11, 5);

            _writer.WriteDataToRange(ws, "(име, фамилия, подпис)", false, 10, false
                , XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignBottom
                , _row + 14, 5, _row + 14, 8);
            _drawing.BorderDraw(ws, _row + 14, 5, _row + 14, 8, XlLineStyle.xlContinuous, XlBorderWeight.xlThin, XlBordersIndex.xlEdgeTop);

            string transmisionDate = string.Join('-', dayReport.TransmissionDate.Split('-',StringSplitOptions.RemoveEmptyEntries).Reverse());

            _drawing.WriteDataToRange(ws, "Дата на предаване", false, 12, false
                , XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter
                , _row + 13, 2, _row + 13, 2);
            _drawing.WriteDataToRange(ws, $"{transmisionDate}", true, 12, true
                , XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter
                , _row + 14, 2, _row + 14, 2);
            _drawing.BordersAroundDraw(ws, _row + 13, 2, _row + 14, 2, XlLineStyle.xlContinuous, XlBorderWeight.xlMedium, default);
        }

        private bool IsNotEnoughSpaceForFooterOnPage(int banknotesCount)
        {
            int rowRemaining = 51 - (_row % 52);
            int banknoteIndex = 0;
            if (banknotesCount > 0) 
            {
                banknoteIndex = 2;
            }
            return rowRemaining < 16 + banknotesCount / 2 + banknoteIndex;            
        }
        private void BuildSheet(int sheet)
        {
            double[] columnSize = [2, 32, 9.33, 10.56, 12.78, 11.56, 11.56, 2];
            ws = wb.Worksheets[sheet];
            for (int i = 1; i <= columnSize.Length; i++) 
            {
                ws.Range[ws.Cells[1, i], ws.Cells[1, i]].ColumnWidth = columnSize[i - 1];
            }            
        }

        public void BuildHeather()
        {   
            _writer.WriteDataToRange(ws, "Дневен Отчет", true, 22, false
                , XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter
                , 1, 1, 2, 5);
            _writer.WriteDataToRange(ws, "Дата на доставките:", false, 12, false
                , XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter 
                , 1, 6, 1, 8);
            _writer.WriteDataToRange(ws, dayReport.DayReportID, false, 14, true
                , XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter
                , 2, 6, 2, 8);

            _drawing.BordersAroundDraw(ws, 1, 1, 2, 5, XlLineStyle.xlContinuous, XlBorderWeight.xlThin, default);
            _drawing.BordersAroundDraw(ws, 1, 6, 1, 8, XlLineStyle.xlContinuous, XlBorderWeight.xlThin, default);
            _drawing.BordersAroundDraw(ws, 1, 1, 2, 8, XlLineStyle.xlContinuous, XlBorderWeight.xlMedium, default);

            _startRow = 3;
            _pageCount = 1;
        }

        public void BuildBody()
        {
            BuildBodyHeather();
            IEnumerable<Invoice> invoices = dayReport.GetAllInvoices()
                .OrderBy(i => i.PayMethod == "В брой" ? 1 :
                              i.PayMethod == "За кредитно" ? 2 :
                              i.PayMethod == "За анулиране" ? 3 :
                              i.PayMethod == "С карта" ? 4 :
                              i.PayMethod == "Банка" ? 5 :
                              i.PayMethod == "Стара сметка" ? 6 :
                              i.PayMethod == "Кредитно" ? 7 :
                              i.PayMethod == "Разход" ? 8 :
                              9)
                .ThenBy(i => i.InvoiceID);

            bool expenses = true;
            bool creditNote = true;
            bool oldInvoice = true;
            bool bankPay = true;
            bool cashPay = true;
            bool cardPay = true;

            bool nonPayExists = false;

            List<string> nonPayInvoices = new List<string>();

            foreach (Invoice invoice in invoices)
            {               
                switch (invoice.PayMethod)
                {
                    case "Банка":
                        BuildBodyComponent("Плащане по банка", ref bankPay, ref nonPayExists, invoice, nonPayInvoices, dayReport.IsNonPayInvoice, dayReport.GetId);                        
                        break;
                    case "С карта":
                        BuildBodyComponent("Плащане с карта", ref cardPay, ref nonPayExists, invoice, nonPayInvoices, dayReport.IsNonPayInvoice, dayReport.GetId);
                        
                        break;
                    case "В брой":
                    case "За кредитно":
                    case "За анулиране":
                        BuildBodyComponent("Плащане в брой", ref cashPay, ref nonPayExists, invoice, nonPayInvoices, dayReport.IsNonPayInvoice, dayReport.GetId);
                       
                        break;
                    case "Кредитно":
                        BuildBodyComponent("Кредитни известия", ref creditNote, ref nonPayExists, invoice, nonPayInvoices, dayReport.IsNonPayInvoice, dayReport.GetId);
                        
                        break;
                    case "Разход":
                        BuildBodyComponent("Разходи", ref expenses, ref nonPayExists, invoice, nonPayInvoices, dayReport.IsNonPayInvoice, dayReport.GetId);
                        
                        break;
                    case "Стара сметка":
                        BuildBodyComponent("Стари сметки", ref oldInvoice, ref nonPayExists, invoice, nonPayInvoices, dayReport.IsNonPayInvoice, dayReport.GetId);
                        
                        break;
                }

                if (IsNewPage())
                {
                    _drawing.BordersAroundDraw(ws, _startRow + 2, 1, _row - 1, 8, XlLineStyle.xlContinuous, XlBorderWeight.xlMedium, "#A6A6A6");                   
                }
            }

            PrintNonPayInvoice(nonPayInvoices, ref nonPayExists);

            if (IsNewPage() == false)
            {
                _drawing.BordersAroundDraw(ws, _startRow + 2, 1, _row - 1, 8, XlLineStyle.xlContinuous, XlBorderWeight.xlMedium, "#A6A6A6");
            }
            _invoiceMarker = new List<InvoiceMarker>();
        }

        public void BuildFooter()
        {
            var filteredBanknotes = dayReport.GetAllBanknotes().Where(b => b.Value > 0);
            int banknotesCount = filteredBanknotes.Count();

            if (IsNotEnoughSpaceForFooterOnPage(banknotesCount))
            {
                _row = _pageCount * 51 + 1;
            }

            if (banknotesCount > 0)
            {
                BuildBanknotesFooter(filteredBanknotes);
            }

            BuildTotalsFooter();
            BuildVehicleFooter();
            BuildPersonalInformationFooter();

        }

    }
}
