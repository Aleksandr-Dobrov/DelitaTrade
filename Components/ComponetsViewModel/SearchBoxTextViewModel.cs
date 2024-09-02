using DelitaTrade.Models.Interfaces;
using DelitaTrade.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DelitaTrade.Components.ComponetsViewModel
{
    public class SearchBoxTextViewModel : ViewModelBase
    {
		private string _item;
		private string _text;
		private ObservableCollection<string> _items;

        public SearchBoxTextViewModel(ObservableCollection<string> items, string text)
        {
            _items = items;
			_text = text;
			ItemsChanged += () => { };
			_items.CollectionChanged += ColectionChanged;
        }

		public event Action ItemsChanged;

		private void ColectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
		{
			ItemsChanged();
		}
		       
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

		public IEnumerable<string> Items => _items;
	}
}
