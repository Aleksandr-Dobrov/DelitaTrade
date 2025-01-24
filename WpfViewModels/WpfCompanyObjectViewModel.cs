using DelitaTrade.Components.ComponentsViewModel.ErrorComponents;
using DelitaTrade.Core.ViewModels;
using DelitaTrade.ViewModels.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelitaTrade.WpfViewModels
{
    public class WpfCompanyObjectViewModel : ValidationViewModel
    {
        private CompanyObjectViewModel? _companyObjectViewModel;
        public int Id { get; set; }
        public string Name 
        {
            get => _companyObjectViewModel?.Name ?? string.Empty;
            set
            {
                if (_companyObjectViewModel != null)
                {
                    _companyObjectViewModel.Name = value;
                    OnPropertyChange();
                }
            }
        }
        public bool IsBankPay
        { 
            get => _companyObjectViewModel?.IsBankPay ?? false; 
            set
            {
                if (_companyObjectViewModel != null)
                {
                    _companyObjectViewModel.IsBankPay = value;
                    OnPropertyChange();
                }
            }
        }

        public void SelectViewModel(CompanyObjectViewModel companyObjectViewModel)
        {
            _companyObjectViewModel = companyObjectViewModel;
        }
    }
}
