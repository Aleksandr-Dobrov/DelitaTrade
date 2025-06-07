using System.Windows.Controls;

namespace DelitaTrade.Components.ComponentsView
{
    /// <summary>
    /// Interaction logic for SearchComboBoxBase.xaml
    /// </summary>
    public partial class SearchComboBoxBase : UserControl
    {
        public SearchComboBoxBase()
        {
            InitializeComponent();
        }
        private bool _isCancelCopy;

        private void SearchComboBoxText_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            searchComboBoxBaseText.IsDropDownOpen = true;
        }

        private void SearchComboBoxText_PreviewKeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Tab && !(autoCompleteBox.Text == "Search" || autoCompleteBox.Text == string.Empty))
            {
                if (searchComboBoxBaseText.Items.Count > 0 && !_isCancelCopy)
                {
                    searchComboBoxBaseText.Text = searchComboBoxBaseText.Items[0].ToString();
                    searchComboBoxBaseText.SelectedItem = searchComboBoxBaseText.Items[0];
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
            searchComboBoxBaseText.Focus();
        }
    }
}
