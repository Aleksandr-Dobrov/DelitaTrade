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

namespace DelitaTrade.Components.ComponentsView
{
    /// <summary>
    /// Interaction logic for ItemsBoxNonDeletableNonEditable.xaml
    /// </summary>
    public partial class ItemsBoxNonDeletableNonEditable : UserControl
    {
        private bool _isAlreadyUpdate = false;
        public ItemsBoxNonDeletableNonEditable()
        {
            InitializeComponent();
        }

        private void searchBoxText_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SearchBoxTextUpdateSourse();
                _isAlreadyUpdate = true;
            }
        }

        private void SearchBoxTextUpdateSourse()
        {
            if (searchBoxText.SelectedItem != null || searchBoxText.Text != null)
            {
                BindingExpression be = searchBoxText.GetBindingExpression(ComboBox.TextProperty);
                be?.UpdateSource();
            }
        }

        private void searchBoxText_DropDownClosed(object sender, EventArgs e)
        {
            SearchBoxTextUpdateSourse();
            _isAlreadyUpdate = true;
        }

        private void searchBoxText_LostFocus(object sender, RoutedEventArgs e)
        {
            if (_isAlreadyUpdate == false)
            {
                SearchBoxTextUpdateSourse();
            }
            _isAlreadyUpdate = false;
        }
    }
}
