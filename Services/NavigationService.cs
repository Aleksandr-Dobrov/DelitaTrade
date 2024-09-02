using DelitaTrade.ViewModels;
using DelitaTrade.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
