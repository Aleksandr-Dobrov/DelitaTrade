using System.Collections;
using System.IO;

namespace DelitaTrade.Models.ReturnProtocol
{
    public class ProductsDataService : IEnumerable
    {
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

        public void AddProduct(Product product)
        {
            _productsData.AddProductToDataBase(product);
            _productsDataBase.SaveAllData(_productsData);
        }

        public IEnumerator GetEnumerator()
        {
            return  _productsData.GetEnumerator();
        }
    }
}
