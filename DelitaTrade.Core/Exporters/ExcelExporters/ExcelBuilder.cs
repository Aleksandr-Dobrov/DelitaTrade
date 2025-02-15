using Microsoft.Office.Interop.Excel;
using System.IO;
using _Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;
using System.Windows;
using DelitaTrade.Common.Enums;
using static DelitaTrade.Common.Enums.EnumTranslators.PayMethodTranslator;
using static DelitaTrade.Common.GlobalVariables;
using DelitaTrade.Core.Interfaces;

namespace DelitaTrade.Core.Exporters.ExcelExporters
{
    internal class ExcelBuilder : IDayReportBuilder
    {
        private const int rowsOnPage = 51;

        private _Application _excel;
        private Workbook _wb;
        private Worksheet _ws;

        private ExcelWriter _writer;
        private ExcelDrawing _drawing;

        private IExportedDayReport _dayReport;

        private List<InvoiceMarker> _invoiceMarker;

        private decimal _totalSum = 0;
        private int _startRow;
        private int _row;
        private int _counter = 1;
        private int _pageCount = 0;

        private readonly string _inputPath;
        private string _path;
        private string _excelPath;

        public ExcelBuilder(string inputPath)
        {
            _writer = new ExcelWriter();
            _drawing = new ExcelDrawing();
            _invoiceMarker = new List<InvoiceMarker>();
            _inputPath = inputPath;
        }

        public string ExportedFilePath => _excelPath;

        public string Path
        {
            get => _path;
            set
            {
                _path = value;
            }
        }

        public IDayReportBuilder InitializedExporter(IExportedDayReport report, string filePath, Func<string, bool> messageToCloseAndContinue)
        {
            Path = filePath;
            if (IsFileNotInUse())
            {
                _dayReport = report;
                _excel = new Application();
                OpenFile();
                CreateSheet();
            }
            else if (MessageToCloseExportFile(messageToCloseAndContinue))
            {
                InitializedExporter(report, filePath, messageToCloseAndContinue);
            }
            else
            {
                throw new InvalidOperationException($"Can not export, because file:{Path} is open.");
            }
            return this;
        }

        public IDayReportBuilder BuildHeather()
        {
            _writer.WriteDataToRange(_ws, "Дневен Отчет", true, 22, false
                , XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter
                , 1, 1, 2, 5);
            _writer.WriteDataToRange(_ws, "Дата на доставките:", false, 12, false
                , XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter
                , 1, 6, 1, 8);
            _writer.WriteDataToRange(_ws, _dayReport.DayReportID, false, 14, true
                , XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter
                , 2, 6, 2, 8);

            _drawing.BordersAroundDraw(_ws, 1, 1, 2, 5, XlLineStyle.xlContinuous, XlBorderWeight.xlThin, default);
            _drawing.BordersAroundDraw(_ws, 1, 6, 1, 8, XlLineStyle.xlContinuous, XlBorderWeight.xlThin, default);
            _drawing.BordersAroundDraw(_ws, 1, 1, 2, 8, XlLineStyle.xlContinuous, XlBorderWeight.xlMedium, default);

            _startRow = 3;
            _pageCount = 1;
            return this;
        }

