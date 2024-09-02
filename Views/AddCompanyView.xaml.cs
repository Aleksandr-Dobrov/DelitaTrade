using System.Windows.Controls;

namespace DelitaTrade.Views
{
    /// <summary>
    /// Interaction logic for AddCompanyView.xaml
    /// </summary>
    public partial class AddCompanyView : UserControl
    {
        public AddCompanyView()
        {
            InitializeComponent();
        }

        private void Bulstad_GotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            textBox.CaretIndex = 2;
        }
    }
}
