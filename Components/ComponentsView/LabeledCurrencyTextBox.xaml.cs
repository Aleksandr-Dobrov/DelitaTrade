using DelitaTrade.Models.Loggers;
using DelitaTrade.Views.ViewComponets;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DelitaTrade.Components.ComponentsView
{
    /// <summary>
    /// Interaction logic for LabeledCurrencyTextBox.xaml
    /// </summary>
    public partial class LabeledCurrencyTextBox : UserControl
    {
        private CurrencyInputViewComponent _currencyView = new();
        public LabeledCurrencyTextBox()
        {
            InitializeComponent();
        }
        private void IncomeKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                _currencyView.InvoiceId_KeyUp(sender, e);
            }
            catch (OverflowException ex)
            {
                new MessageBoxLogger().Log(ex, Logger.LogLevel.Error);
                _currencyView.ResetCurrencyValue(sender);
            }
        }
        private void TextBoxIncomeGotFocus(object sender, RoutedEventArgs e)
        {
            _currencyView.TextBoxIDGotFocusSelectIndex(sender, e);
        }
    }
}
