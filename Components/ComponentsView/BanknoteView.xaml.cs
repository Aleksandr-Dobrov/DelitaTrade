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
    /// Interaction logic for BanknoteView.xaml
    /// </summary>
    public partial class BanknoteView : UserControl
    {
        private double _mouseWheelSpeed;

        public BanknoteView()
        {
            InitializeComponent();
        }

        private void TextBox_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            int x = 0;            
            int.TryParse(textBox?.Text, out x);
            int speed = Math.Abs(e.Delta) / 12;
            

            _mouseWheelSpeed += (Math.Abs(e.Delta) / (speed + 1)) / (double)10;

            if (e.Delta > 0)
            {
                if (_mouseWheelSpeed > 1)
                {
                    x++;
                    _mouseWheelSpeed = 0;
                }
            }
            else 
            {
                if (_mouseWheelSpeed > 1)
                {
                    x--;
                    _mouseWheelSpeed = 0;
                }
            }
            if (x < 1)
            {
                x = 1;
            }
            textBox.Text = x.ToString();
        }
    }
}
