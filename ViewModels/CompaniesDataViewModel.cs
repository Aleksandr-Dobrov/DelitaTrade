﻿using DelitaTrade.Core.ViewModels;
using DelitaTrade.Infrastructure.Data.Models;
using DelitaTrade.ViewModels.Interfaces;

namespace DelitaTrade.ViewModels
{
    public class CompaniesDataViewModel : ViewModelBase, ICompanyData, ICompanyObjectData
    {
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

        public string CompanyName
        {
            get => _company;
            set
            {
                _company = value;
                OnPropertyChange();
            }
        }
        public string CompanyType
        {
            get => _companyType;
            set
            {
                _companyType = value.ToUpper();
                OnPropertyChange();
            }
        }
        public string Bulstad
        {
            get => _bulstad;
            set
            {                
                _bulstad = value;
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
    }
}