        public IDayReportBuilder BuildBody()
        {
            BuildBodyHeather();
            IEnumerable<IExportedInvoice> invoices = _dayReport.Invoices
                .OrderBy(i => i.PayMethod == PayMethod.Cash ? 1 :
                              i.PayMethod == PayMethod.ForCreditNote ? 2 :
                              i.PayMethod == PayMethod.Cancellation ? 3 :
                              i.PayMethod == PayMethod.Card ? 4 :
                              i.PayMethod == PayMethod.Bank ? 5 :
                              i.PayMethod == PayMethod.OldPayCash ? 6 :
                              i.PayMethod == PayMethod.OldPayCard ? 7 :
                              i.PayMethod == PayMethod.CreditNote ? 8 :
                              i.PayMethod == PayMethod.Expense ? 9 :
                              10)
                .ThenBy(i => i.Amount == 0)
                .ThenBy(i => i.Number);

            bool expenses = true;
            bool creditNote = true;
            bool oldInvoice = true;
            bool bankPay = true;
            bool cashPay = true;
            bool cardPay = true;

            bool nonPayExists = false;

            List<string> nonPayInvoices = new List<string>();

            foreach (IExportedInvoice invoice in invoices)
            {
                switch (invoice.PayMethod)
                {
                    case PayMethod.Bank:
                        BuildBodyComponent("Плащане по банка", ref bankPay, ref nonPayExists, invoice, nonPayInvoices, _dayReport.GetRepeatNumbering);
                        break;
                    case PayMethod.Card:
                        BuildBodyComponent("Плащане с карта", ref cardPay, ref nonPayExists, invoice, nonPayInvoices, _dayReport.GetRepeatNumbering);

                        break;
                    case PayMethod.Cash:
                    case PayMethod.ForCreditNote:
                    case PayMethod.Cancellation:
                        BuildBodyComponent("Плащане в брой", ref cashPay, ref nonPayExists, invoice, nonPayInvoices, _dayReport.GetRepeatNumbering);

                        break;
                    case PayMethod.CreditNote:
                        BuildBodyComponent("Кредитни известия", ref creditNote, ref nonPayExists, invoice, nonPayInvoices, _dayReport.GetRepeatNumbering);

                        break;
                    case PayMethod.Expense:
                        BuildBodyComponent("Разходи", ref expenses, ref nonPayExists, invoice, nonPayInvoices, _dayReport.GetRepeatNumbering);

                        break;
                    case PayMethod.OldPayCash:
                    case PayMethod.OldPayCard:
                        BuildBodyComponent("Стари сметки", ref oldInvoice, ref nonPayExists, invoice, nonPayInvoices, _dayReport.GetRepeatNumbering);

                        break;
                }

                if (IsNewPage())
                {
                    _drawing.BordersAroundDraw(_ws, _startRow + 2, 1, _row - 1, 8, XlLineStyle.xlContinuous, XlBorderWeight.xlMedium, "#A6A6A6");
                }
            }

            PrintNonPayInvoice(nonPayInvoices, ref nonPayExists);

            if (IsNewPage() == false)
            {
                _drawing.BordersAroundDraw(_ws, _startRow + 2, 1, _row - 1, 8, XlLineStyle.xlContinuous, XlBorderWeight.xlMedium, "#A6A6A6");
            }
            _invoiceMarker = new List<InvoiceMarker>();
            return this;
        }

        public IDayReportBuilder BuildFooter()
        {
            var filteredBanknotes = _dayReport.Banknotes.Where(b => b.Value > 0);
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
            return this;
        }

        public void Export()
        {
            Save();
        }

        public void Dispose()
        {
            if (_excel != null)
            {
                Close();
            }
        }

        private void Save()
        {
            _wb.Save();
        }

