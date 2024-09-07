using DelitaTrade.Components.ComponetsViewModel.ViewComponets;
using DelitaTrade.Models.Loggers;
using DelitaTrade.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DelitaTrade.Views
{
    /// <summary>
    /// Interaction logic for DayReportsView.xaml
    /// </summary>
    public partial class DayReportsView : UserControl
    {
        private InputViewComponent _invoiceIdViewComponet;
        private CurrencyInputViewComponent _amauntViewComponent;
        private CurrencyInputViewComponent _incomeViewComponent;
       
        public DayReportsView()
        {
            InitializeComponent();
            _invoiceIdViewComponet = new InputViewComponent();
            _amauntViewComponent = new CurrencyInputViewComponent();
            _incomeViewComponent = new CurrencyInputViewComponent();
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            textBox?.SelectAll();            
        }

        private void TextBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            textBox?.SelectAll();
        }

        private void TextBoxIDGotFocusSelectIndex(object sender, RoutedEventArgs e)
        {
            _invoiceIdViewComponet.TextBoxIDGotFocusSelectIndex(sender, e);            
        }

        private void TextBoxAmountGotFocus(object sender, RoutedEventArgs e)
        {
            _amauntViewComponent.TextBoxIDGotFocusSelectIndex(sender, e);
        }

        private void TextBoxIncomeGotFocus(object sender, RoutedEventArgs e)
        {
            _incomeViewComponent.TextBoxIDGotFocusSelectIndex(sender, e);
        }

        private void ListView_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var item = (sender as ListView)?.SelectedItem;
            if (item != null)
            {
                if (item is InvoiceViewModel invoice)
                {
                    invoiceId.Text = invoice.InvoiceID;
                }

            }
        }

        private void ListView_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            var item = (sender as ListView)?.SelectedItem;
            if(item != null && e.Key == Key.Enter)
            {
                if (item is InvoiceViewModel invoice)
                {
                    invoiceId.Text = invoice.InvoiceID;
                }
            }
        }

        private void InvoiceId_KeyUp(object sender, KeyEventArgs e)
        {
            _invoiceIdViewComponet.InvoiceId_KeyDown(sender, e);            
        }

        private void AmountKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                _amauntViewComponent.InvoiceId_KeyDown(sender, e);
            }
            catch(OverflowException ex) 
            {
                new MessageBoxLogger().Log(ex, Logger.LogLevel.Error);
                _amauntViewComponent.ResetCurrencyValue(sender);
            }
        }

        private void IncomeKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                _incomeViewComponent.InvoiceId_KeyDown(sender, e);
            }
            catch (OverflowException ex)
            {
                new MessageBoxLogger().Log(ex, Logger.LogLevel.Error);
                _incomeViewComponent.ResetCurrencyValue(sender);
            }
        }
    }
}
