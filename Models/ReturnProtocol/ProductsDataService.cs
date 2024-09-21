using System.Collections;
using System.IO;
using System.Runtime.Serialization;
using DelitaTrade.Interfaces.ReturnProtocol;

namespace DelitaTrade.Models.ReturnProtocol
{
    public class ProductsDataService : IEnumerable
    {
        [DataMember]
        private ProductsDataBase _productsData;

        private IDataBase<ProductsDataBase> _productsDataBase;

        private string _productsDataBaseFilePath;

        public event Action ProductsDataChange;

        public ProductsDataService(string dataBaseFilePath)
        {
            _productsDataBase = new XmlDataBase<ProductsDataBase>();
            _productsDataBaseFilePath = dataBaseFilePath;
            _productsDataBase.Path = _productsDataBaseFilePath;
            TryLoadDataBase();
            _productsData.DataBaseChange += SaveDataBase;
            ProductsDataChange += () => { };
            _productsData.DataBaseChange += ProductDataChanged;
        }

        private void ProductDataChanged()
        {
            ProductsDataChange.Invoke();
        }

        private void TryLoadDataBase()
        {
            if (File.Exists(_productsDataBaseFilePath))
            {
                _productsData = _productsDataBase.LoadAllData();
            }
            else
            {
                _productsData = new ProductsDataBase();
            }
        }

        private void SaveDataBase()
        {
            _productsDataBase.SaveAllData(_productsData);
        }

        public void AddProduct(IProduct product)
        {
            _productsData.AddProductToDataBase(product);           
        }

        public bool TryAddProduct(IProduct product)
        {
            if (_productsData.Contains(product) == false)
            {
                _productsData.AddProductToDataBase(product);
                _productsDataBase.SaveAllData(_productsData);
                return true;
            }
            else
            {
                return false;
            }
        }

        public IEnumerator GetEnumerator()
        {
            return  _productsData.GetEnumerator();
        }
    }
}
