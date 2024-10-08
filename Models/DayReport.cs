using System.Runtime.Serialization;

namespace DelitaTrade.Models
{
    [DataContract]
    public class DayReport
    {
        [DataMember]
        private readonly string _dayReportID;
        [DataMember]
        private List<Invoice> _invoices;
        [DataMember]
        private PayDesk _payDesk;
        [DataMember]
        private decimal _totalAmaunt;
        [DataMember]
        private decimal _totalIncome;
        [DataMember]
        private decimal _totalExpenses;
        [DataMember]
        private decimal _totalNonPay;
        [DataMember]
        private double _totalWeight;
        [DataMember]
        private decimal _totalOldInvoice;
        [DataMember]
        private string _vehicle;
        [DataMember]
        private string _transmissionDate;

        public DayReport(string dayReportID)
        {
            _dayReportID = dayReportID;
            _invoices = new List<Invoice>();
            _payDesk = new PayDesk();
        }

        public string DayReportID => _dayReportID;

        public decimal TotalAmaunt => _totalAmaunt;

        public decimal TotalIncome => _totalIncome;

        public decimal TotalExpenses => _totalExpenses;

        public decimal TotalNonPay => _totalNonPay;

        public decimal TotalOldInvoice => _totalOldInvoice;

        public int InvoicesCount => _invoices.Count;

        public double TotalWeight => _totalWeight;

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
            get => _transmissionDate;
            set
            {
                _transmissionDate = value;
            }
        }

        public bool CalculateAmount()
        {
            foreach (var invoice in _invoices)
            {
                _totalAmaunt += invoice.Amount;
            }
            return false;
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
            _invoices.Add(invoice);
            SumInvoice(invoice);
            TotalWeightCalcilate();
        }

        public void UpdateInvoice(Invoice invoice)
        {
            Invoice invoiceToUpdate = _invoices.FirstOrDefault(i => i.InvoiceID == invoice.InvoiceID && i.Id == invoice.Id);
            if (invoiceToUpdate != null)
            {
                _invoices.Remove(invoiceToUpdate);
                SubtractInvoice(invoiceToUpdate);
                _invoices.Add(invoice);
                SumInvoice(invoice);
                TotalWeightCalcilate();
            }
            else
            {
                throw new ArgumentException($"Invoice with ID: {invoice.InvoiceID} not exists in this report.");
            }
        }

        public void RemoveInvoice(string invoiceId, int id)
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
        }

        public void AddMoney(decimal value, int count)
        {
            _payDesk.AddMoney(value.ToString(), count);
        }

        public void RemoveMoney(decimal value, int count)
        {
            _payDesk.RemoveMonet(value.ToString(), count);
        }

        public IEnumerable<Invoice> GetAllInvoices()
        {
            return _invoices;
        }

        public IDictionary<decimal, int> GetAllBanknotes()
        {
            return _payDesk.GetAllBanknotes();
        }

        private bool IsFirstInvoice(Invoice invoice)
        {
            return invoice.Id == 0;
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
                    _totalOldInvoice += invoice.Amount;
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
                    _totalOldInvoice -= invoice.Amount;
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

            foreach (var invoice in _invoices)
            {
                if (invoice.Id == 0)
                {
                    _totalWeight += invoice.Weight;
                }
            }
        }
    }
}
