﻿using DelitaTrade.Views.ViewComponets;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DelitaTrade.Components.ComponentsView
{
    /// <summary>
    /// Interaction logic for LabeledInvoiceBox.xaml
    /// </summary>
    public partial class LabeledInvoiceBox : UserControl
    {
        private InputViewComponent _invoiceIdViewComponent;

        public LabeledInvoiceBox()
        {
            InitializeComponent();
            _invoiceIdViewComponent = new InputViewComponent();
        }
        private void TextBoxIDGotFocusSelectIndex(object sender, RoutedEventArgs e)
        {
            _invoiceIdViewComponent.TextBoxIDGotFocusSelectIndex(sender, e);
        }
        private void InvoiceId_KeyUp(object sender, KeyEventArgs e)
        {
            _invoiceIdViewComponent.InvoiceId_KeyUp(sender, e);
        }
        private void InvoiceId_KeyDown(object sender, KeyEventArgs e)
        {
            _invoiceIdViewComponent.InvoiceId_KeyDown(sender, e);
        }
    }
}
