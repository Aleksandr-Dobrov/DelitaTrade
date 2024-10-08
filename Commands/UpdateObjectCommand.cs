using DelitaTrade.Models;
using DelitaTrade.ViewModels;
using System.ComponentModel;

namespace DelitaTrade.Commands
{
    public class UpdateObjectCommand : CommandBase
    {
        private readonly DelitaTradeCompany _delitaTrade;
        private readonly AddNewCompanyViewModel _addNewCompanyViewModel;

        public UpdateObjectCommand(AddNewCompanyViewModel addNewCompanyViewModel, DelitaTradeCompany delitaTrade)
        {
            _addNewCompanyViewModel = addNewCompanyViewModel;
            _delitaTrade = delitaTrade;
            _addNewCompanyViewModel.PropertyChanged += OnViewModelPropertyChanged;
            _addNewCompanyViewModel.Trader.PropertyChanged += OnViewModelPropertyChanged;
        }

        private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(AddNewCompanyViewModel.CurrentCompany) ||
                e.PropertyName == nameof(AddNewCompanyViewModel.ObjectName) ||
                e.PropertyName == nameof(AddNewCompanyViewModel.Address) || 
                e.PropertyName == nameof(AddNewCompanyViewModel.Trader.Item) ||
                e.PropertyName == nameof(AddNewCompanyViewModel.BankPay))
            {
                OnCanExecuteChanged();
            }
        }

        public override bool CanExecute(object? parameter)
        {
            return _addNewCompanyViewModel.CurrentCompany != null
                && _addNewCompanyViewModel.Companies
                .First(c => c.CompanyName == _addNewCompanyViewModel.CompanyName).CompanyObjects
                .FirstOrDefault(o => o.ObjectName == _addNewCompanyViewModel.ObjectName) != null
                && (_addNewCompanyViewModel.CurrentObject?.Adrress != _addNewCompanyViewModel.Address
                || _addNewCompanyViewModel.CurrentObject?.BankPay != _addNewCompanyViewModel.BankPay
                || _addNewCompanyViewModel.CurrentObject?.Trader != _addNewCompanyViewModel.Trader.Item)
                && base.CanExecute(parameter);
        }

        public override void Execute(object? parameter)
        {
            if (Agreement("Update", _addNewCompanyViewModel.ObjectName))
            {
                _delitaTrade.UpdateCompanyObject(new CompanyObject(_addNewCompanyViewModel.CompanyName,
                                                                   _addNewCompanyViewModel.ObjectName,
                                                                   _addNewCompanyViewModel.Address,
                                                                   _addNewCompanyViewModel.Trader.Item,
                                                                   _addNewCompanyViewModel.BankPay));
            }
        }
    }
}
