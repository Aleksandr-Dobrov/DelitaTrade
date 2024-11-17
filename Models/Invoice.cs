using DelitaTrade.Models.DataBases;
using DelitaTrade.Models.Interfaces.DataBase;
using System.Runtime.Serialization;

namespace DelitaTrade.Models
{
    [DataContract]
    public class Invoice : IDBData, ICloneable
    {
        private int _dayReportId;
        private int _id;
        private string _companyName;
        private string _companyType;
        private string _objectName;
        private string _invoiceID;
        private decimal _amount;
        private decimal _income;
        private string _payMethod;
        private double _weight;

        public Invoice(string companyName,string companyType, string objectName, string invoiceID,
                       string payMethod, decimal amount, decimal income, double weight)
        {
            _companyName = companyName;
            _companyType = companyType;
            _objectName = objectName;
            _invoiceID = invoiceID;
            _payMethod = payMethod;
            _amount = amount;
            _income = income;
            _weight = weight;            
        }

        public Invoice(int id, string companyName, string companyType, string objectName, string invoiceID,
                       string payMethod, decimal amount, decimal income, double weight)
            : this(companyName, companyType, objectName, invoiceID, payMethod, amount, income, weight)
        {
            _id = id;
        }
        

        public Invoice(int dayReportId, int id, string companyName, string companyType, string objectName, string invoiceID,
                       string payMethod, decimal amount, decimal income, double weight)
            : this(id ,companyName, companyType, objectName, invoiceID, payMethod, amount, income, weight)
        {
            _dayReportId = dayReportId;
        }

        public Invoice(int id)
        {
            _id = id;
        }

        public DayReport DayReport { get; set; }
        public string CompanyFullName => $"{_companyName} {_companyType}";
        public string CompanyName => _companyName;
        public string CompanyType => _companyType;
        public string ObjectName => _objectName;
        public string InvoiceID => _invoiceID;
        public decimal Amount => _amount;
        public decimal Income => _income;
        public string PayMethod => _payMethod;
        public double Weight => _weight;
        public int Id => _id;
        public int DayReportId => _dayReportId;

        public string Parameters => "id-=-user_name-=-day_report_id-=-new_invoice_id-=-new_company_name-=-new_company_type-=-" +
                                    "new_object_name-=-new_pay_method-=-new_amount-=-new_weight-=-invoice_Income";

        public string Data => $"{Id}-=-{DayReport.User}-=-{DayReport.DayReportID}-=-{InvoiceID}-=-{_companyName}-=-{_companyType}-=-{ObjectName}-=-{PayMethod}-=-{Amount}-=-{Weight}-=-{Income}";
        public string Procedure => "add_invioce_toDayReportdb";

        public int NumberOfAdditionalParameters => 0;

        public object Clone()
        {
            return new Invoice(DayReportId, Id, _companyName, _companyType, ObjectName, InvoiceID, PayMethod, Amount, Income, Weight);
        }

        public void Update(Invoice newInvoice)
        {
            _companyName = newInvoice.CompanyName;
            _companyType = newInvoice.CompanyType;
            _objectName = newInvoice.ObjectName;
            _amount = newInvoice.Amount;
            _income = newInvoice.Income;
            _payMethod = newInvoice.PayMethod;
            _weight = newInvoice.Weight;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override bool Equals(object? obj)
        {
            var invoice = obj as Invoice;
            return invoice.Id == Id;
        }

        public void SetId(DayReportDataService dayReportDataService)
        {
            _id = dayReportDataService.GetNextInvoiceId();
        }
    }
}
