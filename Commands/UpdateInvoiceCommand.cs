using DelitaTrade.Models;
using DelitaTrade.ViewModels;
using System.ComponentModel;

namespace DelitaTrade.Commands
{
    public class UpdateInvoiceCommand : CommandBase
    {

        private readonly DelitaTradeDayReport _dayReport;

        private readonly DayReportsViewModel _dayReportsViewModel;

        private readonly AddNewCompanyViewModel _addNewCompanyViewModel;

        public UpdateInvoiceCommand(DelitaTradeDayReport dayReport, AddNewCompanyViewModel addNewCompanyViewModel, DayReportsViewModel dayReportsViewModel)
        {
            _dayReport = dayReport;
            _dayReportsViewModel = dayReportsViewModel;
            _addNewCompanyViewModel = addNewCompanyViewModel;
            _dayReportsViewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(DayReportsViewModel.SelectedInvoiceViewModel))
            {
                OnCanExecuteChanged();               
            }
        }

        public override bool CanExecute(object? parameter)
        {
            return _dayReportsViewModel.SelectedInvoiceViewModel != null
                && base.CanExecute(parameter);
        }

        public override void Execute(object? parameter)
        {
            Invoice invoice = new Invoice($"{_dayReportsViewModel.SearchBox.InputText} {_dayReportsViewModel.CompanyType}",
                                                    _dayReportsViewModel.SearchBoxObject.InputTextObject,
                                                    _dayReportsViewModel.SelectedInvoiceViewModel.InvoiceID,
                                                    _dayReportsViewModel.PayMethodBox.PayMethodText,
                                                    _dayReportsViewModel.DecimalAmount,
                                                    _dayReportsViewModel.DecimalIncome,
                                                    _dayReportsViewModel.DoubleWeight);
            
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
             
             _dayReport.UpdateInvoice(invoice);            
        }
    }
}
