using DelitaTrade.ViewModels;
using System.Collections.ObjectModel;

namespace DelitaTrade.Components.ComponetsViewModel
{
    public class SearchBoxViewModel : ViewModelBase 
    {
        private ObservableCollection<CompanyViewModel> _companyViewModels;

        private string _inputText;

        public SearchBoxViewModel(ObservableCollection<CompanyViewModel> companyViewModels)
        {  
            _companyViewModels = companyViewModels;
        }

        public event Action CompanyNameChanged;

        public IEnumerable<CompanyViewModel> CompanyViewModels => _companyViewModels;
        
        public string InputText
        {
            get => _inputText;
            set
            {                 
                _inputText = value;
                OnCompanyNameChange();
                OnPropertyChange();
            }
        }

        private void OnCompanyNameChange()
        {
            CompanyNameChanged?.Invoke();
        }
    }
}
