using DelitaReturnProtocolProvider.Services;
using DelitaTrade.Models.ReturnProtocol;
using DelitaTrade.ViewModels;
using DelitaTrade.ViewModels.ReturnProtocolControllers;
using DelitaReturnProtocolProvider.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;

namespace DelitaTrade.Components.ComponentsViewModel.ReturnProtocolComponentViewModels
{
    public enum DataAction
    {
        Save,
        Update,
        Delete
    }
    public class ListViewInputViewModel : ViewModelBase
    {   
        private IServiceProvider _serviceProvider;
        
        private ProductViewModel _selectedProduct;
        private ReturnProtocolController _returnProtocolController;
        private ReturnProtocolViewModel _currentReturnProtocolViewModel;
        
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
            DescriptionCreate += OnDescriptionCreate;
            ProductCreate += AddProduct;
            DescriptionCreate += AddDescription;
            UpdateProduct();
            _returnProtocolController.ReturnProtocolSelected += InitializedList;
            _returnProtocolController.ReturnProtocolUnSelected += UnselectedProtocol;
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
        public void RemoveRow(ProductToReturnViewModel row) 
        {
            var service = _serviceProvider.GetService<ReturnProductService>()
                ?? throw new InvalidOperationException($"Service: {nameof(ReturnProductService)} not available");
                service.RemoveProductAsync(row.Id, _currentReturnProtocolViewModel.Id);
            _list.Remove(row);
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
            _products.Add(product);
        }

        private void AddDescription(ReturnedProductDescriptionViewModel description)
        {
            _description.Add(description);
        }

        private async void UpdateProduct()
        {
            var products = await _serviceProvider.GetService<ProductService>().GetAllAsync();
            _products.Clear();
            foreach (var prod in products)
            {
                _products.Add(prod);
            }                
            
            var descriptions = await _serviceProvider.GetService<ProductDescriptionService>().GetAllAsync();
            _description.Clear();
            foreach (var desc in descriptions)
            {
                _description.Add(desc);
            }
        }

        private void OnCreatedProduct(ProductViewModel product)
        {
            var service = _serviceProvider.GetService<ProductService>() ?? throw new InvalidOperationException($"Service {nameof(ProductService)} not available");
            service.AddProduct(product);
        }
        private void OnDescriptionCreate(ReturnedProductDescriptionViewModel description)
        {
            var service = _serviceProvider.GetService<ProductDescriptionService>() 
                ?? throw new InvalidOperationException($"Service {nameof(ProductDescriptionService)} not available");
            service.AddDescription(description);
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
