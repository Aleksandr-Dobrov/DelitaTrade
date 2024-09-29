using DelitaTrade.Components.ComponetsViewModel.OptionsComponentViewModels;
using DelitaTrade.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
