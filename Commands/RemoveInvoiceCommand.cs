using DelitaTrade.Models;
using DelitaTrade.ViewModels;
using System.ComponentModel;
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
            MessageBoxResult boxResult = MessageBox.Show($"Delete invoice: {_dayReportsViewModel.SelectedInvoiceViewModel.InvoiceID}?"
                                                       , "Delete?", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (boxResult == MessageBoxResult.Yes)
            {
                _dayReport.RemoveInvoice(_dayReportsViewModel.SelectedInvoiceViewModel.InvoiceID, _dayReportsViewModel.SelectedInvoiceViewModel.Id);
            }
        }
    }
}
