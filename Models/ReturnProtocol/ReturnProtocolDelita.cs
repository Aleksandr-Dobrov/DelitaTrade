using System.Collections;
using System.Runtime.Serialization;

namespace DelitaTrade.Models.ReturnProtocol
{
    [DataContract]
    public class ReturnProtocolDelita : IEnumerable
    {
        [DataMember]
        private readonly string _id;
        [DataMember]
        private readonly string _companyName;
        [DataMember]
        private readonly string _objectName;
        [DataMember]
        private readonly string _objectAddress;
        [DataMember]
        private readonly string _userName;

        [DataMember]
        private string _trader;
        [DataMember]
        private DateOnly _date;

        [DataMember]
        private List<Product> _products;

        public ReturnProtocolDelita(Company company, CompanyObject companyObject, string userName, string id)
        {
            _companyName = $"{company.Name} {company.Type}";
            _objectName = companyObject.Name;
            _objectAddress = companyObject.Adrress;
            _userName = userName;
            _id = id;
            _date = DateOnly.Parse(DateTime.Now.ToString("dd-MM-yyyy"));
            _products = new List<Product>();
        }

        public string ID => _id;
        public string CompanyName => _companyName;
        public string ObjectName => _objectName;
        public string ObjectAddress => _objectAddress;
        public string UserName => _userName;
        public string Trader => _trader;
        public DateOnly Date => _date;

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
