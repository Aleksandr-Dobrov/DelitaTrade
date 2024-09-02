using System.Runtime.Serialization;

namespace DelitaTrade.Models.ReturnProtocol
{
    [DataContract]
    public class ProductsNameUnitDataBase
    {
        [DataMember]
        private HashSet<ProductNameUnit> _products;

        public event Action DataBaseChange;

        public ProductsNameUnitDataBase()
        {
            DataBaseChange += () => { };
        }
        public void AddProductToDataBase(ProductNameUnit product)
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
                    throw new ArgumentException("Product nameUnit is already exists");
                }
            }
            else 
            {
                throw new ArgumentNullException("Product nameUnit is null");
            }
        }

        public IEnumerable<ProductNameUnit> GetProducts() 
        {
            return _products;
        }
    }
}
