using DelitaTrade.Models;
using DelitaTrade.ViewModels;
using System.ComponentModel;

namespace DelitaTrade.Commands
{
    public class CreateCompanyCommand : CommandBase
    {
        private const int minLengthCompanyName = 3;
        private readonly DelitaTradeCompany _delitaTrade;
        private readonly AddNewCompanyViewModel _addNewCompanyViewModel;

        public CreateCompanyCommand(AddNewCompanyViewModel addNewCompanyViewModel, DelitaTradeCompany delitaTrade)
        {
            _addNewCompanyViewModel = addNewCompanyViewModel;
            _delitaTrade = delitaTrade;
            _addNewCompanyViewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            //if (e.PropertyName == nameof(AddNewCompanyViewModel.CurrentCompany) 
            //    || e.PropertyName == nameof(AddNewCompanyViewModel.CompanyName)) 
            //{
            //    OnCanExecuteChanged();
            //}
        }

        //public override bool CanExecute(object? parameter)
        //{
        //    return _addNewCompanyViewModel.CurrentCompany == null
        //        && _addNewCompanyViewModel.CompanyName != null
        //        && _addNewCompanyViewModel.CompanyName?.Length >= minLengthCompanyName
        //        && base.CanExecute(parameter);
        //}

        public override void Execute(object? parameter)
        {
            
            //_delitaTrade.CreateNewCompany(new Company(_addNewCompanyViewModel.CompanyName,
            //    _addNewCompanyViewModel.CompanyType, 
            //    _addNewCompanyViewModel.Bulstad));
        }
    }
}
