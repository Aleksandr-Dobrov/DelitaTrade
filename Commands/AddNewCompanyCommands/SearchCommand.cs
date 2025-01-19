using DelitaTrade.Models.Loggers;
using DelitaTrade.ViewModels;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
namespace DelitaTrade.Commands.AddNewCompanyCommands
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

            if (_viewModel.CompaniesDataManager.CompanyData.GpsCoordinates.IsNullOrEmpty() == false)
            {                
                _searchString = _viewModel.CompaniesDataManager.CompanyData.GpsCoordinates;
            }
            else
            {
                _searchString = $"{_viewModel.CompaniesDataManager.CompanyData.Town} {_viewModel.CompaniesDataManager.CompanyData.Street} {_viewModel.CompaniesDataManager.CompanyData.Number} {_viewModel.CompaniesDataManager.CompanyData.Description}";
            }
        }

        private string GetSearchString()
        {
            SetSearchArgs();
            return _searchUrl + string.Join('+',_searchString.Split(' ', StringSplitOptions.RemoveEmptyEntries));
        }
    }
}
