using DelitaTrade.ViewModels;
using DelitaTrade.Common.Constants;
using DelitaTrade.ViewModels.ReturnProtocolControllers;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using DelitaTrade.Core.Contracts;
using DelitaTrade.Core.ViewModels;
using DelitaTrade.ViewModels.Controllers;
using System.Threading.Tasks;

namespace DelitaTrade.Components.ComponentsViewModel.ReturnProtocolComponentViewModels
{
    public class ListViewInputViewModel : ViewModelBase
    {   
        private IServiceProvider _serviceProvider;
        
        private ProductViewModel _selectedProduct;
        private ReturnProtocolController _returnProtocolController;
        private ReturnProtocolViewModel? _currentReturnProtocolViewModel;
        private readonly DescriptionCategoryController _descriptionCategoryController;
        
        private readonly ObservableCollection<ProductToReturnViewModel> _list;
        private ObservableCollection<string> _productUnit;

        public ListViewInputViewModel(IServiceProvider serviceProvider, ReturnProtocolController returnProtocolViewModel, DescriptionCategoryController descriptionCategoryController)
        {
            _serviceProvider = serviceProvider;
            _returnProtocolController = returnProtocolViewModel;
            _descriptionCategoryController = descriptionCategoryController;
            _list = new ObservableCollection<ProductToReturnViewModel>();
            _productUnit = new ObservableCollection<string> { ProductUnit.Count, ProductUnit.Kg, ProductUnit.Box, ProductUnit.Liter, ProductUnit.Pack, ProductUnit.Meter };
            ProductCreate += OnCreatedProduct;
            DescriptionCreate += OnCreateDescription;
            _returnProtocolController.ReturnProtocolSelected += InitializedList;
            _returnProtocolController.ReturnProtocolUnSelected += UnselectedProtocol;
            ReturnedProductCreate += AddReturnedProduct;
        }

        public ObservableCollection<ProductToReturnViewModel> List => _list;
        public DescriptionCategoryController DescriptionCategoryController => _descriptionCategoryController;
        public ObservableCollection<string> Unit => _productUnit;
        public event Func<ProductViewModel, Task> ProductCreate;
        public event Func<ReturnedProductDescriptionViewModel, Task<ReturnedProductDescriptionViewModel>> DescriptionCreate;
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
            if (_currentReturnProtocolViewModel == null) throw new ArgumentNullException("No return protocol loaded");
            using var scope = _serviceProvider.CreateScope();
            var service = scope.GetService<IReturnProductService>();
            await service.DeleteProductAsync(returnedProduct.Id);
            _currentReturnProtocolViewModel.RemoveProductById(returnedProduct.Id);
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

        private async Task UpdateRow(ReturnedProductViewModel returnedProduct)
        {
            if (_currentReturnProtocolViewModel == null) throw new ArgumentNullException("No return protocol loaded");
            using var scope = _serviceProvider.CreateScope();
            var service = scope.GetService<IReturnProductService>();
            await service.UpdateProductAsync(returnedProduct);
        }

        private async void AddRow()
        {
            if (_list.Count > 0)
            {
                var product = new ProductViewModel { Name = _list[^1].ProductName, Unit = _list[^1].Unit, Number = _list[^1].Number };
                ReturnedProductDescriptionViewModel? description = null;
                ReturnedProductDescriptionViewModel? resultDescription = null;
                if (string.IsNullOrEmpty(_list[^1].Description) == false)
                {
                    description = new ReturnedProductDescriptionViewModel(_list[^1].Description!);
                    resultDescription = await DescriptionCreate(description);
                }
                _list[^1].IsCompleted -= AddRow;
                var t = ProductCreate(product);
                await t;
                ReturnedProductViewModel newProduct = new ReturnedProductViewModel
                {
                    Batch = _list[^1].Batch,
                    Quantity = _list[^1].ProductQuantity > 0 ? _list[^1].ProductQuantity : throw new ArgumentException("Product quantity must be greater than 0"),
                    BestBefore = _list[^1].BestBefore ?? throw new ArgumentNullException("Best before property is required"),
                    Product = product,
                    Description = resultDescription,
                    DescriptionCategory = _list[^1].DescriptionCategory ?? throw new ArgumentNullException("Description category is required"),
                };
                newProduct.Id = await ReturnedProductCreate(newProduct);
                _list[^1].Id = newProduct.Id;
                _list[^1].SetCreatedProduct(newProduct);
                _list[^1].UpdateProduct += OnProductUpdate;
            }
            _list.Add(new(this, _serviceProvider.GetRequiredService<ProductSearchController>()));
            _list[^1].IsCompleted += AddRow;            
        }

        private async void OnProductUpdate(ProductToReturnViewModel model)
        {
            var productToUpdate = _currentReturnProtocolViewModel!.Products.FirstOrDefault(x => x.Id == model.Id);
            if (productToUpdate == null) throw new ArgumentNullException("Product to update not found");

            var product = new ProductViewModel { Name = model.ProductName, Unit = model.Unit, Number = model.Number };
            ReturnedProductDescriptionViewModel? description = null;
            ReturnedProductDescriptionViewModel? resultDescription = null;
            if (string.IsNullOrWhiteSpace(model.Description) == false)
            {
                description = new ReturnedProductDescriptionViewModel(model.Description);
                resultDescription = await DescriptionCreate(description);
            }
            var t = ProductCreate(product);
            await t;

            productToUpdate.Batch = model.Batch;
            productToUpdate.Quantity = model.ProductQuantity;
            productToUpdate.BestBefore = model.BestBefore ?? throw new ArgumentNullException("Best before date is required");
            productToUpdate.Product = product;
            productToUpdate.Description = resultDescription;
            productToUpdate.DescriptionCategory = model.DescriptionCategory ?? throw new ArgumentNullException("Description category is required");

            await UpdateRow(productToUpdate);
        }

        private void InitializedList(ReturnProtocolViewModel returnProtocol)
        {
            UnselectedProtocol();

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
            _list.Add(new(this, _serviceProvider.GetRequiredService<ProductSearchController>()));
            _list[^1].IsCompleted += AddRow;
        }

        private void LoadList(ReturnProtocolViewModel returnProtocol)
        {
            _currentReturnProtocolViewModel = returnProtocol;
            _list.Clear();
            foreach (var item in returnProtocol.Products)
            {
                _list.Add(new ProductToReturnViewModel(item, this, _serviceProvider.GetRequiredService<ProductSearchController>()));
                _list[^1].UpdateProduct += OnProductUpdate;
            }
            _list.Add(new(this, _serviceProvider.GetRequiredService<ProductSearchController>()));
            _list[^1].IsCompleted += AddRow;
        }

        private async Task OnCreatedProduct(ProductViewModel product)
        {
            using var scope = _serviceProvider.CreateScope();
            var service = scope.GetService<IProductService>();
            await service.AddProductAsync(product);
        }

        private async Task<ReturnedProductDescriptionViewModel> OnCreateDescription(ReturnedProductDescriptionViewModel description)
        {
            using var scope = _serviceProvider.CreateScope();
            var service = scope.GetService<IProductDescriptionService>();
            return await service.AddDescriptionAsync(description);
        }

        private void UnselectedProtocol()
        {
            if (_list.Count > 0)
            {
                for (int i = 0; i < _list.Count - 1; i++)
                {
                    _list[i].UpdateProduct -= OnProductUpdate;
                }

                _list[^1].IsCompleted -= AddRow;
                _list.Clear();
            }
            _currentReturnProtocolViewModel = null;
        }
    }
}
