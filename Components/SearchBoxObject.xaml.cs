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
    /// Interaction logic for SearchBoxObject.xaml
    /// </summary>
    public partial class SearchBoxObject : UserControl
    {
        public SearchBoxObject()
        {
            InitializeComponent();
        }

        private void SearchBoxObjectsDelita_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            SearchBoxObjectsDelita.IsDropDownOpen = true;
        }
    }
}
