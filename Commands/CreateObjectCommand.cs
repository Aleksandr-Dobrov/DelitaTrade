using DelitaTrade.Models;
using DelitaTrade.ViewModels;
using System.ComponentModel;

namespace DelitaTrade.Commands
{
    public class CreateObjectCommand : CommandBase
    {
        private const int minLengthObjectName = 2;
        private readonly DelitaTradeCompany _delitaTrade;
        private readonly AddNewCompanyViewModel _addNewCompanyViewModel;

        public CreateObjectCommand(AddNewCompanyViewModel addNewCompanyViewModel,DelitaTradeCompany delitaTrade)
        {
            _addNewCompanyViewModel = addNewCompanyViewModel;
            _delitaTrade = delitaTrade;
            _addNewCompanyViewModel.PropertyChanged += OnViewModelPropertyChanged;
        }
        private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(AddNewCompanyViewModel.CurrentCompany) ||
                e.PropertyName == nameof(AddNewCompanyViewModel.ObjectName))
            {
                OnCanExecuteChanged();
            }
        }

        public override bool CanExecute(object? parameter)
        {
            return _addNewCompanyViewModel.CurrentCompany != null
                && _addNewCompanyViewModel.ObjectName != null
                && _addNewCompanyViewModel.ObjectName.Length >= minLengthObjectName
                && _addNewCompanyViewModel.Companies
                .First(c => c.CompanyName == _addNewCompanyViewModel.CompanyName)
                .CompanyObjects.FirstOrDefault(o => o.ObjectName == _addNewCompanyViewModel.ObjectName) == null
                && _addNewCompanyViewModel.ObjectName != "Empty"
                && base.CanExecute(parameter);
        }
        public override void Execute(object? parameter)
        {
            _delitaTrade.CreateNewCompanyObject(new CompanyObject(_addNewCompanyViewModel.CompanyName,
                                                                  _addNewCompanyViewModel.ObjectName,
                                                                  _addNewCompanyViewModel.Address,
                                                                  _addNewCompanyViewModel.Trader.Item,
                                                                  _addNewCompanyViewModel.BankPay));
        }       
    }
}
