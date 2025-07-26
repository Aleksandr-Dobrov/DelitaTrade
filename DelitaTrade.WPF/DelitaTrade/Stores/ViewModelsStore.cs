using DelitaTrade.ViewModels;

namespace DelitaTrade.Stores
{
    public class ViewModelsStore
    {
        private List<ViewModelBase> _viewModels;
        private ViewModelBase _currentViewModel;

        public ViewModelsStore(ViewModelBase[] viewModels)
        {
            _viewModels = [.. viewModels];
        }

        public event Action ViewModelChanged;

        public ViewModelBase CurrentViewModel => _currentViewModel;

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
