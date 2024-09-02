using System.Runtime.Serialization;

namespace DelitaTrade.Models
{
    [DataContract]
    public class Invoice
    {
        [DataMember]
        private string _companyName;
        [DataMember]
        private string _objectName;
        [DataMember]
        private string _invoiceID;
        [DataMember]
        private decimal _amount;
        [DataMember]
        private decimal _income;
        [DataMember]
        private string _payMethod;
        [DataMember]
        private double _weight;

        public Invoice(string companyName, string objectName, string invoiceID,
                       string payMethod, decimal amount, decimal income, double weight)
        {
            _companyName = companyName;
            _objectName = objectName;
            _invoiceID = invoiceID;
            _payMethod = payMethod;
            _amount = amount;
            _income = income;
            _weight = weight;
        }

        public string CompanyName => _companyName;
        public string ObjectName => _objectName;
        public string InvoiceID => _invoiceID;
        public decimal Amount => _amount;
        public decimal Income => _income;
        public string PayMethod => _payMethod;
        public double Weight => _weight;

    }
}
