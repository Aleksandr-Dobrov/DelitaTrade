using System.Collections;
using System.Runtime.Serialization;

namespace DelitaTrade.Models.ReturnProtocol
{
    [DataContract]
    public class ProductsDataBase : IEnumerable
    {
        [DataMember]
        private HashSet<Product> _products;
        
        public event Action DataBaseChange;

        public ProductsDataBase()
        {
            DataBaseChange += () => { };            
        }
        public void AddProductToDataBase(Product product)
        {
            if (product != null)
            {
                if (_products.Contains(product) == false)
                {
                    _products.Add(product);
                    DataBaseChange.Invoke();
                }
                else 
                {
                    throw new ArgumentException("Product is already exists");
                }
            }
            else 
            {
                throw new ArgumentNullException("Product is null");
            }
        }
        
        public IEnumerator GetEnumerator()
        {
            yield return _products.GetEnumerator();
        }
    }
}
