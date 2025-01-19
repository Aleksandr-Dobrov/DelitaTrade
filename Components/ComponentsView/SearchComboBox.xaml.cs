using System.Windows.Controls;
using System.Windows.Data;

namespace DelitaTrade.Components.ComponentsView
{
    /// <summary>
    /// Interaction logic for SearchComboBox.xaml
    /// </summary>
    public partial class SearchComboBox : UserControl
    {
        public SearchComboBox()
        {
            InitializeComponent();
        }

        private bool _isCancelCopy;

        private void SearchComboBoxText_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            searchComboBoxText.IsDropDownOpen = true;
        }

        private void SearchComboBoxText_PreviewKeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Tab && !(autoCompleteBox.Text == "Search here" || autoCompleteBox.Text == string.Empty))
            {
                if (searchComboBoxText.Items.Count > 0 && !_isCancelCopy)
                {
                    searchComboBoxText.Text = searchComboBoxText.Items[0].ToString();
                    searchComboBoxText.SelectedItem = searchComboBoxText.Items[0];
                }
                else
                {
                    _isCancelCopy = false;
                }
            }
            else if (e.Key == System.Windows.Input.Key.Escape)
            {
                e.Handled = true;
                _isCancelCopy = true;
            }
            
        }

        private void TextBlock_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            searchComboBoxText.Focus();
        }
    }
}
