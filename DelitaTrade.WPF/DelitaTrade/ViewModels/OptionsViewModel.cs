using DelitaTrade.Components.ComponentsViewModel.OptionsComponentViewModels;
using DelitaTrade.Models;
using DelitaTrade.Services;
using DelitaTrade.ViewModels.Controllers;

namespace DelitaTrade.ViewModels
{
    public class OptionsViewModel : ViewModelBase
    {
        private readonly SoundOptionsViewModel _soundOptionsViewModel;
        private readonly DayReportInputOptionsViewModelComponent _dayReportInputOptions;
        private readonly DescriptionCategoryManagerController _descriptionCategoryManagerViewModel;
        private readonly ReturnProtocolBuildersController _returnProtocolBuildersController;


        public OptionsViewModel(SoundOptionsViewModel soundOptions, DayReportInputOptionsViewModelComponent options, DescriptionCategoryManagerController descriptionCategoryManagerViewModel, ReturnProtocolBuildersController returnProtocolBuildersController)
        {
            _soundOptionsViewModel = soundOptions;
            _dayReportInputOptions = options;
            _descriptionCategoryManagerViewModel = descriptionCategoryManagerViewModel;
            _returnProtocolBuildersController = returnProtocolBuildersController;
        }

        public SoundOptionsViewModel SoundOptionsViewModel => _soundOptionsViewModel;
        public DayReportInputOptionsViewModelComponent DayReportInputOptionsViewModel => _dayReportInputOptions;
        public DescriptionCategoryManagerController DescriptionCategoryManagerViewModel => _descriptionCategoryManagerViewModel;
        public ReturnProtocolBuildersController ReturnProtocolBuildersController => _returnProtocolBuildersController;
    }
}
