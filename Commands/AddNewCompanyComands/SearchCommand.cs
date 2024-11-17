using DelitaTrade.Models.Loggers;
using DelitaTrade.ViewModels;
using System.Diagnostics;
namespace DelitaTrade.Commands.AddNewCompanyComands
{
    public class SearchCommand : CommandBase
    {
        const string _searchUrl = "https://www.google.com/maps/search/";
        private readonly AddNewCompanyViewModel _viewModel;
        private string _searchString;
        
        public SearchCommand(AddNewCompanyViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override void Execute(object? parameter)
        {
            try 
            {
                Process.Start("explorer.exe", GetSearchString());
            }
            catch (Exception ex)
            {
                new MessageBoxLogger().Log(ex, Logger.LogLevel.Error);
            }
        }

        private void SetSearchArgs()
        {
            string address = _viewModel.Address;

            if (_viewModel.Address.Contains('(') && _viewModel.Address.Contains(')'))
            {
                int startIndex = address.IndexOf('(') + 1;
                int endIndex = address.IndexOf(')');
                _searchString = address[startIndex..endIndex];
            }
            else
            {
                _searchString = address;
            }
        }

        private string GetSearchString()
        {
            SetSearchArgs();
            return _searchUrl + string.Join('+',_searchString.Split(' ', StringSplitOptions.RemoveEmptyEntries));
        }
    }
}
