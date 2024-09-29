using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DelitaTrade.Components
{
    /// <summary>
    /// Interaction logic for SearchBoxTextDeletable.xaml
    /// </summary>
    public partial class SearchBoxTextDeletable : UserControl
    {
        private bool _doNotOpenDropDown = false;

        public SearchBoxTextDeletable()
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
