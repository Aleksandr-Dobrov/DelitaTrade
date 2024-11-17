using DelitaTrade.ViewModels;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace DelitaTrade.Components.ComponentsViewModel
{
    public class SearchBoxTextViewModel : ViewModelBase
    {
		private ObservableCollection<string> _items;
		private string _item;
		private string _text;
        private string _textColor = "black";

        public SearchBoxTextViewModel(ObservableCollection<string> items, string text)
        {
            _items = items;
			_text = text;
			ItemsChanged += () => { };
			_items.CollectionChanged += ColectionChanged;
        }

		public event Action ItemsChanged;

		public IEnumerable<string> Items => _items;
		       
        public string Item
		{
			get => _item;
			set 
			{
				_item = value?.ToUpper();
				OnPropertyChange();
			}
		}

		public string Text
		{
			get => _text;
			set
			{
				_text = value;
				OnPropertyChange();
			}
		}

		public string TextColor
		{
			get => _textColor;
			set
			{
				_textColor = value;
				OnPropertyChange();
			}
		}

		private void ColectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
		{
			ItemsChanged();
		}
	}
}
