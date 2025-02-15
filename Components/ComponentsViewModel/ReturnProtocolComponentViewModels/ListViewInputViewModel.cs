using DelitaTrade.ViewModels;
using DelitaTrade.Common.Constants;
using DelitaTrade.ViewModels.ReturnProtocolControllers;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using DelitaTrade.Core.Contracts;
using DelitaTrade.Core.ViewModels;

namespace DelitaTrade.Components.ComponentsViewModel.ReturnProtocolComponentViewModels
{
    public class ListViewInputViewModel : ViewModelBase
    {   
        private IServiceProvider _serviceProvider;
        
        private ProductViewModel _selectedProduct;
        private ReturnProtocolController _returnProtocolController;
        private ReturnProtocolViewModel? _currentReturnProtocolViewModel;
        
        private readonly ObservableCollection<ProductToReturnViewModel> _list;
        private ObservableCollection<string> _productUnit;
        private ObservableCollection<ProductViewModel> _products = new ObservableCollection<ProductViewModel>();
        private ObservableCollection<ReturnedProductDescriptionViewModel> _description = new ObservableCollection<ReturnedProductDescriptionViewModel>();

        public ListViewInputViewModel(IServiceProvider serviceProvider, ReturnProtocolController returnProtocolViewModel)
        {
            _serviceProvider = serviceProvider;
            _returnProtocolController = returnProtocolViewModel;
            _list = new ObservableCollection<ProductToReturnViewModel>();
            _productUnit = new ObservableCollection<string> { ProductUnit.Count, ProductUnit.Kg, ProductUnit.Box };
            ProductCreate += OnCreatedProduct;
            ProductCreate += AddProduct;
            DescriptionCreate += OnDescriptionCreate;
            DescriptionCreate += AddDescription;
            UpdateProduct();
            _returnProtocolController.ReturnProtocolSelected += InitializedList;
            _returnProtocolController.ReturnProtocolUnSelected += UnselectedProtocol;
            ReturnedProductCreate += AddReturnedProduct;
        }

        public ObservableCollection<ProductToReturnViewModel> List => _list;
        public ObservableCollection<ProductViewModel> ProductsList => _products;
        public ObservableCollection<ReturnedProductDescriptionViewModel> Descriptions => _description;
        public ObservableCollection<string> Unit => _productUnit;
        public event Action<ProductViewModel> ProductCreate;
        public event Action<ReturnedProductDescriptionViewModel> DescriptionCreate;
        public event Func<ReturnedProductViewModel, Task<int>> ReturnedProductCreate;

        public ProductViewModel SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                _selectedProduct = value;
            }
        }        

        public async void RemoveRow(ProductToReturnViewModel returnedProduct) 
        {
            using var scope = _serviceProvider.CreateScope();
            var service = scope.GetService<IReturnProductService>();
            await service.DeleteProductAsync(returnedProduct.Id);
            _list.Remove(returnedProduct);
        }

        private async Task<int> AddReturnedProduct(ReturnedProductViewModel returnProduct)
        {
            if (_currentReturnProtocolViewModel == null) throw new ArgumentNullException("No return protocol loaded");
            using var scope = _serviceProvider.CreateScope();
            var service = scope.GetService<IReturnProductService>();
            returnProduct.Id = await service.AddProductAsync(returnProduct, _currentReturnProtocolViewModel.Id);
            _currentReturnProtocolViewModel.Products.Add(returnProduct);
            return returnProduct.Id;
        }

        private async void AddRow()
        {
            if (_list.Count > 0)
            {
                var product = new ProductViewModel { Name = _list[^1].ProductName, Unit = _list[^1].Unit };
                var description = new ReturnedProductDescriptionViewModel(_list[^1].Description);
                _list[^1].IsCompleted -= AddRow;                
                ProductCreate(product);
                DescriptionCreate(description);
                _list[^1].Id = await ReturnedProductCreate(new ReturnedProductViewModel
                {
                    Batch = _list[^1].Batch,
                    Quantity = _list[^1].ProductQuantity,
                    BestBefore = _list[^1].BestBefore,
                    Product = product,
                    Description = description
                });
            }
            _list.Add(new("", this));
            _list[^1].IsCompleted += AddRow;            
        }
        private void InitializedList(ReturnProtocolViewModel returnProtocol)
        {
            if (returnProtocol.Products.Count > 0)
            {
                LoadList(returnProtocol);
            }
            else 
            {
                CreateNewList(returnProtocol);
            }
        }

        private void CreateNewList(ReturnProtocolViewModel returnProtocol)
        {
            _currentReturnProtocolViewModel = returnProtocol;
            _list.Clear();
            _list.Add(new("", this));
            _list[^1].IsCompleted += AddRow;
        }

        private void LoadList(ReturnProtocolViewModel returnProtocol)
        {
            _currentReturnProtocolViewModel = returnProtocol;
            _list.Clear();
            foreach (var item in returnProtocol.Products)
            {
                _list.Add(new ProductToReturnViewModel(item, this));
            }
            _list.Add(new("", this));
            _list[^1].IsCompleted += AddRow;
        }

        private void AddProduct(ProductViewModel product) 
        {
            if (_products.FirstOrDefault(p => p.Name == product.Name && p.Unit == product.Unit) == null)
            { 
                _products.Add(product);
            }
        }

        private void AddDescription(ReturnedProductDescriptionViewModel description)
        {
            if (_description.FirstOrDefault(d => d.Description == description.Description) == null)
            { 
                _description.Add(description);
            }
        }

        private async void UpdateProduct()
        {
            using var scope = _serviceProvider.CreateScope();
            var products = await scope.GetService<IProductService>().GetAllAsync();
            _products.Clear();
            foreach (var prod in products)
            {
                _products.Add(prod);
            }                
            
            var descriptions = await scope.GetService<IProductDescriptionService>().GetAllAsync();
            _description.Clear();
            foreach (var desc in descriptions)
            {
                _description.Add(desc);
            }
        }

        private async void OnCreatedProduct(ProductViewModel product)
        {
            using var scope = _serviceProvider.CreateScope();
            var service = scope.GetService<IProductService>();
            await service.AddProductAsync(product);
        }

        private async void OnDescriptionCreate(ReturnedProductDescriptionViewModel description)
        {
            using var scope = _serviceProvider.CreateScope();
            var service = scope.GetService<IProductDescriptionService>();
            description.Id = await service.AddDescription(description);
        }

        private void UnselectedProtocol()
        {
            if (_list.Count > 0)
            {
                _list[^1].IsCompleted -= AddRow;
                _list.Clear();
            }
            _currentReturnProtocolViewModel = null;
        }
    }
}
