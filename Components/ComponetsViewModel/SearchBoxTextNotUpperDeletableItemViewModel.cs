using DelitaTrade.Components.ComponentsCommands;
using DelitaTrade.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DelitaTrade.Components.ComponetsViewModel
{
    public class SearchBoxTextNotUpperDeletableItemViewModel : SearchBoxTextNotUpperViewModel
    {
        private string _item;

        public SearchBoxTextNotUpperDeletableItemViewModel(ObservableCollection<string> items, string text, IDeleteColectionStringItem deletableColection) : base(items, text)
        {
            DeleteCommand = new TextBoxTextDeleteCommand(deletableColection, this);
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
        public ICommand DeleteCommand { get; }
    }
}
