﻿using DelitaTrade.Models;

namespace DelitaTrade.ViewModels
{
    public class CompanyObjectViewModel
    {
        private CompanyObject _companyObject;

        public CompanyObjectViewModel(CompanyObject companyObject)
        {
            _companyObject = companyObject;
        }

        public string ObjectName => _companyObject.Name;
        public string Adrress => _companyObject.Adrress;
        public bool BankPay => _companyObject.BankPay;
        public string Trader => _companyObject.Trader;

        public override string ToString()
        {
            return ObjectName;
        }
    }
}
