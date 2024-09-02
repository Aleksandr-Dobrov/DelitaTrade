using DelitaTrade.Commands;
using DelitaTrade.Components.ComponetsViewModel;
using DelitaTrade.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelitaTrade.Components.ComponentsCommands
{
    public class TextBoxTextDeleteCommand : CommandBase
    {
        private readonly IDeleteColectionStringItem _deleteColectionStringItem;

        private readonly SearchBoxTextNotUpperDeletableItemViewModel _searchBoxViewModel;

        public TextBoxTextDeleteCommand(IDeleteColectionStringItem deleteColectionStringItem, SearchBoxTextNotUpperDeletableItemViewModel searchBoxTextViewModel)
        {
            _deleteColectionStringItem = deleteColectionStringItem;
            _searchBoxViewModel = searchBoxTextViewModel;
            _searchBoxViewModel.PropertyChanged += OnViewModelPropertyChanged;
            _searchBoxViewModel.ItemsChanged += OnItemsChanged;
            
        }

        private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SearchBoxTextNotUpperDeletableItemViewModel.Item))
            {
                OnCanExecuteChanged();
            }
        }

        private void OnItemsChanged()
        {
            OnCanExecuteChanged();
        }

        public override bool CanExecute(object? parameter)
        {
            return _searchBoxViewModel.Items.Contains(_searchBoxViewModel.Item)
                && base.CanExecute(parameter);
        }

        public override void Execute(object? parameter)
        {
            _deleteColectionStringItem.DeleteItem(_searchBoxViewModel.Item);
            _searchBoxViewModel.Item = string.Empty;
        }
    }
}
