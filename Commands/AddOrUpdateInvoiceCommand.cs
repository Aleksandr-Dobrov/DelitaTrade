using DelitaTrade.Models;
using DelitaTrade.ViewModels;
using System.ComponentModel;

namespace DelitaTrade.Commands
{
    public class AddOrUpdateInvoiceCommand : CommandBase
    {
        private readonly DelitaTradeDayReport _dayReport;

        private readonly DayReportsViewModel _dayReportsViewModel;

        private readonly AddNewCompanyViewModel _addNewCompanyViewModel;

        public AddOrUpdateInvoiceCommand(DelitaTradeDayReport dayReport, AddNewCompanyViewModel addNewCompanyViewModel, DayReportsViewModel dayReportsViewModel)
        {
            _dayReport = dayReport;
            _dayReportsViewModel = dayReportsViewModel;
            _addNewCompanyViewModel = addNewCompanyViewModel;
            _dayReportsViewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(DayReportsViewModel.InvoiceID) || e.PropertyName == nameof(DayReportsViewModel.Invoices))               
            {
                OnCanExecuteChanged();
                if (_dayReportsViewModel.Invoices
                    .FirstOrDefault(i => i.InvoiceID == _dayReportsViewModel.InvoiceID) != null)
                {
                    _dayReportsViewModel.AddOrUpdateTextCommand = "Update";
                }
                else
                {
                    _dayReportsViewModel.AddOrUpdateTextCommand = "Add";
                }
            }
        }

        public override bool CanExecute(object? parameter)
        {
            return _dayReportsViewModel.InvoiceID.Length == 10                 
                && _dayReportsViewModel.InvoiceID.All(char.IsDigit)
                && base.CanExecute(parameter);
        }

        public override void Execute(object? parameter)
        {
            Invoice invoice = new Invoice($"{_dayReportsViewModel.SearchBox.InputText} {_dayReportsViewModel.CompanyType}",
                                                    _dayReportsViewModel.SearchBoxObject.InputTextObject,
                                                    _dayReportsViewModel.InvoiceID,
                                                    _dayReportsViewModel.PayMethodBox.PayMethodText,
                                                    _dayReportsViewModel.DecimalAmount,
                                                    _dayReportsViewModel.DecimalIncome,
                                                    _dayReportsViewModel.DoubleWeight);
            if (_dayReportsViewModel.AddOrUpdateTextCommand == "Add")
            {
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
            else if (_dayReportsViewModel.AddOrUpdateTextCommand == "Update")
            {
                _dayReport.UpdateInvoice(invoice);
            }
        }
    }
}
