using DelitaTrade.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelitaTrade.Stores
{
    public class ViewModelsStore
    {
        private List<ViewModelBase> _viewModels;

        private ViewModelBase _currentViewModel;
        public ViewModelBase CurrentViewModel => _currentViewModel;

        public event Action ViewModelChanged;
        public ViewModelsStore(ViewModelBase[] viewModels)
        {
            _viewModels = [.. viewModels];
        }

        public void SetViewModel<TViewModel>() where TViewModel : ViewModelBase
        {
            _currentViewModel = _viewModels.First(v => v is TViewModel);
            CurrentViewModelChanged();
        }

        private void CurrentViewModelChanged()
        {
            ViewModelChanged?.Invoke();
        }
    }
}
