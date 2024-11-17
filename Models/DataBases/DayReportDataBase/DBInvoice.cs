using DelitaTrade.Models.Interfaces.DataBase;
using System.Runtime.Serialization;

namespace DelitaTrade.Models.DataBases.DayReportDataBase
{
    public class DBInvoice : IDBData
    {
        private string _invoiceID;
        private string _companyName;
        private string _companyType;
        private string _objectName;
        private decimal _amount;
        private decimal _weight;

        public DBInvoice(string invoiceID, string companyName, string companyType, 
                         string objectName, decimal amount, decimal weight)
        {
            _invoiceID = invoiceID;
            _companyName = companyName;
            _companyType = companyType;
            _objectName = objectName;
            _amount = amount;
            _weight = weight;
        }

        public string CompanyName => _companyName;
        public string CompanyType => _companyType;
        public string ObjectName => _objectName;
        public string InvoiceID => _invoiceID;
        public decimal Amount => _amount;
        public decimal Weight => _weight;

        public string Parameters => "new_invoice_id-=-new_company_name-=-new_company_type-=-new_object_name-=-new_amount-=-new_weight";
        
        public string Data => $"{InvoiceID}-=-{CompanyName}-=-{CompanyType}-=-{ObjectName}-=-{Amount}-=-{Weight}";

        public string Procedure => "add_invoice_full";

        public int NumberOfAdditionalParameters => 0;
    }
}
