using DelitaTrade.Models.Interfaces.DataBase;
using System.Runtime.Serialization;

namespace DelitaTrade.Models
{
    [DataContract]
    public class DayReport : IDBData, IDBDataId, ICloneable
    {
        const string _user = "Александр Добров";
        private readonly int _id;
        private readonly string _dayReportID;
        private List<Invoice> _invoices;
        private PayDesk _payDesk;
        private decimal _totalAmaunt;
        private decimal _totalIncome;
        private decimal _totalExpenses;
        private decimal _totalNonPay;
        private double _totalWeight;
        private decimal _totalOldInvoice;
        private string _vehicle;
        private string _transmissionDate;
        private int _payDeskId;

        public DayReport(string dayReportID)
        {
            _dayReportID = dayReportID;
            _invoices = new List<Invoice>();
            _payDesk = new PayDesk();
        }

        public DayReport(string dayReportID, int payDeskId) : this(dayReportID)
        {
            _payDeskId = payDeskId;
            _payDesk = new PayDesk(payDeskId);
        }

        public DayReport(string id, string dayReportID, decimal totalAmaunt, decimal totalIncome, decimal totalExpenses,
                        decimal totalNonPay, double totalWeight, decimal totalOldInvoice, string vehicle, 
                        string transmissionDate, int payDeskId) : this(dayReportID)
        {     
            _id = int.Parse(id);
            _totalAmaunt = totalAmaunt;
            _totalIncome = totalIncome;
            _totalExpenses = totalExpenses;
            _totalNonPay = totalNonPay;
            _totalWeight = totalWeight;
            _totalOldInvoice = totalOldInvoice;
            _vehicle = vehicle;
            _transmissionDate = transmissionDate;
            _payDeskId = payDeskId;
        }

        public DayReport(string id, string dayReportID, decimal totalAmaunt, decimal totalIncome, decimal totalExpenses,
                        decimal totalNonPay, double totalWeight, decimal totalOldInvoice, string vehicle,
                        string transmissionDate, int payDeskId, PayDesk payDesk, List<Invoice> invoices) :
                        this(id, dayReportID, totalAmaunt, totalIncome, totalExpenses, totalNonPay, totalWeight,
                             totalOldInvoice, vehicle, transmissionDate, payDeskId)
        {
            _payDesk = payDesk;
            _invoices = invoices;
        }

        public IDBData PayDesk => _payDesk;
        public PayDesk PayDeskBanknotes => _payDesk;
        public IEnumerable<IDBData> Invoices => _invoices;
        public string DayReportID => _dayReportID;

        public decimal TotalAmount => _totalAmaunt;

        public decimal TotalIncome => _totalIncome;

        public decimal TotalExpenses => _totalExpenses;

        public decimal TotalNonPay => _totalNonPay;

        public decimal TotalOldInvoice => _totalOldInvoice;

        public int InvoicesCount => _invoices.Count;

        public double TotalWeight => _totalWeight;

        public int PayDeskId => _payDeskId;

        public string User => _user;

        public int Id => _id;

        public string Vehicle
        {
            get => _vehicle;
            set
            {
                _vehicle = value;
            }
        }

        public string TransmissionDate
        {
            get => _transmissionDate == null ? "01-01-0001" : _transmissionDate;
            set
            {
                _transmissionDate = value;
            }
        }

        public string DBDataId => _dayReportID;
        public string Parameters => "day_report_user-=-day_report_date-=-day_report_transmission_date-=-day_report_vehicle-=-" +
            "day_report_total_amount-=-day_report_total_income-=-day_report_total_expenses-=-day_report_total_nonPay-=-" +
            "day_report_total_oldInvoice-=-day_report_total_weight-=-pay_desk_amount-=-pay_desk_banknotes";

        public string Data => $"{_user}-=-{DayReportID}-=-{string.Join('-', TransmissionDate.Split('-').Reverse())}-=-{Vehicle}-=-{TotalAmount}-=-" +
            $"{TotalIncome}-=-{TotalExpenses}-=-{TotalNonPay}-=-{TotalOldInvoice}-=-{TotalWeight}-=-{_payDesk.Amount}-=-{_payDesk.AllBankcote}";

