//using Microsoft.Office.Interop.Excel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DelitaTrade.Components.ComponetsViewModel.ViewComponets
{
    public class InputViewComponent
    {
        protected int _textBoxIDIndex;
        protected Stack<List<string>> _textBoxItems;
        protected string _textBoxInitialValue;

        public InputViewComponent()
        {
            _textBoxItems = new Stack<List<string>>();
            _textBoxInitialValue = string.Empty;
        }

        public void TextBoxIDGotFocusSelectIndex(object sender, RoutedEventArgs e)
        {
            IDGotFocusSelectIndex(sender, e);
        }

        public void InvoiceId_KeyDown(object sender, KeyEventArgs e)
        {
            KeyDown(sender, e);
        }

        private void IDGotFocusSelectIndex(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            _textBoxInitialValue = textBox?.Text;
            TextBoxSetStartPosition(sender);
        }

        protected virtual void TextBoxSetStartPosition(object sender)
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

        protected virtual void TextBoxIndexMove(object sender, Direction direction)
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

        protected virtual void KeyDown(object sender, KeyEventArgs e)
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
                case Key.D0:
                case Key.D1:
                case Key.D2:
                case Key.D3:
                case Key.D4:
                case Key.D5:
                case Key.D6:
                case Key.D7:
                case Key.D8:
                case Key.D9:
                    if (textBox != null)
                    {
                        _textBoxItems.Push(new List<string>([_textBoxIDIndex.ToString(), textBox.Text]));                        
                    }
                    TextBoxIndexMove(sender, Direction.Left);
                    break;
                default:
                    
                    break;
            }
        }

        public enum Direction
        {
            Left = -1,
            Right = 1
        }
    }
}
