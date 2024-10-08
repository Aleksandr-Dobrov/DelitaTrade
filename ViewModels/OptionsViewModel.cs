using DelitaTrade.Components.ComponentsViewModel.OptionsComponentViewModels;
using DelitaTrade.Models;

namespace DelitaTrade.ViewModels
{
    public class OptionsViewModel : ViewModelBase
    {
        private readonly DelitaTradeDayReport _delitaTradeDayReport;
        private readonly SoundOptionsViewModel _soundOptionsViewModel;
        
        public OptionsViewModel(DelitaTradeDayReport delitaTradeDayReport)
        {
            _delitaTradeDayReport = delitaTradeDayReport;
            _soundOptionsViewModel = new SoundOptionsViewModel(delitaTradeDayReport);
        }

        public SoundOptionsViewModel SoundOptionsViewModel => _soundOptionsViewModel;
    }
}