        public string Procedure => "create_day_report";

        public int NumberOfAdditionalParameters => 1;

        public object Clone()
        {
            var clone = new DayReport(Id.ToString(), DayReportID, TotalAmount, TotalIncome, TotalExpenses, TotalNonPay, TotalWeight, TotalOldInvoice, Vehicle,
                                TransmissionDate, PayDeskId, (PayDesk)_payDesk.Clone(), CloneAllInvoice());
            foreach (var item in clone.GetAllInvoices())
            {
                item.DayReport = clone;
            }
            return clone;
        }

        public decimal BankPayTotal()
        {
            decimal _bankPayTotal = 0;
            foreach (var item in _invoices)
            {
                if (item.PayMethod == "Банка")
                {
                    _bankPayTotal += item.Amount;
                }
            }
            return _bankPayTotal;
        }

        public bool CheckMoneyStatus()
        {
            if (_payDesk.Amount >= TotalIncome)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsNonPayInvoice(Invoice invoice)
        {
            decimal totalIncome = 0;
            List<Invoice> invoices = _invoices.Where(i => i.InvoiceID == invoice.InvoiceID).ToList();
            foreach (var item in invoices)
            {
                totalIncome += item.Income;
            }
            return invoice.Amount > totalIncome;
        }

        public int GetId(string invoiceID)
        {
            if (_invoices.FirstOrDefault(i => i.InvoiceID == invoiceID) == null)
            {
                return 0;
            }
            List<Invoice> invoices = _invoices.Where(i => i.InvoiceID == invoiceID).ToList();
            return invoices.Count;
        }

        public void AddInvoice(Invoice invoice)
        {
            invoice.DayReport = this;                
            SumInvoice(invoice);
            _invoices.Add(invoice);
            TotalWeightCalcilate();
        }

        public void UpdateInvoice(Invoice invoice)
        {
            Invoice invoiceToUpdate = _invoices.FirstOrDefault(i => i.InvoiceID == invoice.InvoiceID && i.Id == invoice.Id);
            if (invoiceToUpdate != null)
            {
                _invoices.Remove(invoiceToUpdate);
                SubtractInvoice(invoiceToUpdate);
                invoiceToUpdate.Update(invoice);
                SumInvoice(invoiceToUpdate);
                _invoices.Add(invoiceToUpdate);
                TotalWeightCalcilate();
            }
            else
            {
                throw new ArgumentException($"Invoice with ID: {invoice.InvoiceID} not exists in this report.");
            }
        }

        public Invoice RemoveInvoice(string invoiceId, int id)
        {
            Invoice invoiceToRemove = _invoices.FirstOrDefault(i => i.InvoiceID == invoiceId && i.Id == id);
            if (invoiceToRemove != null)
            {
                _invoices.Remove(invoiceToRemove);
                SubtractInvoice(invoiceToRemove);
                TotalWeightCalcilate();
            }
            else
            {
                throw new ArgumentException($"Invoice with ID: {invoiceId} not exists in this report.");
            }
            return invoiceToRemove;
        }

        public void AddMoney(decimal value, int count)
        {
            _payDesk.AddMoney(value.ToString(), count);
        }

        public void RemoveMoney(decimal value, int count)
        {
            _payDesk.RemoveMonet(value.ToString(), count);
        }

        public void InitiateInvoices(Invoice[] invoices)
        {
            if (_invoices.Count == 0)
            {
                foreach (Invoice invoice in invoices)
                {
                    if (invoice.DayReportId == Id)
                    {
                        invoice.DayReport = this;
                        _invoices.Add(invoice);
                    }
                    else 
                    {
                        throw new ArgumentException("The invoice is not from this day report.");
                    }
                }                
            }
        }

        public IEnumerable<Invoice> GetAllInvoices()
        {
            return _invoices;
        }

        public IDictionary<decimal, int> GetAllBanknotes()
        {
            return _payDesk.GetAllBanknotes();
        }

        public void SetPayDesk(PayDesk payDesk)
        {
            if (payDesk == _payDesk)
            {
                return;
            }
            if (payDesk.Id == _payDeskId)
            {
                _payDesk = payDesk;
            }
            else
            {
                throw new ArgumentException("Pay desk is not from this day report.");
            }
        }

        public override int GetHashCode()
        {
            return DayReportID.GetHashCode();
        }

        public override bool Equals(object? obj)
        {
            var dayReport = obj as DayReport;
            return dayReport?.DayReportID == DayReportID;
        }

        private bool IsFirstInvoice(Invoice invoice)
        {
            return !_invoices.Where(i => i.InvoiceID == invoice.InvoiceID).Any();
        }

        private void SumInvoice(Invoice invoice)
        {
            switch (invoice.PayMethod)
            {
                case "Банка":
                    if (IsFirstInvoice(invoice))
                    {
                        _totalAmaunt += invoice.Amount;
                    }
                    break;
                case "В брой":
                case "За кредитно":
                    if (invoice.Amount > invoice.Income)
                    {
                        _totalNonPay += invoice.Amount - invoice.Income;
                    }
                    if (IsFirstInvoice(invoice))
                    {
                        _totalAmaunt += invoice.Amount;
                    }
                    _totalIncome += invoice.Income;
                    break;
                case "Стара сметка":
                    if (IsFirstInvoice(invoice))
                    {
                        _totalOldInvoice += invoice.Amount;
                    }
                    _totalIncome += invoice.Income;
                    break;
                case "С карта":
                    if (IsFirstInvoice(invoice))
                    {
                        _totalAmaunt += invoice.Amount;
                    }
                    break;
                case "Кредитно":
                    _totalExpenses += invoice.Income * -1;
                    _totalIncome += invoice.Income;
                    break;
                case "Разход":
                    _totalExpenses += invoice.Income * -1;
                    _totalIncome += invoice.Income;
                    break;
                default:
                    break;
            }

        }

        private void SubtractInvoice(Invoice invoice)
        {
            switch (invoice.PayMethod)
            {
                case "Банка":
                    if(IsFirstInvoice(invoice))
                    {
                        _totalAmaunt -= invoice.Amount;
                    }
                    break;
                case "В брой":
                case "За кредитно":
                    if (invoice.Amount > invoice.Income)
                    {
                        _totalNonPay -= invoice.Amount - invoice.Income;
                    }
                    if (IsFirstInvoice(invoice))
                    {
                        _totalAmaunt -= invoice.Amount;
                    }
                    _totalIncome -= invoice.Income;
                    break;
                case "Стара сметка":
                    if (IsFirstInvoice(invoice))
                    { 
                        _totalOldInvoice -= invoice.Amount;
                    }
                    _totalIncome -= invoice.Income;
                    break;
                case "С карта":
                    if (IsFirstInvoice(invoice))
                    {
                        _totalAmaunt -= invoice.Amount;
                    }
                    break;
                case "Кредитно":
                    _totalExpenses -= invoice.Income * -1;
                    _totalIncome -= invoice.Income;
                    break;
                case "Разход":
                    _totalExpenses -= invoice.Income * -1;
                    _totalIncome -= invoice.Income;
                    break;
                default:
                    break;
            }

        }

        private void TotalWeightCalcilate()
        {
            _totalWeight = 0;

            HashSet<string> invoicesId = new HashSet<string>();

            foreach (var invoice in _invoices)
            {
                if (invoicesId.Contains(invoice.InvoiceID) == false)
                { 
                    invoicesId.Add(invoice.InvoiceID);
                    _totalWeight += invoice.Weight;
                }
            }
        }

        private List<Invoice> CloneAllInvoice()
        {
            var result = new List<Invoice>();
            foreach (var invoice in _invoices)
            {
                result.Add((Invoice)invoice.Clone());
            }
            return result;
        }
    }
}
