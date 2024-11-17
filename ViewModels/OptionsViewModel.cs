using DelitaTrade.Components.ComponentsViewModel.OptionsComponentViewModels;
using DelitaTrade.Models;

namespace DelitaTrade.ViewModels
{
    public class OptionsViewModel : ViewModelBase
    {
        private readonly DelitaTradeDayReport _delitaTradeDayReport;
        private readonly SoundOptionsViewModel _soundOptionsViewModel;
        private readonly DayReportInputOptionsViewModelComponent _dayReoprtInputOptions;
        
        public OptionsViewModel(DelitaTradeDayReport delitaTradeDayReport, DayReportInputOptionsViewModelComponent options)
        {
            _delitaTradeDayReport = delitaTradeDayReport;
            _soundOptionsViewModel = new SoundOptionsViewModel(delitaTradeDayReport);
            _dayReoprtInputOptions = options;
        }

        public SoundOptionsViewModel SoundOptionsViewModel => _soundOptionsViewModel;
        public DayReportInputOptionsViewModelComponent DayReportInputOptionsViewModel => _dayReoprtInputOptions;
    }
}
