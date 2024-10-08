using DelitaTrade.Commands;
using DelitaTrade.Models;
using DelitaTrade.ViewModels;
using System.ComponentModel;

namespace DelitaTrade.Commands
{
    public class DeleteCompanyCommand :CommandBase
    {
        private readonly DelitaTradeCompany _delitaTrade;
        private readonly AddNewCompanyViewModel _addNewCompanyViewModel;

        public DeleteCompanyCommand(AddNewCompanyViewModel addNewCompanyViewModel, DelitaTradeCompany delitaTrade)
        {
            _addNewCompanyViewModel = addNewCompanyViewModel;
            _delitaTrade = delitaTrade;
            _addNewCompanyViewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(AddNewCompanyViewModel.CurrentCompany))
            {
                OnCanExecuteChanged();
            }
        }

        public override bool CanExecute(object? parameter)
        {
            return _addNewCompanyViewModel.CurrentCompany != null               
                && base.CanExecute(parameter);
        }

        public override void Execute(object? parameter)
        {
            if (Agreement("Delete", _addNewCompanyViewModel.CompanyName))
            {
                _delitaTrade.DeleteCompany(new Company(_addNewCompanyViewModel.CurrentCompany.CompanyName,
                                                       _addNewCompanyViewModel.CurrentCompany.CompanyType,
                                                       _addNewCompanyViewModel.CurrentCompany.Bulstad));
            }
        }
    }
}
