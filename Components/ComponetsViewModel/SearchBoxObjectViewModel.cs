using DelitaTrade.Models;
using DelitaTrade.ViewModels;
using System.Collections.ObjectModel;

namespace DelitaTrade.Components.ComponetsViewModel
{
    public class SearchBoxObjectViewModel : ViewModelBase
    {
        private readonly AddNewCompanyViewModel _addNewCompanyViewModel;

		private string _inputText;

		private ObservableCollection<CompanyObjectViewModel> _companyObjects;

        private ObservableCollection<CompanyObjectViewModel> _emptyCompanyObjects;

        public IEnumerable<CompanyObjectViewModel> CompanyObjects => _companyObjects;

		public event Action ObjectNameCanged;

		public string InputTextObject
		{
			get => _inputText; 
			set
            {
                _inputText = value;
				ObjectNameCanged.Invoke();
				OnPropertyChange();
			}			
		}
        public SearchBoxObjectViewModel(AddNewCompanyViewModel addNewCompanyViewModel)
        {
			_addNewCompanyViewModel = addNewCompanyViewModel;
			_emptyCompanyObjects = [new CompanyObjectViewModel(new CompanyObject("Empty", "Empty", "Empty", false))];
			_companyObjects = _emptyCompanyObjects;
			_addNewCompanyViewModel.CompanySelected += CompanySelected;
			_addNewCompanyViewModel.CompanyUnselected += CompanyUnselected;
        }

		private void CompanySelected()
		{
			_companyObjects = (ObservableCollection<CompanyObjectViewModel>)_addNewCompanyViewModel.CurrentCompany.CompanyObjects;
			SelectInitialObject();
			OnPropertyChange(nameof(CompanyObjects));
		}

		private void CompanyUnselected()
		{
			_companyObjects = _emptyCompanyObjects;
			SelectInitialObject();
			OnPropertyChange(nameof(CompanyObjects));
		}

		private void SelectInitialObject()
		{
			if (_companyObjects.Count > 0)
			{
				InputTextObject = _companyObjects[0].ObjectName;
			}
			else
			{
				InputTextObject = _emptyCompanyObjects[0].ObjectName;
			}			
		}
    }
}
