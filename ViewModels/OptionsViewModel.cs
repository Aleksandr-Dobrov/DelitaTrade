using DelitaTrade.Components.ComponentsViewModel.OptionsComponentViewModels;
using DelitaTrade.Models;
using DelitaTrade.Services;

namespace DelitaTrade.ViewModels
{
    public class OptionsViewModel : ViewModelBase
    {
        private readonly SoundOptionsViewModel _soundOptionsViewModel;
        private readonly DayReportInputOptionsViewModelComponent _dayReoprtInputOptions;
        
        public OptionsViewModel(SoundOptionsViewModel soundOptions, DayReportInputOptionsViewModelComponent options)
        {
            _soundOptionsViewModel = soundOptions;
            _dayReoprtInputOptions = options;
        }

        public SoundOptionsViewModel SoundOptionsViewModel => _soundOptionsViewModel;
        public DayReportInputOptionsViewModelComponent DayReportInputOptionsViewModel => _dayReoprtInputOptions;
    }
}
