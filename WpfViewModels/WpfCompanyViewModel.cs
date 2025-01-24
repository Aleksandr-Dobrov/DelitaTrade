using DelitaTrade.Components.ComponentsViewModel.ErrorComponents;
using DelitaTrade.Core.ViewModels;
using DelitaTrade.ViewModels.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelitaTrade.WpfViewModels
{
    public class WpfCompanyViewModel : ValidationViewModel, ICompanyData
    {
        private CompanyViewModel _companyViewModel = new() { Name = string.Empty , Bulstad = _initialBulstad, Type = _initialCompanyType };
        private const string _initialCompanyType = "ООД";
        private const string _initialBulstad = "BG";
        public int Id { get; set; }

        public string CompanyName
        {
            get => _companyViewModel.Name;
            set
            {
                _companyViewModel.Name = value;
                OnPropertyChange();
            }
        }
        [MinLength(2)]
        public string CompanyType
        { 
            get => _companyViewModel.Type ?? string.Empty;
            set
            {
                _companyViewModel.Type = value.ToUpper();
                OnPropertyChange();
            }
        }

        public string Bulstad 
        { 
            get => _companyViewModel.Bulstad ?? string.Empty;
            set
            {
                _companyViewModel.Bulstad = value.ToUpper();
                OnPropertyChange();
            }
        }

        public void SelectViewModel(CompanyViewModel companyViewModel)
        {
            _companyViewModel = companyViewModel;
            OnPropertyChange(nameof(Bulstad));
            OnPropertyChange(nameof(CompanyType));
        }

        public void UnSelectViewModel()
        {
            _companyViewModel = new() { Name = string.Empty, Bulstad = _initialBulstad, Type = _initialCompanyType };
            OnPropertyChange(nameof(Bulstad));
            OnPropertyChange(nameof(CompanyType));
        }
        public void InvokePropertyChange(string propertyName)
        {
            OnPropertyChange(propertyName);
        }
    }
}
