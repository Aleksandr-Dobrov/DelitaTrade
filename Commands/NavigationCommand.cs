using DelitaTrade.Services;
using DelitaTrade.ViewModels;

namespace DelitaTrade.Commands
{
    public class NavigationCommand<TVievModel> : CommandBase where TVievModel : ViewModelBase
    {
        private readonly NavigationService<TVievModel> _navigationService;

        public NavigationCommand(NavigationService<TVievModel> navigationService)
        {
             _navigationService = navigationService;
        }
        public override void Execute(object? parameter)
        {
            _navigationService.Navigate();
        }
    }
}
