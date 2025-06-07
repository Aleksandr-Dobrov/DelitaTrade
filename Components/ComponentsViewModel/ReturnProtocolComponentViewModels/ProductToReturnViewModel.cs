using DelitaTrade.ViewModels;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using DelitaTrade.Core.ViewModels;
using DelitaTrade.Models.Loggers;
using DelitaTrade.Commands;
using System.ComponentModel.DataAnnotations;
using DelitaTrade.Components.ComponentsViewModel.ErrorComponents;
using DelitaTrade.Common;

namespace DelitaTrade.Components.ComponentsViewModel.ReturnProtocolComponentViewModels
{
    public class ProductToReturnViewModel : ValidationViewModel
    {
        private const string _buttonAddContent = "➕";
        private const string _buttonAddColor = "#FF4CAF50"; // Green color
        private const string _buttonContentDelete = "✖";
        private const string _buttonColorDelete = "#FF0000"; // Red color
        private const string _buttonContentUpdate = "🔄";
        private const string _buttonColorUpdate = "#FFFFBD00"; // light yellow color

        private string _buttonContent = _buttonAddContent;
        private string _buttonColor = _buttonAddColor;

        private int _id;
        private double _quantity;
        private string _unit = string.Empty;
        private string _batch = string.Empty;
        private DateTime? _bestBefore = DateTime.Now;
        private bool _dateError = false;

        private ReturnedProductViewModel? _product;
        private readonly ProductSearchController _productController;
        private DescriptionCategoryViewModel? _descriptionCategory;
        private ListViewInputViewModel _viewModel;        
        private Visibility _isVisible = Visibility.Collapsed;

        public ProductToReturnViewModel(ListViewInputViewModel viewModel, ProductSearchController productController)
        {
            _viewModel = viewModel;
            _productController = productController;
            IsCompleted += OnComplete;
            PropertyChanged += OnViewModelPropertyChange;
            ProductSearchController.DescriptionSearchModel.PropertyChanged += OnViewModelPropertyChange;
            ProductSearchController.ProductSearchModel.PropertyChanged += OnViewModelPropertyChange;
            DeleteProductCommand = new NotAsyncDefaultCommand(RowCommand, IsButtonEnabled,
                [this, ProductSearchController.ProductSearchModel, ProductSearchController.DescriptionSearchModel], 
                nameof(IsRowChanged),
                nameof(ProductName),
                nameof(ProductQuantity), 
                nameof(BestBefore),
                nameof(Batch),
                nameof(DescriptionCategory),
                nameof(ProductSearchController.ProductSearchModel.TextValue),
                nameof(ProductSearchController.DescriptionSearchModel.TextValue),
                nameof(HasErrors));
            ProductSearchController.ProductSearchModel.Value.ValueChanged += OnProductNamePropertyChange;
            ProductSearchController.DescriptionSearchModel.Value.ValueChanged += OnPropertyDescriptionChange;
            Validate(this, nameof(StringProductQuantity));
            Validate(this, nameof(Batch));
            Validate(this, nameof(Unit));
            Validate(this, nameof(DescriptionCategory));
            IsCompleted += () => { };
            UpdateProduct += (product) => { };
        }

        public ProductToReturnViewModel(ReturnedProductViewModel product, ListViewInputViewModel viewModel, ProductSearchController productController)
        {
            productController.DescriptionCategoryController.LoadComplete += DescriptionCategoryLoadCompleteCallBack;
            _product = product;
            _productController = productController;            
            _viewModel = viewModel;
            Id = product.Id;
            ProductName = product.Product.Name;
            Unit = product.Product.Unit;
            ProductQuantity = product.Quantity;
            Batch = product.Batch;
            BestBefore = product.BestBefore;
            Description = product.Description?.Description;
            DescriptionCategory = product.DescriptionCategory;
            PropertyChanged += OnViewModelPropertyChange;
            ProductSearchController.DescriptionSearchModel.PropertyChanged += OnViewModelPropertyChange;
            ProductSearchController.ProductSearchModel.PropertyChanged += OnViewModelPropertyChange;
            DeleteProductCommand = new NotAsyncDefaultCommand(RowCommand, IsButtonEnabled,
                [this, ProductSearchController.ProductSearchModel, ProductSearchController.DescriptionSearchModel],
                nameof(IsRowChanged),
                nameof(ProductName),
                nameof(ProductQuantity),
                nameof(BestBefore),
                nameof(Batch),
                nameof(DescriptionCategory),
                nameof(ProductSearchController.ProductSearchModel.TextValue),
                nameof(ProductSearchController.DescriptionSearchModel.TextValue),
                nameof(HasErrors));
            ProductSearchController.ProductSearchModel.Value.ValueChanged += OnProductNamePropertyChange;
            ProductSearchController.DescriptionSearchModel.Value.ValueChanged += OnPropertyDescriptionChange;
            OnComplete();
            IsCompleted += () => { };
            UpdateProduct += (product) => { };
        }

