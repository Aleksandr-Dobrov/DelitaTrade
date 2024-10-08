using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace DelitaTrade.Components
{
    /// <summary>
    /// Interaction logic for SearchBoxTextNotDeletable.xaml
    /// </summary>
    public partial class SearchBoxTextNotDeletable : UserControl
    {
        private bool _doNotOpenDropDown = false;

        public SearchBoxTextNotDeletable()
        {
            InitializeComponent();
        }
        private void TextPropertyUpdateSource()
        {
            BindingExpression be = searchBoxText.GetBindingExpression(ComboBox.TextProperty);
            be.UpdateSource();
        }

        private void searchBoxText_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                TextPropertyUpdateSource();
                _doNotOpenDropDown = true;
            }
        }

        private void searchBoxText_LostFocus(object sender, RoutedEventArgs e)
        {
            TextPropertyUpdateSource();
        }

        private void searchBoxText_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (_doNotOpenDropDown == false)
            {
                searchBoxText.IsDropDownOpen = true;
            }
            else
            {
                _doNotOpenDropDown = false;
            }
        }

        private void searchBoxText_DropDownClosed(object sender, EventArgs e)
        {
            TextPropertyUpdateSource();
        }
    }
}