        private void Close()
        {
            _wb.Close(true, _excelPath, Missing.Value);
            _excel.DisplayAlerts = true;
            _excel.Quit();
            Release(_ws);
            Release(_wb);
            Release(_excel);
            _ws = null;
            _wb = null;
            _excel = null;
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

        private void OpenFile()
        {
            CreateExportFile();
            _wb = _excel.Workbooks.Open(_excelPath);
        }

        private void CreateSheet()
        {
            BuildSheet(1);
        }

        private void CreateExportFile()
        {
            File.Copy(_inputPath, Path, true);
            FileInfo fileInfo = new FileInfo(Path);
            _excelPath = fileInfo.FullName;
        }

        private bool IsFileNotInUse()
        {
            FileInfo fileInfo = new FileInfo(Path);
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

        private bool MessageToCloseExportFile(Func<string, bool> message)
        {
            return message.Invoke($"File {Path} is open. Please close it and Press OK");
        }

        private void InitializeData()
        {
            _totalSum = 0;
            _row = _startRow;
        }

        private void BuildBodyHeather()
        {
            InitializeData();
            _writer.WriteDataToRange(_ws, "№", true, 14, false, XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter, _row, 1, _row + 1, 1);
            _writer.WriteDataToRange(_ws, "Име на фирма", true, 12, false, XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter, _row, 2, _row + 1, 2);
            _writer.WriteDataToRange(_ws, "Име на обект", true, 12, false, XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter, _row, 3, _row + 1, 4);
            _writer.WriteDataToRange(_ws, "Документ №", true, 12, false, XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter, _row, 5, _row + 1, 5);
            _writer.WriteDataToRange(_ws, "Тотал лв.", true, 12, false, XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter, _row, 6, _row + 1, 6);
            _writer.WriteDataToRange(_ws, "Платено лв.", true, 12, false, XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter, _row, 7, _row + 1, 8);
            _drawing.BordersAroundDraw(_ws, _row, 1, _row + 1, 8, XlLineStyle.xlContinuous, XlBorderWeight.xlMedium, default);
            _row += 2;
        }

        private void SetBodyComponentColor()
        {
            if (_counter % 2 == 0)
            {
                _drawing.BackgroundColorRange(_ws, "#ededed", _row, 1, _row, 8);
            }
        }

        private void SetBanknoteComponentColor(int column, int counter)
        {
            if (counter % 2 == 0)
            {
                _drawing.BackgroundColorRange(_ws, "#ededed", _row, column, _row, column + 3);
            }
        }

        private void SetNewBodyPage()
        {
            _pageCount++;
            _startRow = _row;
            BuildBodyHeather();
        }

        private void BuildBodyComponent(string name, ref bool isInitialized, ref bool nonPayExists, IExportedInvoice invoice, List<string> nonPayInvoices, Func<string, int> repeatInvoiceCount)
        {
            if (isInitialized && IsNewPage(1))
            {
                _drawing.BordersAroundDraw(_ws, _startRow + 2, 1, _row - 1, 8, XlLineStyle.xlContinuous, XlBorderWeight.xlMedium, "#A6A6A6");
                _row++;
                SetNewBodyPage();
            }
            else if (IsNewPage())
            {
                SetNewBodyPage();
            }

            if (isInitialized && (name == "Плащане в брой" && invoice.IsPaid == false) == false)
            {
                isInitialized = false;
                _drawing.BackgroundColorRange(_ws, XlRgbColor.rgbLightGrey, _row, 1, _row, 8);
                _writer.WriteDataToRange(_ws, name, true, 11, false, XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter, _row, 1, _row, 8);
                _row++;
                _counter = 1;
            }

            switch (name)
            {
                case "Плащане по банка":
                    _writer.WriteDataToCell(_ws, invoice.Amount.ToString("C"), _row, 6);
                    _writer.WriteDataToCell(_ws, PayMethodsToString[Language][invoice.PayMethod], _row, 7);
                    break;
                case "Плащане с карта":
                    _writer.WriteDataToCell(_ws, invoice.Amount.ToString("C"), _row, 6);
                    _writer.WriteDataToCell(_ws, invoice.Income.ToString("C"), _row, 7);
                    break;
                case "Плащане в брой":
                    if (invoice.IsPaid == false)
                    {
                        nonPayExists = true;
                        nonPayInvoices.Add($"{invoice.Amount:C} -- {invoice.Income:C} -- {invoice.CompanyFullName} -- {invoice.PayMethod} -- {invoice.Number}");
                        return;
                    }
                    else
                    {
                        _writer.WriteDataToCell(_ws, invoice.Amount.ToString("C"), _row, 6);
                        _writer.WriteDataToCell(_ws, invoice.Income.ToString("C"), _row, 7);
                    }
                    break;
                case "Кредитни известия":
                    if (invoice.Income == 0)
                    {
                        _writer.WriteDataToCell(_ws, PayMethodsToString[Language][invoice.PayMethod], _row, 7);
                        _writer.WriteDataToCell(_ws, invoice.Amount.ToString("C"), _row, 6);
                    }
                    else
                    {
                        _writer.WriteDataToCell(_ws, PayMethodsToString[Language][invoice.PayMethod], _row, 6);
                        _writer.WriteDataToCell(_ws, invoice.Income.ToString("C"), _row, 7);
                    }
                    break;
                case "Разходи":
                    _writer.WriteDataToCell(_ws, PayMethodsToString[Language][invoice.PayMethod], _row, 6);
                    _writer.WriteDataToCell(_ws, invoice.Income.ToString("C"), _row, 7);
                    break;
                case "Стари сметки":
                    _writer.WriteDataToCell(_ws, invoice.Amount.ToString("C"), _row, 6);
                    _writer.WriteDataToCell(_ws, invoice.Income.ToString("C"), _row, 7);
                    break;
            }

            _writer.WriteDataToCell(_ws, _counter.ToString(), _row, 1);
            if (repeatInvoiceCount(invoice.Number) > 1)
            {
                InvoiceMarker marker;

                if (_invoiceMarker.Count == 0)
                {
                    marker = new InvoiceMarker(invoice.Number);
                    _invoiceMarker.Add(marker);
                }
                else if (_invoiceMarker.FirstOrDefault(m => m.InvoiceId == invoice.Number) == null)
                {
                    marker = new InvoiceMarker(_invoiceMarker[^1], invoice.Number);
                    _invoiceMarker.Add(marker);
                }
                else
                {
                    marker = _invoiceMarker.FirstOrDefault(m => m.InvoiceId == invoice.Number);
                }
                _writer.WriteDataToCell(_ws, $"{marker}{invoice.CompanyFullName}", _row, 2);
            }
            else
            {
                _writer.WriteDataToCell(_ws, $"{invoice.CompanyFullName}", _row, 2);
            }
            _writer.WriteDataToRange(_ws, invoice.ObjectName, false, 11, false, XlHAlign.xlHAlignLeft, XlVAlign.xlVAlignTop, _row, 3, _row, 4);
            _writer.WriteDataToCell(_ws, invoice.Number, false, 11, XlHAlign.xlHAlignLeft, XlVAlign.xlVAlignTop, _row, 5);

            SetBodyComponentColor();

            _row++;
            _counter++;
        }

        private void PrintNonPayInvoice(List<string> nonPayInvoices, ref bool nonPayExists)
        {
            bool toNextPage = false;
            if (nonPayExists)
            {
                if (IsNewPage(1))
                {
                    _drawing.BordersAroundDraw(_ws, _startRow + 2, 1, _row - 1, 8, XlLineStyle.xlContinuous, XlBorderWeight.xlMedium, "#A6A6A6");
                    _row++;
                    SetNewBodyPage();
                }
                else if (IsNewPage())
                {
                    _drawing.BordersAroundDraw(_ws, _startRow + 2, 1, _row - 1, 8, XlLineStyle.xlContinuous, XlBorderWeight.xlMedium, "#A6A6A6");
                    SetNewBodyPage();
                }

                List<string> orderedList = nonPayInvoices.OrderBy(i => i.Split(" -- ", StringSplitOptions.RemoveEmptyEntries)[3] == "За кредитно" ? 1 :
                                            i.Split(" -- ", StringSplitOptions.RemoveEmptyEntries)[3] == "В брой" ? 2 :
                                            i.Split(" -- ", StringSplitOptions.RemoveEmptyEntries)[3] == "За анулиране" ? 3 :
                                            4).ToList();
                _drawing.BackgroundColorRange(_ws, XlRgbColor.rgbLightGrey, _row, 1, _row, 8);
                _writer.WriteDataToRange(_ws, "Неплатени", true, 11, false, XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter, _row, 1, _row, 8);
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

                    _writer.WriteDataToCell(_ws, _counter.ToString(), _row, 1);
                    _writer.WriteDataToCell(_ws, amountIncome[2], _row, 2);
                    _writer.WriteDataToCell(_ws, amountIncome[0], _row, 6);
                    _writer.WriteDataToCell(_ws, amountIncome[1], _row, 7);
                    _writer.WriteDataToRange(_ws, amountIncome[3], false, 11, false, XlHAlign.xlHAlignLeft, XlVAlign.xlVAlignTop, _row, 3, _row, 4);
                    _writer.WriteDataToCell(_ws, amountIncome[4], false, 11, XlHAlign.xlHAlignLeft, XlVAlign.xlVAlignTop, _row, 5);

                    SetBodyComponentColor();

                    _row++;
                    _counter++;
                    if (IsNewPage())
                    {
                        _drawing.BordersAroundDraw(_ws, _startRow + 2, 1, _row - 1, 8, XlLineStyle.xlContinuous, XlBorderWeight.xlMedium, "#A6A6A6");
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
            _writer.WriteDataToRange(_ws, "Отчет на парите", true, 14, false
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
                rowCount = banknotesCount / 2 + 1;
            }

            int rowStart = _row;

            int column = 2;
            _totalSum = 0;
            int counter = 1;

            foreach (var banknote in filteredBanknotes)
            {
                _totalSum += banknote.Key * banknote.Value;
                _writer.WriteDataToCell(_ws, $"{banknote.Value} -", false, 11,
                                        XlHAlign.xlHAlignRight, XlVAlign.xlVAlignBottom, _row, column);
                _writer.WriteDataToCell(_ws, banknote.Key.ToString("C"), _row, column + 1);
                _writer.WriteDataToCell(_ws, (banknote.Value * banknote.Key).ToString("C"), _row, column + 2);

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

            _writer.WriteDataToCell(_ws, "Тотал:", true, 11,
                XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter, _row, 6);
            _writer.WriteDataToRange(_ws, _totalSum.ToString("C"), true, 11, false
                , XlHAlign.xlHAlignLeft, XlVAlign.xlVAlignCenter, _row, 7, _row, 8);
            _drawing.BordersAroundDraw(_ws, rowStart - 1, 3, _row, 8, XlLineStyle.xlContinuous, XlBorderWeight.xlMedium, default);
            _row++;

            _writer.WriteDataToCell(_ws, "Преброена сума", false, 11,
                XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter, _row, 2);
            _writer.WriteDataToCell(_ws, "Разлика", false, 11,
                XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter, _row + 2, 2);
            _drawing.BordersAroundDraw(_ws, _row, 2, _row + 2, 2, XlLineStyle.xlContinuous, XlBorderWeight.xlMedium, default);
            _drawing.BordersAroundDraw(_ws, _row + 2, 2, _row + 3, 2, XlLineStyle.xlContinuous, XlBorderWeight.xlMedium, default);
        }

        private void BuildVehicleFooter()
        {
            _drawing.WriteDataToRange(_ws, "Превозно средство №:", false, 12, false
                , XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter
                , _row + 5, 2, _row + 5, 2);
            _drawing.WriteDataToRange(_ws, _dayReport.Vehicle, true, 12, false
                , XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter
                , _row + 6, 2, _row + 6, 2);
            _drawing.BordersAroundDraw(_ws, _row + 5, 2, _row + 6, 2, XlLineStyle.xlContinuous, XlBorderWeight.xlMedium, default);
        }

        private void BuildTotalsFooter()
        {
            _writer.WriteDataToRange(_ws, "Общо приходи от доставки: ", false, 12, false
                , XlHAlign.xlHAlignRight, XlVAlign.xlVAlignCenter
                , _row, 3, _row, 5);
            _writer.WriteDataToRange(_ws, _dayReport.TotalAmount.ToString("C"), false, 12, false
                , XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter
                , _row, 6, _row, 8);

            _writer.WriteDataToRange(_ws, "Общо приходи от стари сметки: ", false, 12, false
                , XlHAlign.xlHAlignRight, XlVAlign.xlVAlignCenter
                , _row + 1, 3, _row + 1, 5);
            _writer.WriteDataToRange(_ws, _dayReport.TotalOldInvoice.ToString("C"), false, 12, false
                , XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter
                , _row + 1, 6, _row + 1, 8);

            _writer.WriteDataToRange(_ws, "Сума от неплатени сметки: ", false, 12, false
                , XlHAlign.xlHAlignRight, XlVAlign.xlVAlignCenter
                , _row + 2, 3, _row + 2, 5);
            _writer.WriteDataToRange(_ws, _dayReport.TotalNonPay.ToString("C"), false, 12, false
                , XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter
                , _row + 2, 6, _row + 2, 8);

            _writer.WriteDataToRange(_ws, "Сума от документи по банка: ", false, 12, false
                , XlHAlign.xlHAlignRight, XlVAlign.xlVAlignCenter
                , _row + 3, 3, _row + 3, 5);
            _writer.WriteDataToRange(_ws, _dayReport.BankPayTotal().ToString("C"), false, 12, false
                , XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter
                , _row + 3, 6, _row + 3, 8);

            _writer.WriteDataToRange(_ws, "Разходи + КИ: ", false, 12, false
                , XlHAlign.xlHAlignRight, XlVAlign.xlVAlignCenter
                , _row + 4, 3, _row + 4, 5);
            _writer.WriteDataToRange(_ws, _dayReport.TotalExpenses.ToString("C"), false, 12, false
                , XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter
                , _row + 4, 6, _row + 4, 8);

            _writer.WriteDataToRange(_ws, "Тотал - Отчетена сума: ", true, 14, false
                , XlHAlign.xlHAlignRight, XlVAlign.xlVAlignCenter
                , _row + 5, 3, _row + 5, 5);
            _writer.WriteDataToRange(_ws, _dayReport.TotalIncome.ToString("C"), true, 14, false
                , XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter
                , _row + 5, 6, _row + 5, 8);

            if (_totalSum - _dayReport.TotalIncome < 0)
            {
                _writer.WriteDataToCell(_ws, "Задължение:", false, 12, XlHAlign.xlHAlignRight, XlVAlign.xlVAlignCenter, _row + 6, 5);
            }
            else
            {
                _writer.WriteDataToCell(_ws, "Ресто:", false, 12, XlHAlign.xlHAlignRight, XlVAlign.xlVAlignCenter, _row + 6, 5);
            }

            _writer.WriteDataToRange(_ws, Math.Abs(_totalSum - _dayReport.TotalIncome).ToString("C"), false, 12, false
                , XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter, _row + 6, 6, _row + 6, 8);

            _drawing.BordersAroundDraw(_ws, _row, 6, _row + 6, 8, XlLineStyle.xlContinuous, XlBorderWeight.xlMedium, default);
        }

        private void BuildPersonalInformationFooter()
        {
            _writer.WriteDataToRange(_ws, "Съставил отчета / Предал:", false, 11, false
                , XlHAlign.xlHAlignLeft, XlVAlign.xlVAlignBottom
                , _row + 7, 2, _row + 7, 2);
            _writer.WriteDataToRange(_ws, "Приел отчета:", false, 11, false
                , XlHAlign.xlHAlignLeft, XlVAlign.xlVAlignBottom
                , _row + 7, 4, _row + 7, 5);

            _writer.WriteDataToRange(_ws, _dayReport.EmployeeFullName, true, 16, false
                , XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignBottom
                , _row + 8, 2, _row + 9, 3);

            _writer.WriteDataToRange(_ws, "(име, фамилия, подпис)", false, 10, false
                , XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignBottom
                , _row + 10, 2, _row + 10, 3);
            _drawing.BorderDraw(_ws, _row + 10, 2, _row + 10, 3, XlLineStyle.xlContinuous, XlBorderWeight.xlThin, XlBordersIndex.xlEdgeTop);

            _writer.WriteDataToRange(_ws, "(име, фамилия, подпис)", false, 10, false
                , XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignBottom
                , _row + 10, 5, _row + 10, 8);
            _drawing.BorderDraw(_ws, _row + 10, 5, _row + 10, 8, XlLineStyle.xlContinuous, XlBorderWeight.xlThin, XlBordersIndex.xlEdgeTop);

            _writer.WriteDataToRange(_ws, "Обработил отчета:", false, 11, false
               , XlHAlign.xlHAlignLeft, XlVAlign.xlVAlignBottom
               , _row + 11, 4, _row + 11, 5);

            _writer.WriteDataToRange(_ws, "(име, фамилия, подпис)", false, 10, false
                , XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignBottom
                , _row + 14, 5, _row + 14, 8);
            _drawing.BorderDraw(_ws, _row + 14, 5, _row + 14, 8, XlLineStyle.xlContinuous, XlBorderWeight.xlThin, XlBordersIndex.xlEdgeTop);

            _drawing.WriteDataToRange(_ws, "Дата на предаване", false, 12, false
                , XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter
                , _row + 13, 2, _row + 13, 2);
            _drawing.WriteDataToRange(_ws, $"{_dayReport.TransmissionDate}", true, 12, true
                , XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter
                , _row + 14, 2, _row + 14, 2);
            _drawing.BordersAroundDraw(_ws, _row + 13, 2, _row + 14, 2, XlLineStyle.xlContinuous, XlBorderWeight.xlMedium, default);
        }

        private bool IsNotEnoughSpaceForFooterOnPage(int banknotesCount)
        {
            int rowRemaining = 51 - _row % 52;
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
            _ws = _wb.Worksheets[sheet];
            for (int i = 1; i <= columnSize.Length; i++)
            {
                _ws.Range[_ws.Cells[1, i], _ws.Cells[1, i]].ColumnWidth = columnSize[i - 1];
            }
        }
    }
}