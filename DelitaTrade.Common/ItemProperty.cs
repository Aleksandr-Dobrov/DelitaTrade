using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelitaTrade.Common
{
    public class ItemProperty<T>
    {
        private T _value;

        public ItemProperty()
        {
            ValueChanged += (v) => { };
        }

        public event Action<T> ValueChanged;

        public bool IsValueCleared { get; set; }

        public T Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
                ValueChanged(Value);                
            }
        }

        public void ResetProperty()
        {  
            IsValueCleared = true;
            Value = default;
            IsValueCleared = false;
        }

        public static implicit operator T(ItemProperty<T> value)
        {
            return value.Value;
        }

        public static implicit operator ItemProperty<T>(T value)
        {
            return new ItemProperty<T> { Value = value };
        }
    }
}
