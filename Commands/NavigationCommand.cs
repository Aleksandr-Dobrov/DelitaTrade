using DelitaTrade.Services;
using DelitaTrade.ViewModels;

namespace DelitaTrade.Commands
{
    public class NavigationCommand<TViewModel> : CommandBase where TViewModel : ViewModelBase
    {
        private readonly NavigationService<TViewModel> _navigationService;

        public NavigationCommand(NavigationService<TViewModel> navigationService)
        {
             _navigationService = navigationService;
        }
        public override void Execute(object? parameter)
        {
            _navigationService.Navigate();
        }
    }
}