        public event Action IsCompleted;
        public event Action<ProductToReturnViewModel> UpdateProduct;    

        public int Id 
        {
            get => _id;
            set => _id = value;
        }

        public string ProductName
        {
            get { return _productController.ProductSearchModel.TextValue; }
            set 
            { 
                _productController.ProductSearchModel.TextValue = value;
                OnPropertyChange();               
            }
        }

        public string? Number { get; set; }

        public double ProductQuantity 
        {
            get { return _quantity; }
            set
            {
                _quantity = value;
                OnPropertyChange();
            }
        }

        [Range(typeof(double), "0.01", "1000000", ErrorMessage = "Product quantity must be greater than 0")]
        public string? StringProductQuantity
        {
            get { return _quantity.ToString(); }
            set
            {
                if (double.TryParse(value, out double result) && result > 0)
                {
                    ProductQuantity = result;
                }
                else
                {
                    ProductQuantity = 0;
                }
                OnPropertyChange();
            }
        }

        [Required(ErrorMessage = "Unit is required")]
        public string Unit
        {
            get { return _unit; }
            set
            {
                _unit = value;
                OnPropertyChange();
            }
        }

        [Required(ErrorMessage = "Batch is required")]
        [MinLength(2, ErrorMessage = "Batch is to short")]
        [MaxLength(ValidationConstants.ReturnedProductBatchMaxLength, ErrorMessage = $"Batch is to long")]
        public string Batch
        {
            get { return _batch; }
            set
            {
                _batch = value;
                OnPropertyChange();
            }
        }

        [Required(ErrorMessage = "Best before date is required")]
        public DateTime? BestBefore
        {
            get { return _bestBefore; }
            set
            {
                if (value == null)
                {
                    _dateError = true;
                }
                else
                {
                    _dateError = false;
                }
                _bestBefore = value;
                OnPropertyChange();
            }
        }
        public string? Description
        {
            get { return _productController.DescriptionSearchModel.TextValue; }
            set
            {
                if (value == null)
                {
                    _productController.DescriptionSearchModel.TextValue = string.Empty;
                }
                else
                {
                    _productController.DescriptionSearchModel.TextValue = value;
                }
                OnPropertyChange();
            }
        }

        [Required(ErrorMessage = "Description is required")]
        public DescriptionCategoryViewModel? DescriptionCategory
        {
            get => ProductSearchController.DescriptionCategoryController.SelectedDescriptionCategory; 
            set
            {
                ProductSearchController.DescriptionCategoryController.SelectedDescriptionCategory = value;
                OnPropertyChange(nameof(DescriptionCategory));
            }
        }

        public Visibility IsVisible
        {
            get => _isVisible;
            set 
            {
                _isVisible = value;
                OnPropertyChange();
            }
        }

