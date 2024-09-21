using DelitaTrade.Models;
using DelitaTrade.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DelitaTrade.Commands
{
    public class RemoveInvoiceCommand : CommandBase
    {
        private readonly DelitaTradeDayReport _dayReport;

        private readonly DayReportsViewModel _dayReportsViewModel;

        public RemoveInvoiceCommand(DelitaTradeDayReport dayReport, DayReportsViewModel dayReportsViewModel)
        {
            _dayReport = dayReport;
            _dayReportsViewModel = dayReportsViewModel;
            _dayReportsViewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(DayReportsViewModel.InvoiceID) || e.PropertyName == nameof(DayReportsViewModel.Invoices))
            {
                OnCanExecuteChanged();
            }
        }

        public override bool CanExecute(object? parameter)
        {
            return _dayReportsViewModel.SelectedInvoiceViewModel != null
               && (_dayReportsViewModel.Invoices
                    .FirstOrDefault(i => i.InvoiceID == _dayReportsViewModel.InvoiceID 
                                            && i.Id == _dayReportsViewModel.Id) != null)
               && base.CanExecute(parameter);
        }

        public override void Execute(object? parameter)
        {
            MessageBoxResult boxResult = MessageBox.Show($"Delete invoice: {_dayReportsViewModel.InvoiceID}?"
                                                       , "Delete?", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (boxResult == MessageBoxResult.Yes)
            {
                _dayReport.RemoveInvoice(_dayReportsViewModel.InvoiceID, _dayReportsViewModel.Id);
            }
        }
    }
}
