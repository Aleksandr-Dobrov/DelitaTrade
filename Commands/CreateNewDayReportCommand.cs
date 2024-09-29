using DelitaTrade.Models;
using DelitaTrade.ViewModels;
using System.ComponentModel;

namespace DelitaTrade.Commands
{
    public class CreateNewDayReportCommand : CommandBase
    {
        private readonly DelitaTradeDayReport _dayReport;

        private readonly DayReportsViewModel _dayReportsViewModel;
               
        public CreateNewDayReportCommand(DelitaTradeDayReport dayReport, DayReportsViewModel dayReportsViewModel)
        {
            _dayReport = dayReport;
            _dayReportsViewModel = dayReportsViewModel;
            _dayReportsViewModel.PropertyChanged += OnViewModelPropertyChanged;
            _dayReportsViewModel.DayReportIdViewModel.DataBaseChange += OnCanExecuteChanged;
        }

        private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(DayReportsViewModel.Date))
            {
                OnCanExecuteChanged();
            }
        }

        public override bool CanExecute(object? parameter)
        {
            return (_dayReportsViewModel.DayReportsId.Contains(_dayReportsViewModel.Date) == false)
                    && base.CanExecute(parameter);
        }

        public override void Execute(object? parameter)
        {
            _dayReport.CreateDayReport(_dayReportsViewModel.Date);
        }
    }
}
