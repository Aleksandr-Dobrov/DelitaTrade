using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelitaTrade.Components.ComponetsViewModel
{
    public class SearchBoxTextNotUpperViewModel : SearchBoxTextViewModel
    {
        private string _item;
        public SearchBoxTextNotUpperViewModel(ObservableCollection<string> items, string text) : base(items, text)
        {
        }

        public string Item
        {
            get => _item;
            set
            { 
                _item = value;
                OnPropertyChange();
            }
        }
    }
}
