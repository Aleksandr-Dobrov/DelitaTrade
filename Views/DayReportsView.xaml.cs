using DelitaTrade.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

namespace DelitaTrade.Views
{
    /// <summary>
    /// Interaction logic for DayReportsView.xaml
    /// </summary>
    public partial class DayReportsView : UserControl
    {
        enum Direction
        {
            Left = -1,
            Right = 1
        }

        private int _textBoxIDIndex;
        private Stack<List<string>> _textBoxItems;
        private string _textBoxInitialValue;
        public DayReportsView()
        {
            InitializeComponent();
            _textBoxItems = new Stack<List<string>>();
            _textBoxInitialValue = string.Empty;
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            textBox?.SelectAll();            
        }

        private void TextBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            textBox?.SelectAll();
        }

        private void TextBoxIDGotFocusSelectIndex(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            _textBoxInitialValue = textBox?.Text;
            TextBoxSetStartPosition(sender);
        }

        private void TextBoxSetStartPosition(object sender)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null)
            {
                _textBoxIDIndex = textBox.Text.Length - 1;
                textBox.Select(_textBoxIDIndex, 1);
                _textBoxItems.Clear();
            }
        }

        private void TextBoxResetValue(object sender)
        {
            _textBoxItems.Clear();
            TextBox textBox = sender as TextBox;
            if (textBox != null)
            {
                textBox.Text = _textBoxInitialValue;
            }
        }

        private void TextBoxIndexMove(object sender, Direction direction)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null)
            {
                int maxLength = textBox.Text.Length - 1;
               
                if ((direction == Direction.Right && _textBoxIDIndex < maxLength) || (direction == Direction.Left && _textBoxIDIndex > 0))
                {   
                    _textBoxIDIndex += (int)direction;
                }

                textBox.Select(_textBoxIDIndex, 1);
            }
        }

        private void ListView_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var item = (sender as ListView)?.SelectedItem;
            if (item != null)
            {
                if (item is InvoiceViewModel invoice)
                {
                    invoiceId.Text = invoice.InvoiceID;
                }

            }
        }

        private void ListView_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            var item = (sender as ListView)?.SelectedItem;
            if(item != null && e.Key == Key.Enter)
            {
                if (item is InvoiceViewModel invoice)
                {
                    invoiceId.Text = invoice.InvoiceID;
                }
            }
        }

        private void InvoiceId_KeyUp(object sender, KeyEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            switch (e.Key)
            {
                case Key.Tab:
                    break;
                case Key.Escape:
                    textBox.Text = _textBoxInitialValue;
                    TextBoxSetStartPosition(sender);
                    break;
                case Key.Back:
                    if (_textBoxItems.Count > 0)
                    {
                        var backData = _textBoxItems.Pop();
                        _textBoxIDIndex = int.Parse(backData[0]);
                        textBox.Text = backData[1];
                    }
                    else if (_textBoxItems.Count == 0)
                    {
                        textBox.Text = _textBoxInitialValue;
                    }
                    textBox.Select(_textBoxIDIndex, 1);
                    break;
                case Key.Right:
                    TextBoxIndexMove(sender, Direction.Right);
                    break;
                    case Key.Left:
                    TextBoxIndexMove(sender, Direction.Left);
                        break;
                default:
                    if (textBox != null)
                    {
                        _textBoxItems.Push(new List<string>([_textBoxIDIndex.ToString(), textBox.Text]));               
                    }
                    TextBoxIndexMove(sender, Direction.Left);
                    break;
            }
        }
    }
}
