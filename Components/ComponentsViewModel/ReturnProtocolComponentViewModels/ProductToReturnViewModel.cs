using DelitaTrade.Commands.ReturnProtocolCommands;
using DelitaTrade.ViewModels;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using DelitaTrade.Core.ViewModels;

namespace DelitaTrade.Components.ComponentsViewModel.ReturnProtocolComponentViewModels
{
    public class ProductToReturnViewModel : ViewModelBase
    {
        private int _id;
        private string _name;
        private double _quantity;
        private string _unit;
        private string _batch;
        private DateTime _bestBefore = DateTime.Now;
        private string _description;

        private ListViewInputViewModel _viewModel;        
        private Visibility _isVisible = Visibility.Collapsed;

        public ProductToReturnViewModel(string name, ListViewInputViewModel viewModel)
        {
            _name = name;
            _viewModel = viewModel;
            PropertyChanged += OnViewModelPropertyChange;
            PropertyChanged += OnProductNamePropertyChange;
            IsCompleted += OnComplete;
            DeleteProductCommand = new DeleteProductCommand(this);            
        }
        public ProductToReturnViewModel(ReturnedProductViewModel product, ListViewInputViewModel viewModel)
        {
            Id = product.Id;
            ProductName = product.Product.Name;
            Unit = product.Product.Unit;
            ProductQuantity = product.Quantity;
            Batch = product.Batch;
            BestBefore = product.BestBefore;
            Description = product.Description.Description;
            _viewModel = viewModel;
            PropertyChanged += OnProductNamePropertyChange;
            DeleteProductCommand = new DeleteProductCommand(this);
            OnComplete();
        }

        public event Action IsCompleted;

        public int Id 
        {
            get => _id;
            set => _id = value;
        }

        public string ProductName
        {
            get { return _name; }
            set 
            { 
                _name = value;
                OnPropertyChange();               
            }
        }

        public double ProductQuantity 
        {
            get { return _quantity; }
            set
            {
                _quantity = value;
                OnPropertyChange();
            }
        }
        public string Unit
        {
            get { return _unit; }
            set
            {
                _unit = value;
                OnPropertyChange();
            }
        }
        public string Batch
        {
            get { return _batch; }
            set
            {
                _batch = value;
                OnPropertyChange();
            }
        }
        public DateTime BestBefore
        {
            get { return _bestBefore; }
            set
            {
                _bestBefore = value;
                OnPropertyChange();
            }
        }
        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                OnPropertyChange();
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

        public ICommand DeleteProductCommand { get; }

        public ListViewInputViewModel ListViewInputViewModel => _viewModel;

        public bool IsComplete()
        {
            foreach (var property in GetType().GetProperties())
            {
                if (property.PropertyType == typeof(string))
                {
                    if (string.IsNullOrEmpty((string?)property.GetValue(this, null)))
                    {
                        return false;
                    }
                }
                else if (property.PropertyType == typeof(double))
                {
                    if ((double?)property.GetValue(this, null) == 0)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private void OnViewModelPropertyChange(object? sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName != nameof(IsVisible))
            { 
                if (IsComplete()) { IsCompleted(); }
            }
            
        }
        private void OnProductNamePropertyChange(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ProductName))
            {
                if (_viewModel.SelectedProduct != null) Unit = _viewModel.SelectedProduct.Unit;
            }
        }

        private void OnComplete() 
        {
            IsVisible = Visibility.Visible;
        }
    }
}
