using DelitaTrade.Common;
using DelitaTrade.Components.ComponentsViewModel.ErrorComponents;
using DelitaTrade.Core.Interfaces;
using DelitaTrade.ViewModels;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace DelitaTrade.Components.ComponentsViewModel
{
    public class EntityBaseControllerViewModel<T> : ValidationViewModel where T : INamed, IIdent
    {
        private string _name;
        private string _textValue;
        private ObservableCollection<T> _entities;

        public EntityBaseControllerViewModel()
        {
            _entities = new ObservableCollection<T>();
            Value = new();
            Value.ValueChanged += PropertyChange;
        }

        public IEnumerable<T> Items => _entities;

        public string Name 
        { 
            get => _name;
            set 
            { 
                _name = value; 
            } 
        }
        [MinLength(3, ErrorMessage = "Min length is 3 symbols")]
        public string TextValue
        {
            get => _textValue;
            set
            {
                _textValue = value;
                OnPropertyChange();
            }
        }

        public ItemProperty<T> Value
        {
            get;
            set;
        }

        public void SetSelectedValue(T value)
        {
            UpdateItems([value]);
            TextValue = value.Name;
            Value.Value = value;
        }

        public void Add(T value)
        {
            _entities.Add(value);
        }

        public void Remove(T value)
        { 
            _entities.Remove(value);
        }

        public void UpdateItems(IEnumerable<T> items)
        {
            Value.ResetProperty();
            _entities.Clear();
            foreach (var item in items)
            {
                _entities.Add(item);
            }
        }
        private void PropertyChange(T value)
        {
            OnPropertyChange(nameof(Value.Value));
        }
    }
}
