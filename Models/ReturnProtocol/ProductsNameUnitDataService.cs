using System.IO;

namespace DelitaTrade.Models.ReturnProtocol
{
    public class ProductsNameUnitDataService
    {
        private ProductsNameUnitDataBase _productsData;

        private IDataBase<ProductsNameUnitDataBase> _productsDataBase;

        private string _productsDataBaseFilePath;

        public event Action ProductsDataChange;

        public ProductsNameUnitDataService(string dataBaseFilePath)
        {
            _productsDataBase = new XmlDataBase<ProductsNameUnitDataBase>();
            _productsDataBaseFilePath = dataBaseFilePath;
            _productsDataBase.Path = _productsDataBaseFilePath;
            _productsData = TryLoadDataBase();
            _productsData.DataBaseChange += SaveDataBase;
            ProductsDataChange += () => { };
            _productsData.DataBaseChange += ProductDataChanged;
        }

        private void ProductDataChanged()
        {
            ProductsDataChange.Invoke();
        }

        private ProductsNameUnitDataBase TryLoadDataBase()
        {
            if (File.Exists(_productsDataBaseFilePath))
            {
                return _productsDataBase.LoadAllData();
            }
            else
            {
                return new ProductsNameUnitDataBase();
            }
        }

        private void SaveDataBase()
        {
            _productsDataBase.SaveAllData(_productsData);
        }

        public void AddProduct(ProductNameUnit product)
        {
            _productsData.AddProductToDataBase(product);
            _productsDataBase.SaveAllData(_productsData);
        }

        public IEnumerable<ProductNameUnit> GetProducts() 
        {
            return _productsData.GetProducts();
        }
    }
}
