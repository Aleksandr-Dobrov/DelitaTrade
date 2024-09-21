using System.Collections;
using System.Runtime.Serialization;
using DelitaTrade.Interfaces.ReturnProtocol;

namespace DelitaTrade.Models.ReturnProtocol
{
    [DataContract]
    public class ProductsDataBase : IEnumerable
    {
        [DataMember]
        private HashSet<ProductBase> _products;
        
        public event Action DataBaseChange;

        public ProductsDataBase()
        {
            _products = new HashSet<ProductBase>();
            DataBaseChange += () => { };            
        }
        public void AddProductToDataBase(IProduct product)
        {
            if (product != null)
            {
                if (product is ProductBase &&_products.Contains(product) == false)
                {
                    _products.Add(product as ProductBase);
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

        public bool Contains(IProduct product)
        {
            return _products.Contains(product);
        }
        
        public IEnumerator GetEnumerator()
        {
            yield return _products.GetEnumerator();
        }
    }
}
