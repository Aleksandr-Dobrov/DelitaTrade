using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace DelitaTrade.Components.ComponentsView
{
    /// <summary>
    /// Interaction logic for ItemsBoxNonDeletable.xaml
    /// </summary>
    public partial class ItemsBoxNonDeletable : UserControl
    {
        private bool _isAlreadyUpdate = false;
        public ItemsBoxNonDeletable()
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
            if (searchBoxText.SelectedItem != null)
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
