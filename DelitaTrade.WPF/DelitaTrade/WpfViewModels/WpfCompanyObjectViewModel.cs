using DelitaTrade.Components.ComponentsViewModel.ErrorComponents;
using DelitaTrade.Core.ViewModels;
using DelitaTrade.Infrastructure.Data.Models;
using DelitaTrade.Models;
using DelitaTrade.ViewModels.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelitaTrade.WpfViewModels
{
    public class WpfCompanyObjectViewModel : ValidationViewModel, ICompanyObjectData
    {
        private CompanyObjectViewModel? _companyObjectViewModel;
        private CompanyViewModel? _companyViewModel;
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

        public string ObjectName
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

        public bool BankPay
        {
            get => _companyObjectViewModel == null 
                ? false : _companyObjectViewModel.IsBankPay;
            set
            {
                if (_companyObjectViewModel != null)
                {
                    _companyObjectViewModel.IsBankPay = value;
                    OnPropertyChange();
                }
            }
        }
        [MinLength(3, ErrorMessage = "Min Length is 3 symbols")]
        public string Town
        {
            get => (_companyObjectViewModel == null || _companyObjectViewModel.Address == null) 
                ? string.Empty : _companyObjectViewModel.Address.Town;
            set
            {
                if (_companyObjectViewModel != null && _companyObjectViewModel.Address != null)
                {
                    _companyObjectViewModel.Address.Town = value;
                    OnPropertyChange();
                }
            }
        }

        public string Street
        {
            get => (_companyObjectViewModel == null || _companyObjectViewModel.Address == null || _companyObjectViewModel.Address.StreetName == null) 
                ? string.Empty : _companyObjectViewModel.Address.StreetName;
            set
            {
                if (_companyObjectViewModel != null && _companyObjectViewModel.Address != null)
                {
                    _companyObjectViewModel.Address.StreetName = value;
                    OnPropertyChange();
                }
            }
        }
        public string Number
        {
            get => (_companyObjectViewModel == null || _companyObjectViewModel.Address == null || _companyObjectViewModel.Address.Number == null) 
                ? string.Empty : _companyObjectViewModel.Address.Number;
            set
            {
                if (_companyObjectViewModel != null && _companyObjectViewModel.Address != null)
                {
                    _companyObjectViewModel.Address.Number = value;
                    OnPropertyChange();
                }
            }
        }
        public string GpsCoordinates
        {
            get => (_companyObjectViewModel == null || _companyObjectViewModel.Address == null || _companyObjectViewModel.Address.GpsCoordinates == null) 
                ? string.Empty : _companyObjectViewModel.Address.GpsCoordinates;
            set
            {
                if (_companyObjectViewModel != null && _companyObjectViewModel.Address != null)
                {
                    _companyObjectViewModel.Address.GpsCoordinates = value;
                    OnPropertyChange();
                }
            }
        }
        public string Description
        {
            get => (_companyObjectViewModel == null || _companyObjectViewModel.Address == null || _companyObjectViewModel.Address.Description == null) 
                ? string.Empty : _companyObjectViewModel.Address.Description;
            set
            {
                if (_companyObjectViewModel != null && _companyObjectViewModel.Address != null)
                {
                    _companyObjectViewModel.Address.Description = value;
                    OnPropertyChange();
                }
            }
        }

        public AddressViewModel? Address => _companyObjectViewModel?.Address;
        public bool HasCompany => _companyViewModel != null;

        public void SelectViewModel(CompanyObjectViewModel companyObjectViewModel)
        {
            _companyObjectViewModel = companyObjectViewModel;
            if (_companyObjectViewModel.Address == null)
            {
                _companyObjectViewModel.Address = new() { Town = string.Empty };
            }
            _companyViewModel = companyObjectViewModel.Company;
            OnPropertyChange(nameof(HasCompany));
            OnPropertyChange(nameof(BankPay));
            OnPropertyChange(nameof(Town));
            OnPropertyChange(nameof(Street));
            OnPropertyChange(nameof(Number));
            OnPropertyChange(nameof(GpsCoordinates));
            OnPropertyChange(nameof(Description));
        }
        public void UnSelectViewModel()
        {
            if (!HasCompany)
            {
                _companyObjectViewModel = null;
            }
            else if (_companyObjectViewModel != null) 
            {
                var company = _companyObjectViewModel.Company;
                _companyObjectViewModel = new CompanyObjectViewModel() {Company = company, Name = string.Empty, Address = new() { Town = string.Empty } };
            }
            OnPropertyChange(nameof(BankPay));
            OnPropertyChange(nameof(Town));
            OnPropertyChange(nameof(Street));
            OnPropertyChange(nameof(Number));
            OnPropertyChange(nameof(GpsCoordinates));
            OnPropertyChange(nameof(Description));
        }

        public void SelectCompany(CompanyViewModel company)
        {
            if(_companyViewModel == null || company.Id != _companyViewModel.Id) 
            {
                _companyViewModel = company;
                _companyObjectViewModel = new CompanyObjectViewModel() { Company = company, Name = string.Empty, Address = new() { Town = string.Empty } };
            }
            OnPropertyChange(nameof(HasCompany));
            OnPropertyChange(nameof(Town));
        }

        public void UnSelectCompany()
        {
            _companyViewModel = null!;
            _companyObjectViewModel = null!;
            OnPropertyChange(nameof(HasCompany));
            OnPropertyChange(nameof(BankPay));
            OnPropertyChange(nameof(Town));
            OnPropertyChange(nameof(Street));
            OnPropertyChange(nameof(Number));
            OnPropertyChange(nameof(GpsCoordinates));
            OnPropertyChange(nameof(Description));
        }

        public void InvokePropertyChange(string propertyName)
        {
            OnPropertyChange(propertyName);
        }
    }
}
