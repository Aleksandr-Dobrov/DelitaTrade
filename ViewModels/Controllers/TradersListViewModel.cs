using DelitaTrade.Components.ComponentsViewModel;
using DelitaTrade.Core.Contracts;
using DelitaTrade.Core.Services;
using DelitaTrade.Core.ViewModels;
using DelitaTrade.Extensions;
using DelitaTrade.ViewModels.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Navigation;

namespace DelitaTrade.ViewModels.Controllers
{
    public class TradersListViewModel : ViewModelBase, ITraderData
    {
        private const string _componentNameConstant = "Traders";
        private const string _propertyNameConstant = "Phone Number";
        private string _componentName;
        private string _phoneNumber;
        private string _propertyName;
        private EntityBaseControllerViewModel<TraderViewModel> _tradersViewModel;
        private TraderCommandsViewModel _traderCommands;

        private readonly IServiceProvider _serviceProvider;

        public TradersListViewModel(IServiceProvider serviceProvider, TraderCommandsViewModel traderCommands)
        {            
            _serviceProvider = serviceProvider;
            _traderCommands = traderCommands;
            _tradersViewModel = new EntityBaseControllerViewModel<TraderViewModel>();
            _traderCommands.CreateCommands(this);
            TraderViewModel.PropertyChanged += OnViewModelChange;
            Application.Current.Startup += UpdateItems;
        }

        public EntityBaseControllerViewModel<TraderViewModel> TraderViewModel => _tradersViewModel;

        public TraderCommandsViewModel TraderCommands => _traderCommands;

        public TraderViewModel Trader => TraderViewModel.Value.Value;

        public string ComponentName
        {
            get => _componentName;
            set
            {
                _componentName = value;
                OnPropertyChange();
            }
        }

        public string PhoneNumber
        {
            get => _phoneNumber;
            set
            {
                _phoneNumber = value;
                OnPropertyChange();
            }
        }

        public string PropertyName
        {
            get => _propertyName;
            set
            {
                _propertyName = value;
                OnPropertyChange();
            }
        }

        public async Task ReloadAll()
        {
            using var scope = _serviceProvider.CreateScope();
            var service = scope.GetService<ITraderService>();
            var traders = await service.GetAllAsync();
            TraderViewModel.UpdateItems(traders.OrderBy(t => t.Name));
        }

        public void InvokePropertyChange(string propertyName)
        {
            OnPropertyChange(propertyName);
        }

        private void OnViewModelChange(object? sender, PropertyChangedEventArgs e) 
        {
            if (e.PropertyName == nameof(TraderViewModel.Value))
            {
                PhoneNumber = TraderViewModel.Value.Value == null ? "" : TraderViewModel.Value.Value.PhoneNumber ?? "";
            }
        }

        private async void UpdateItems(object? sender, StartupEventArgs e) 
        {
            await ReloadAll();
            SetComponentNames();
        }

        private void SetComponentNames()
        {
            ComponentName = _componentNameConstant;
            PropertyName = _propertyNameConstant;
        }
    }
}
