using DelitaTrade.Models;
using DelitaTrade.Models.ReturnProtocol;
using DelitaTrade.ViewModels;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace DelitaTrade.Components.ComponetsViewModel.ReturnProtocolComponentViewModels
{
    public class InitialInformationViewModel
    {   
        private readonly AddNewCompanyViewModel _addNewCompanyViewModel;

        private SearchBoxTextNotUpperViewModel _returnProtocolDate;

        private SearchBoxTextNotUpperViewModel _returnProtocolPayMethod;

        public InitialInformationViewModel(ViewModelBase addNewCompanyViewModel)
        {
            _addNewCompanyViewModel = addNewCompanyViewModel as AddNewCompanyViewModel;
            _returnProtocolDate = new SearchBoxTextNotUpperViewModel(CreateDates(DateTime.Now), "Date");
            _returnProtocolPayMethod = new SearchBoxTextNotUpperViewModel(CreatePayMethods(), "Pay Method");
        }

        public SearchBoxViewModel SearchBox => _addNewCompanyViewModel.SearchBox;
        public SearchBoxObjectViewModel SearchBoxObject => _addNewCompanyViewModel.SearchBoxObject;

        public SearchBoxTextNotUpperDeletableItemViewModel Trader => _addNewCompanyViewModel.Trader;

        public SearchBoxTextNotUpperViewModel ReturnProtocolDate => _returnProtocolDate;

        public SearchBoxTextNotUpperViewModel ReturnProtocolPayMethod => _returnProtocolPayMethod;

        private ObservableCollection<string> CreateDates(DateTime date)
        {
            var dates = new ObservableCollection<string>();

            for (int i = 0; i < 10; i++)
            {
                dates.Add($"{date.Date.AddDays(i):dd-MM-yyyy}");
            }
            return dates;
        }

        private ObservableCollection<string> CreatePayMethods()
        {
            return
            [
                ReturnProtocolPayMethods.BankPay,
                ReturnProtocolPayMethods.Deducted,
                ReturnProtocolPayMethods.NotDeducted
            ];
        }
    }
}