        public bool IsRowChanged 
        {
            get
            {
                if (_product == null 
                    || HasErrors
                    || BestBefore == null 
                    || DescriptionCategory == null
                    || ProductSearchController.ProductSearchModel.Items.FirstOrDefault(p => p.Name == ProductName) == null)
                {  
                    return false;
                }
                
                if (ProductName != _product.Product.Name
                    || Unit != _product.Product.Unit
                    || ProductSearchController.DescriptionSearchModel.TextValue != Description
                    || ProductQuantity != _product?.Quantity
                    || Batch != _product?.Batch
                    || BestBefore.Value.Date != _product?.BestBefore.Date
                    || DescriptionCategory?.Id != _product?.DescriptionCategory.Id
                    || (_product?.Description != null ? ProductSearchController.DescriptionSearchModel.TextValue != _product?.Description?.Name : string.IsNullOrWhiteSpace(ProductSearchController.DescriptionSearchModel.TextValue) == false))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public string ButtonContent => _buttonContent;

        public string ButtonColor => _buttonColor;

        public ICommand DeleteProductCommand { get; }

        public ListViewInputViewModel ListViewInputViewModel => _viewModel;

        public ProductSearchController ProductSearchController => _productController;

        public bool IsComplete()
        {
            if(ProductSearchController.ProductSearchModel.Items.FirstOrDefault(p => p.Name == ProductSearchController.ProductSearchModel.TextValue) == null)
            {
                return false;
            }

            if (string.IsNullOrEmpty(Batch))
            {
                return false;
            }

            if (HasErrors || ProductSearchController.ProductSearchModel.HasErrors)
            {
                return false;
            }

            if (_dateError)
            {
                return false;
            }
            
            return true;
        }

        public void SetCreatedProduct(ReturnedProductViewModel newProduct)
        {
            _product = newProduct;
        }

        private void OnProductNamePropertyChange(ProductViewModel product)
        {
            if (product != null)
            { 
                Number = product.Number;
                Unit = product.Unit;
            }
        }

        private void OnPropertyDescriptionChange(ReturnedProductDescriptionViewModel product)
        {
            if (product != null) 
            {
                OnPropertyChange(nameof(Description));
            }
        }

        private void OnComplete() 
        {
            ButtonChange(_buttonColorDelete);
        }

        private void ButtonChange(string newAction) 
        {
            if (newAction == _buttonAddColor)
            {
                _buttonColor = _buttonAddColor;
                _buttonContent = _buttonAddContent;
            }
            else if (newAction == _buttonColorDelete)
            {
                _buttonColor = _buttonColorDelete;
                _buttonContent = _buttonContentDelete;
            }
            else if (newAction == _buttonColorUpdate)
            {
                _buttonColor = _buttonColorUpdate;
                _buttonContent = _buttonContentUpdate;
            }
            OnPropertyChange(nameof(ButtonColor));
            OnPropertyChange(nameof(ButtonContent));
        }

        private void RowCommand()
        {
            if (_buttonColor == _buttonAddColor)
            {
                IsCompleted();
            }
            else if (_buttonColor == _buttonColorDelete)
            {
                ListViewInputViewModel.RemoveRow(this);
            }
            else if (_buttonColor == _buttonColorUpdate)
            {
                ButtonChange(_buttonColorDelete);
                UpdateProduct(this);
            }
        }

        private bool IsButtonEnabled() 
        {
            if (IsComplete() || ButtonContent == _buttonContentDelete || IsRowChanged)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void DescriptionCategoryLoadCompleteCallBack()
        {
            if (_product != null) 
            {
                DescriptionCategory = _product.DescriptionCategory;
            }
        }

        private void OnViewModelPropertyChange(object? sender, PropertyChangedEventArgs e)
        {
            if (ButtonColor != _buttonAddColor 
                && (e.PropertyName == nameof(ProductQuantity) 
                || e.PropertyName == nameof(BestBefore) 
                || e.PropertyName == nameof(Batch)
                || e.PropertyName == nameof(Unit)
                || e.PropertyName == nameof(DescriptionCategory)
                || e.PropertyName == nameof(ProductSearchController.DescriptionSearchModel.TextValue)
                || e.PropertyName == nameof(ProductSearchController.ProductSearchModel.TextValue)
                || e.PropertyName == nameof(HasErrors)))
            {                
                if (IsRowChanged)
                {
                    ButtonChange(_buttonColorUpdate);
                }
                else
                {
                    ButtonChange(_buttonColorDelete);
                }
            }
        }
    }
}
