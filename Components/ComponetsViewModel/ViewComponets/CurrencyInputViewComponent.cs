using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using static System.Net.Mime.MediaTypeNames;
using System.Windows.Input;
using DelitaTrade.Models.DataProviders;

namespace DelitaTrade.Components.ComponetsViewModel.ViewComponets
{
    public class CurrencyInputViewComponent : InputViewComponent
    {
        CurrencyProvider _currencyProvider;

        public CurrencyInputViewComponent()
        {
            _currencyProvider = new CurrencyProvider();
        }
        protected override void TextBoxIndexMove(object sender, Direction direction)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null)
            {
                int maxLength = textBox.Text.Length - 5;
                do
                {
                    if ((direction == Direction.Right && _textBoxIDIndex < maxLength) || (direction == Direction.Left && _textBoxIDIndex > 0))
                    {
                        _textBoxIDIndex += (int)direction;
                    }
                }
                while (char.IsAsciiDigit(textBox.Text[_textBoxIDIndex]) == false);

                textBox.Select(_textBoxIDIndex, 1);
            }
        }

        protected override void TextBoxSetStartPosition(object sender)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null)
            {
                _textBoxIDIndex = textBox.Text.Length - _currencyProvider.GetCurrencyLength() - 1;
                textBox.Select(_textBoxIDIndex, 1);
                _textBoxItems.Clear();
            }
        }

        private void WriteDigits(object sender)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null)
            {
                decimal amount = 0;
                if (textBox.Text.Length < 5)
                {
                    textBox.Text = $"{0:C2}";
                }
                else
                {
                    amount = _currencyProvider.GetDecimalValue(textBox.Text);
                }
                amount *= 10;
                textBox.Text = $"{amount:C2}";
                textBox.Select(textBox.Text.Length - _currencyProvider.GetCurrencyLength() - 1, 1);
            }

        }

        protected override void KeyDown(object sender, KeyEventArgs e)
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
                        _textBoxIDIndex = backData[1].Length - _currencyProvider.GetCurrencyLength();
                        string restoreCurrency = backData[1].Insert(_textBoxIDIndex, " ");
                        textBox.Text = restoreCurrency;
                    }
                    else if (_textBoxItems.Count == 0)
                    {
                        string initialCurrency = _textBoxInitialValue.Insert(_textBoxIDIndex, " ");
                        textBox.Text = initialCurrency;
                    }
                    textBox.Select(_textBoxIDIndex, 1);
                    break;
                case Key.Space:
                case Key.Delete:
                    ResetCurrencyValue(sender);
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
                        WriteDigits(textBox);
                    }
                    
                    break;
                default:
                    if (_textBoxItems.Count > 0)
                    {
                        _textBoxItems.Push(new List<string>([_textBoxIDIndex.ToString(), textBox.Text]));
                        var backData = _textBoxItems.Peek();
                        _textBoxIDIndex = backData[1].Length - _currencyProvider.GetCurrencyLength();
                        textBox.Text = backData[1];
                    }
                    else 
                    {
                        textBox.Text = _textBoxInitialValue;
                    }
                    textBox.Select(_textBoxIDIndex, 1);
                    break;
            }
        }

        public void ResetCurrencyValue(object sender)
        {
            if (sender is TextBox textBox)
            {
                textBox.Text = $"{0:C2}";
                TextBoxSetStartPosition(sender);
            }
        }
    }
}
