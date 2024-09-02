using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DelitaTrade.ViewModels
{
    public class SavedInvoiceViewModel
    {
        private string _income = string.Empty;
        private string _amount = string.Empty;
        private string _weight  = string.Empty;
        private string _companyName = string.Empty;
        private string _objectName = string.Empty;
        private string _payMethod = string.Empty;
        private string _previousCompanyName = string.Empty;
        private string _previousObjectName = string.Empty;

        private string _invoiceId = string.Empty;
        private bool _isSavedInvoiceData;

        public bool IsSavedInvoiceData => _isSavedInvoiceData;
        public string PreviousCompanyName => _previousCompanyName;
        public string PreviousObjectName => _previousObjectName;

        public void SaveInvoiceData(DayReportsViewModel dayReportsViewModel)
        {
            if (_isSavedInvoiceData == false || _invoiceId == dayReportsViewModel.InvoiceID)
            { 
                _isSavedInvoiceData = true;
                _companyName = dayReportsViewModel.SearchBox.InputText;
                _objectName = dayReportsViewModel.SearchBoxObject.InputTextObject;
                _payMethod = dayReportsViewModel.PayMethodBox.PayMethodText;
                _income = dayReportsViewModel.Income;
                _amount = dayReportsViewModel.Amount;
                _weight = dayReportsViewModel.Weight;
                _previousCompanyName = _companyName;
                _previousObjectName = _objectName;
            }

            _invoiceId = dayReportsViewModel.InvoiceID;
        }

        public void LoadInvoiceData(DayReportsViewModel dayReportsViewModel)
        {
            _isSavedInvoiceData = false;
            dayReportsViewModel.SearchBox.InputText = _companyName;
            dayReportsViewModel.SearchBoxObject.InputTextObject = _objectName;
            dayReportsViewModel.PayMethodBox.PayMethodText = _payMethod;
            dayReportsViewModel.Income = _income;
            dayReportsViewModel.Amount = _amount;
            dayReportsViewModel.Weight = _weight;
        }

    }
}
