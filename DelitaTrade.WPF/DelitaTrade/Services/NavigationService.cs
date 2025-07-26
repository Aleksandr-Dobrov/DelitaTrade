using DelitaTrade.ViewModels;
using DelitaTrade.Stores;

namespace DelitaTrade.Services
{
    public class NavigationService<TViewModel> 
        where TViewModel : ViewModelBase
    {
        private readonly ViewModelsStore _viewModels;
        
        public NavigationService(ViewModelsStore viewModels)
        {
            _viewModels = viewModels;
        }

        public void Navigate()
        {
            _viewModels.SetViewModel<TViewModel>();
        }
    }
}
