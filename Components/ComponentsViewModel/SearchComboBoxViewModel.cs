using DelitaTrade.ViewModels;
using System.Collections.ObjectModel;
using System.ComponentModel;
using DelitaTrade.Core.Interfaces;
using DelitaTrade.Common;
using DelitaTrade.Components.ComponentsViewModel.ErrorComponents;
using System.ComponentModel.DataAnnotations;

namespace DelitaTrade.Components.ComponentsViewModel
{
    public class SearchComboBoxViewModel<T> : ValidationViewModel where T : INamed
    {
        private const string _searchHere = "Search here";
        private const string _notFound = "Not found";
        private string _name;
        private string _textValue;
        private string _autoComplete = _searchHere;
        private ObservableCollection<T> _items;
        private bool _isFocusable;

        public SearchComboBoxViewModel()
        {
            _items = new ObservableCollection<T>();
            Value = new();
            Value.ValueChanged += PropertyChange;
            PropertyChanged += OnViewModelChange;
        }

        public IEnumerable<T> Items => _items;

        public T? SelectedValue => Value.Value;

        public ItemProperty<T> Value
        { 
            get;
            set;
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public bool IsFocusable
        {
            get => _isFocusable;
            set 
            { 
                _isFocusable = value;
                OnPropertyChange();
            }
        }
        [MinLength(2, ErrorMessage = "Min length to create new is 2 symbols")]
        public string TextValue
        {
            get => _textValue;
            set 
            {
                if (!Value.IsValueCleared)
                { 
                    _textValue = value;
                    OnPropertyChange();                   
                }
            }
        }

        public string AutoComplete
        {
            get => _autoComplete;
            set 
            {
                _autoComplete = value;
                OnPropertyChange();
            }
        }

        public void SetSelectedValue(T value)
        {
            UpdateItems([value]);
            TextValue = value.Name;
            Value.Value = value;
        }

        public void SelectItem(T item)
        {
            Value.Value = item;
        }
        
        public void Add(T item)
        {
            _items.Add(item);
        }

        public void Remove(T item)
        {
            _items.Remove(item);
        }

        public void RemoveRange(IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                _items.Remove(item);
            }
        }

        public void UpdateItems(IEnumerable<T> items)
        {            
            Value.ResetProperty();
            _items.Clear();
            foreach (var item in items)
            {
                _items.Add(item);
            }
            SetAutoComplete();
        }

        private void PropertyChange(T value)
        {
            OnPropertyChange(nameof(Value.Value));
        }

        private void OnViewModelChange(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Value.Value))
            {
                if (Value.Value != null)
                {
                    AutoComplete = string.Empty;
                }
                else
                {
                    AutoComplete = _searchHere;
                }
            }
        }

        private void SetAutoComplete()
        {
            if (TextValue != null && TextValue.Length > 0 && _items.Count > 0)
            {
                AutoComplete = $"{TextValue} --> {_items[0].Name}";
            }
            else if (_items.Count > 0 && _items[0].ToString() == TextValue)
            {
                AutoComplete = string.Empty;
            }
            else if (_items.Count == 0)
            {
                AutoComplete = $"{TextValue} --- {_notFound}";
            }
            else
            {
                AutoComplete = _searchHere;
            }
        }
    }
}
