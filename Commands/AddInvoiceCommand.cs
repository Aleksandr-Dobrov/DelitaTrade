using DelitaTrade.Components.ComponentsViewModel;
using DelitaTrade.Models;
using DelitaTrade.ViewModels;
using System.ComponentModel;

namespace DelitaTrade.Commands
{
    public class AddInvoiceCommand : CommandBase
    {
        private readonly DelitaTradeDayReport _dayReport;

        private readonly DayReportsViewModel _dayReportsViewModel;

        private readonly AddNewCompanyViewModel _addNewCompanyViewModel;

        public AddInvoiceCommand(DelitaTradeDayReport dayReport, AddNewCompanyViewModel addNewCompanyViewModel, DayReportsViewModel dayReportsViewModel)
        {
            _dayReport = dayReport;
            _dayReportsViewModel = dayReportsViewModel;
            _addNewCompanyViewModel = addNewCompanyViewModel;
            _dayReportsViewModel.PropertyChanged += OnViewModelPropertyChanged;
            _dayReportsViewModel.SearchBox.PropertyChanged += OnViewModelPropertyChanged;
            _dayReportsViewModel.InvoiceColectionChange += OnColectionIdChanged;
        }

        private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(DayReportsViewModel.InvoiceID) 
                || e.PropertyName == nameof(SearchBoxViewModel.InputText))               
            {
                OnCanExecuteChanged();                
            }
        }

        private void OnColectionIdChanged()
        {
            OnCanExecuteChanged();
        }

        public override bool CanExecute(object? parameter)
        {
            return string.IsNullOrEmpty(_dayReportsViewModel.SearchBox.InputText) == false
                && _dayReportsViewModel.InvoiceID.Length == 10                 
                && _dayReportsViewModel.InvoiceID.All(char.IsDigit)                
                && (_dayReportsViewModel.IsUnpaidInvoice(_dayReportsViewModel.InvoiceID) 
                    || _dayReportsViewModel.IsNewInvoice(_dayReportsViewModel.InvoiceID))
                && base.CanExecute(parameter);
        }

        public override void Execute(object? parameter)
        {
            Invoice invoice = new Invoice(_dayReport.DayReport.GetId(_dayReportsViewModel.InvoiceID), 
                                                    $"{_dayReportsViewModel.SearchBox.InputText} {_dayReportsViewModel.CompanyType}",
                                                    _dayReportsViewModel.SearchBoxObject.InputTextObject,
                                                    _dayReportsViewModel.InvoiceID,
                                                    _dayReportsViewModel.PayMethodBox.PayMethodText,
                                                    _dayReportsViewModel.DecimalAmount,
                                                    _dayReportsViewModel.DecimalIncome,
                                                    _dayReportsViewModel.DoubleWeight
                                                    );
           
            if (_addNewCompanyViewModel.CreateCompanyCommand.CanExecute(invoice))
            {
                _addNewCompanyViewModel.CreateCompanyCommand.Execute(invoice);
                _addNewCompanyViewModel.SetObjectName(invoice.ObjectName);
            }
            if (_addNewCompanyViewModel.CreateObjectCommand.CanExecute(invoice))
            {
                _addNewCompanyViewModel.SetBankPay(_dayReportsViewModel.PayMethodBox.PayMethodText);
                _addNewCompanyViewModel.CreateObjectCommand.Execute(invoice);
            }
            _dayReport.AddInvoice(invoice);
        }
    }
}
