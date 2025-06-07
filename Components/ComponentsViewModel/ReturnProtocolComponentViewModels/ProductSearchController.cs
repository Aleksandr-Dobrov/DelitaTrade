using DelitaTrade.Core.Contracts;
using DelitaTrade.Core.Services;
using DelitaTrade.Core.ViewModels;
using DelitaTrade.Infrastructure.Data.Models;
using DelitaTrade.ViewModels.Controllers;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace DelitaTrade.Components.ComponentsViewModel.ReturnProtocolComponentViewModels
{
    public class ProductSearchController
    {
        private readonly SearchComboBoxViewModel<ProductViewModel> _productSearchModel;
        private readonly SearchComboBoxDescriptionValidationViewModel<ReturnedProductDescriptionViewModel> _descriptionSearchModel;
        private readonly DescriptionCategoryController _descriptionCategoryController;
        private readonly IServiceProvider _serviceProvider;
        
        public ProductSearchController(IServiceProvider serviceProvider, DescriptionCategoryController descriptionCategoryController)
        {
            _productSearchModel = new SearchComboBoxViewModel<ProductViewModel>();
            _descriptionSearchModel = new SearchComboBoxDescriptionValidationViewModel<ReturnedProductDescriptionViewModel>();
            _descriptionCategoryController = descriptionCategoryController;
            _serviceProvider = serviceProvider;
            _productSearchModel.PropertyChanged += OnViewModelPropertyChange;
            _descriptionSearchModel.PropertyChanged += OnViewModelDescriptionPropertyChange;
        }

        public SearchComboBoxViewModel<ProductViewModel> ProductSearchModel => _productSearchModel;
        public SearchComboBoxDescriptionValidationViewModel<ReturnedProductDescriptionViewModel> DescriptionSearchModel => _descriptionSearchModel;
        public DescriptionCategoryController DescriptionCategoryController => _descriptionCategoryController;

        private async void OnViewModelPropertyChange(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ProductSearchModel.TextValue))
            {
                await LoadFilteredProducts(ProductSearchModel.TextValue);
            }
        }
        private async void OnViewModelDescriptionPropertyChange(object? sender, PropertyChangedEventArgs e)
        {           
            if (e.PropertyName == nameof(DescriptionSearchModel.TextValue))
            {
                await LoadFilteredDescriptions(DescriptionSearchModel.TextValue);
            }
        }

        private async Task LoadFilteredProducts(string filter)
        {
            using var scope = _serviceProvider.CreateScope();
            var productService = scope.GetService<IProductService>();

            var result = Task.Run(() => productService.GetFilteredProductsAsync(filter.Split(" ", StringSplitOptions.RemoveEmptyEntries)));
            
            ProductSearchModel.UpdateItems(await result);
        }

        private async Task LoadFilteredDescriptions(string filter)
        {
            using var scope = _serviceProvider.CreateScope();
            var productService = scope.GetService<IProductDescriptionService>();
            var products = await productService.GetFilteredDescriptions(filter.Split(" ", StringSplitOptions.RemoveEmptyEntries));

            DescriptionSearchModel.UpdateItems(products);
        }
    }
}
