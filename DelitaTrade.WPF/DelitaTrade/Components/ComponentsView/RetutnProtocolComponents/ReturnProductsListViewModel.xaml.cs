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

namespace DelitaTrade.Components.ComponentsView.RetutnProtocolComponents
{
    /// <summary>
    /// Interaction logic for ReturnProductsListViewModel.xaml
    /// </summary>
    public partial class ReturnProductsListViewModel : UserControl
    {
        public ReturnProductsListViewModel()
        {
            InitializeComponent();
        }

        private void ComboBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if(sender is ComboBox products) products.IsDropDownOpen = true;
        }

        private void TextBoxGotFocus(object sender, RoutedEventArgs e)
        {
            if(sender is TextBox textBox)
            {
                textBox.SelectAll();
            }
        }
    }
}
