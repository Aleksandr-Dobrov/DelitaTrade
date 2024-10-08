using System.Collections.ObjectModel;

namespace DelitaTrade.Components.ComponentsViewModel
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
