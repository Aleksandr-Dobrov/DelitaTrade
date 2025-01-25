using DelitaTrade.Common;
using DelitaTrade.Components.ComponentsViewModel.ErrorComponents;
using DelitaTrade.Core.ViewModels;
using DelitaTrade.Infrastructure.Data.Models;
using DelitaTrade.ViewModels.Interfaces;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DelitaTrade.ViewModels
{
    public class CompaniesDataViewModel : ValidationViewModel, ICompanyData, ICompanyObjectData
    {
       // private readonly ErrorViewModel _errorViewModel = new();
        private const string _initialCompanyType = "ООД";
        private const string _initialBulstad = "BG";
        private const string _initialAddress = "";
        private string _company = string.Empty;
        private string _companyType = _initialCompanyType;
        private string _bulstad = _initialBulstad;
        private string _companyObject = string.Empty;
        private string _town = _initialAddress;
        private string? _street;
        private string? _number;
        private string? _gpsCoordinates;
        private string? _description;
        private bool _bankPay;

        public CompaniesDataViewModel()
        {
           // _errorViewModel.ErrorsChanged += OnErrorChange;
        }

        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
                
        public string CompanyName
        {
            get => _company;
            set
            {
                _company = value;
                OnPropertyChange();
            }
        }
        [MinLength(2)]
        [MaxLength(4)]
        public string CompanyType
        {
            get => _companyType;
            set
            {
                _companyType = value.ToUpper();
                //Validate(this);
                OnPropertyChange();
            }
        }
        [UserPasswordValidation]
        public string Bulstad
        {
            get => _bulstad;
            set
            {
                _bulstad = value;
                //Validate(this);
                OnPropertyChange();
            }
        }

        public string ObjectName
        {
            get => _companyObject;
            set
            {
                _companyObject = value;
                OnPropertyChange();
            }
        }

        public string Town
        {
            get => _town;
            set
            {
                _town = value;
                OnPropertyChange();
            }
        }

        public string? Street
        {
            get => _street;
            set
            {
                _street = value;
                OnPropertyChange();
            }
        }

        public string? Number
        {
            get => _number;
            set
            {
                _number = value;
                OnPropertyChange();
            }
        }

        public string? GpsCoordinates
        {
            get => _gpsCoordinates;
            set
            {
                _gpsCoordinates = value;
                OnPropertyChange();
            }
        }

        public string? Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChange();
            }
        }

        public bool BankPay
        {
            get => _bankPay;
            set
            {
                _bankPay = value;
                OnPropertyChange();
            }
        }

        public AddressViewModel GetAddress()
        {
            return new()
            {
                Town = Town,
                StreetName = Street,
                Number = Number,
                GpsCoordinates = GpsCoordinates,
                Description = Description,
            };
        }

        public AddressViewModel? AddressViewModel { get; set; }

        public AddressViewModel Address => throw new NotImplementedException();

        //public bool HasErrors => base.HasErrors; //_errorViewModel.HasErrors;

        public void RestoreObjectInputData()
        {
            ObjectName = string.Empty;
            Town = _initialAddress;
            Street = _initialAddress;
            Number = _initialAddress;
            GpsCoordinates = _initialAddress;
            AddressViewModel = null;
            Description = _initialAddress;
            BankPay = default;
        }
        public void RestoreCompanyInputData()
        {
            CompanyName = string.Empty;
            CompanyType = _initialCompanyType;
            Bulstad = _initialBulstad;
        }

        public void InvokePropertyChange(string propertyName)
        {
            OnPropertyChange(propertyName);
        }

        //public IEnumerable GetErrors(string? propertyName)
        //{
        //    return _errorViewModel.GetErrors(propertyName);
        //}

        //private void OnErrorChange(object? sender, DataErrorsChangedEventArgs e)
        //{
        //    ErrorsChanged?.Invoke(this, e);
        //}
    }
}
