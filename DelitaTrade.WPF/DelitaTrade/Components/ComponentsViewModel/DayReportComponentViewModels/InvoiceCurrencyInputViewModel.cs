using DelitaTrade.Common.Enums;
using DelitaTrade.Components.ComponentsViewModel.OptionsComponentViewModels;
using DelitaTrade.Core.ViewModels;
using DelitaTrade.ViewModels.Interfaces;

namespace DelitaTrade.Components.ComponentsViewModel.DayReportComponentViewModels
{
    public class InvoiceCurrencyInputViewModel
    {
        private bool _isInvoiceSelected;
        private bool _objectIsBankPay;
        private readonly LabeledWeightTextBoxViewModel _labeledStringToDecimalTextBoxViewModel;
        private readonly LabeledInvoiceNumberViewModel _invoiceNumberViewModel;
        private readonly LabeledCurrencyViewModel _amountViewModel;
        private readonly LabeledPayMethodSelectableBoxViewModel _payMethodViewModel;
        private readonly LabeledCurrencyViewModel _incomeViewModel;

        public InvoiceCurrencyInputViewModel(LabeledInvoiceNumberViewModel invoiceNumberViewModel, LabeledCurrencyViewModel amountViewModel, LabeledPayMethodSelectableBoxViewModel payMethodViewModel, LabeledCurrencyViewModel incomeViewModel, LabeledWeightTextBoxViewModel labeledStringToDecimalTextBoxViewModel)
        {
            _invoiceNumberViewModel = invoiceNumberViewModel;
            _amountViewModel = amountViewModel;
            _payMethodViewModel = payMethodViewModel;
            _incomeViewModel = incomeViewModel;
            _labeledStringToDecimalTextBoxViewModel = labeledStringToDecimalTextBoxViewModel;
            _payMethodViewModel.PayMethodChange += OnPayMethodChange;
            _amountViewModel.CurrencyChanged += OnAmountChange;
            InitializeViewModels();
        }

        public LabeledInvoiceNumberViewModel InvoiceNumberViewModel => _invoiceNumberViewModel;

        public LabeledCurrencyViewModel AmountViewModel => _amountViewModel;

        public LabeledPayMethodSelectableBoxViewModel PayMethodViewModel => _payMethodViewModel;

        public LabeledCurrencyViewModel IncomeViewModel => _incomeViewModel;

        public LabeledWeightTextBoxViewModel LabeledStringToDecimalTextBoxViewModel => _labeledStringToDecimalTextBoxViewModel;

        public bool HasError => InvoiceNumberViewModel.HasErrors ||
                    AmountViewModel.HasErrors ||
                    PayMethodViewModel.HasErrors ||
                    IncomeViewModel.HasErrors ||
                    LabeledStringToDecimalTextBoxViewModel.HasErrors;
        public void SetPayMethod(PayMethod payMethod)
        {
            if (payMethod == PayMethod.Bank) _objectIsBankPay = true;
            else _objectIsBankPay = false;
            PayMethodViewModel.SetPayMethod(payMethod);
        }

        public void OnInvoiceSelected(InvoiceViewModel invoiceViewModel)
        {
            _isInvoiceSelected = true;
            PayMethodViewModel.SetPayMethod(invoiceViewModel.PayMethod);
            InvoiceNumberViewModel.TextBox = invoiceViewModel.Number;
            AmountViewModel.SetCurrencyValue(invoiceViewModel.Amount);
            IncomeViewModel.SetCurrencyValue(invoiceViewModel.Income);
            LabeledStringToDecimalTextBoxViewModel.TextBox = invoiceViewModel.Weight.ToString();
            _isInvoiceSelected = false;
        }

        private void InitializeViewModels()
        {
            _invoiceNumberViewModel.Label = "Number";
            _amountViewModel.Label = "Amount";
            _payMethodViewModel.Label = "PayMethod";
            _incomeViewModel.Label = "Income";
            OnPayMethodChange(PayMethodViewModel.CurrentPayMethod);
        }

        private void OnPayMethodChange(PayMethod payMethod)
        {
            if (payMethod == PayMethod.Bank || payMethod == PayMethod.Cancellation)
            {
                IncomeViewModel.IsEnable = false;
                IncomeViewModel.SetCurrencyValue("0");
                InvoiceNumberViewModel.SetLastNumber();
            }
            else if (payMethod == PayMethod.CreditNote && _objectIsBankPay)
            {
                IncomeViewModel.IsEnable = true;
                IncomeViewModel.SetCurrencyValue("0");
                InvoiceNumberViewModel.SetLastNumber();
            }
            else
            {
                if (payMethod == PayMethod.Expense)
                {
                    IncomeViewModel.CurrencyStatus = CurrencyStatus.Negative;
                    AmountViewModel.SetCurrencyValue("0");
                    if (_isInvoiceSelected == false)
                    {
                        InvoiceNumberViewModel.SetExpenseNumber(DateTime.Now);
                    }
                }
                else if (payMethod == PayMethod.CreditNote)
                {
                    IncomeViewModel.CurrencyStatus = CurrencyStatus.Negative;
                    InvoiceNumberViewModel.SetLastNumber();
                }
                else
                {
                    IncomeViewModel.CurrencyStatus = CurrencyStatus.Positive;
                    InvoiceNumberViewModel.SetLastNumber();
                }
                IncomeViewModel.IsEnable = true;
                IncomeViewModel.SetMaxAvailableCurrencyValue(AmountViewModel.TextBox);
            }
        }

        private void OnAmountChange(string amount)
        {
            if (PayMethodViewModel.CurrentPayMethod == PayMethod.Cash ||
                (PayMethodViewModel.CurrentPayMethod == PayMethod.CreditNote && _objectIsBankPay == false) ||
                PayMethodViewModel.CurrentPayMethod == PayMethod.OldPayCard ||
                PayMethodViewModel.CurrentPayMethod == PayMethod.OldPayCash ||
                PayMethodViewModel.CurrentPayMethod == PayMethod.Card)
            {
                SetMaxIncomeValue();
                IncomeViewModel.SetCurrencyValue(amount);
            }
            else
            {
                ResetMaxIncomeValue();
            }
        }

        private void SetMaxIncomeValue()
        {
            IncomeViewModel.SetMaxCurrencyValue(AmountViewModel.Money);
        }

        private void ResetMaxIncomeValue()
        {
            IncomeViewModel.ResetMaxValue();
        }
    }
}
