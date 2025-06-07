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
    /// Interaction logic for PlaceHolderPasswordBox.xaml
    /// </summary>
    public partial class PlaceHolderPasswordBox : UserControl
    {
        public PlaceHolderPasswordBox()
        {
            InitializeComponent();
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (passwordBox.Password.Length > 0)
            {
                passwordBox.passwordBox.SelectAll();
            }
            passwordBox.passwordBox.Focus();
        }
    }
}
