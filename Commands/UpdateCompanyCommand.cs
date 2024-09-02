using DelitaTrade.Models;
using DelitaTrade.ViewModels;
using System.ComponentModel;

namespace DelitaTrade.Commands
{
    public class UpdateCompanyCommand : CommandBase
    {
        private readonly DelitaTradeCompany _delitaTrade;
        private readonly AddNewCompanyViewModel _addNewCompanyViewModel;

        public UpdateCompanyCommand(AddNewCompanyViewModel addNewCompanyViewModel, DelitaTradeCompany delitaTrade)
        {
            _delitaTrade = delitaTrade;
            _addNewCompanyViewModel = addNewCompanyViewModel;
            _addNewCompanyViewModel.PropertyChanged += OnViewModelPropertyChanged;
        }
        private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(AddNewCompanyViewModel.CurrentCompany) ||
                e.PropertyName == nameof(AddNewCompanyViewModel.CompanyType) ||
                e.PropertyName == nameof(AddNewCompanyViewModel.Bulstad))                
            {
                OnCanExecuteChanged();
            }
        }

        public override bool CanExecute(object? parameter)
        {
            return _addNewCompanyViewModel.CurrentCompany != null 
                && (_addNewCompanyViewModel.CurrentCompany.CompanyType != _addNewCompanyViewModel.CompanyType
                || _addNewCompanyViewModel.CurrentCompany.Bulstad != _addNewCompanyViewModel.Bulstad)
                && base.CanExecute(parameter);
        }
        public override void Execute(object? parameter)
        {
            if (Agreement("Update", _addNewCompanyViewModel.CompanyName))
            {
                _delitaTrade.UpdateCompanyData(new Company(_addNewCompanyViewModel.CompanyName,
                                                           _addNewCompanyViewModel.CompanyType,
                                                           _addNewCompanyViewModel.Bulstad));
            }
        }
    }
}
