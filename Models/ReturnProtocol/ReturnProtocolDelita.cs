using DelitaTrade.Interfaces.ReturnProtocol;
using System.Collections;
using System.Runtime.Serialization;

namespace DelitaTrade.Models.ReturnProtocol
{
    [DataContract]
    public class ReturnProtocolDelita : IEnumerable, IReturnProtocolData
    {
        [DataMember]
        private readonly string _id;
        [DataMember]
        private string _companyFullName;
        [DataMember]
        private string _objectName;
        [DataMember]
        private string _objectAddress;
        [DataMember]
        private string _userName;
        [DataMember]
        private bool _bankPay;
        [DataMember]
        private string _payMethod;

        [DataMember]
        private string _trader;
        [DataMember]
        private DateOnly _date;

        [DataMember]
        private List<Product> _products;

        public ReturnProtocolDelita(ICompany company, ICompanyObject companyObject, string userName, string id)
        {
            _companyFullName = company.FullName;
            _objectName = companyObject.Name;
            _objectAddress = companyObject.Adrress;
            _bankPay = companyObject.BankPay;
            _userName = userName;
            _id = id;
            _date = DateOnly.Parse(DateTime.Now.ToString("dd-MM-yyyy"));
            _products = new List<Product>();
            _trader = companyObject.Trader;
        }

        public ReturnProtocolDelita(ICompany company, ICompanyObject companyObject, string userName, string id, string trader)
                             : this(company, companyObject, userName, id)
        {
            _trader = trader;
        }

        public string ID => _id;
        public string CompanyFullName => _companyFullName;
        public string ObjectName => _objectName;
        public string ObjectAddress => _objectAddress;
        public string UserName => _userName;
        public string DateString => _date.ToString("dd-MM-yyyy");
        public string Trader
        {
            get => _trader;
            set => _trader = value;
        }

        public DateOnly Date 
        {
            get => _date;
            //ToDo - do date validation.
            set => _date = value;
        }
        public string PayMethod
        {
            get => _payMethod;
            set => _payMethod = value;
        }

        public void AddProduct(Product product)
        {
            _products.Add(product);
        }

        public void RemoveProduct(Product product) 
        {
            _products.Remove(product);
        }

        public void UpdateProduct(Product productToUpdate, Product updatedProduct)
        {
            if (_products.Remove(productToUpdate))
            {
                _products.Add(updatedProduct);
            }
            else 
            {
                throw new ArgumentException($"{productToUpdate} not exists in list.");
            }
        }
                
        public IEnumerator GetEnumerator()
        {
            yield return _products.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
