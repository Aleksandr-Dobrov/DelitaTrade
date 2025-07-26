using DelitaTrade.Components.ComponentsViewModel.ReturnProtocolComponentViewModels;
using Microsoft.Extensions.DependencyInjection;
using DelitaTrade.Extensions;
using DelitaTrade.Core.Services;
using DelitaTrade.Core.ViewModels;
using DelitaTrade.Core.Contracts;
using System.Windows.Controls;
using DelitaTrade.ViewModels.Controllers;

namespace DelitaTrade.ViewModels.ReturnProtocolControllers
{
    public class ReturnProtocolController : ViewModelBase
    {
        private InitialInformationViewModel _initialInformationViewModel;
        private ListViewInputViewModel _listViewInputViewModel;
        private IServiceProvider _serviceProvider;

        public ReturnProtocolController(InitialInformationViewModel initialInformationViewModel, IServiceProvider serviceProvider, UserController userController, DescriptionCategoryController descriptionCategoryController, DescriptionCategoryManagerController descriptionManager)
        {
            _serviceProvider = serviceProvider;
            _initialInformationViewModel = initialInformationViewModel;
            descriptionManager.CategoriesUpdated += descriptionCategoryController.UpdateDescriptionCategories;
            _listViewInputViewModel = new ListViewInputViewModel(serviceProvider, this, descriptionCategoryController);
            userController.UserLogout += UnselectedProtocol;
            InitialInformationViewModel.CreateReturnProtocolEvent += CreateProtocol;
            InitialInformationViewModel.SelectedReturnProtocolEvent += SelectProtocol;
            InitialInformationViewModel.DeleteReturnProtocolEvent += DeleteProtocol;
            InitialInformationViewModel.UpdateReturnProtocolEvent += UpdateProtocol;
        }
        public event Action<ReturnProtocolViewModel> ReturnProtocolSelected;
        public event Action ReturnProtocolUnSelected;

        public InitialInformationViewModel InitialInformationViewModel => _initialInformationViewModel;

        public ListViewInputViewModel ReturnProductsListViewModel => _listViewInputViewModel;

        private async void CreateProtocol(ReturnProtocolViewModel returnProtocol)
        {
            using var scope = _serviceProvider.CreateScope();
            var service = scope.GetService<IReturnProtocolService>();             
            returnProtocol.Id = await service.CreateProtocolAsync(returnProtocol);
            ReturnProtocolSelected(returnProtocol);
        }

        private async void UpdateProtocol(ReturnProtocolViewModel returnProtocol)
        {
            using var scope = _serviceProvider.CreateScope();
            var service = scope.GetService<IReturnProtocolService>();
            await service.UpdateProtocolAsync(returnProtocol);
        }

        private async void SelectProtocol(ReturnProtocolViewModel returnProtocol)
        {
            await InitialInformationViewModel.LoadProductsTask;
            ReturnProtocolSelected(returnProtocol);
        }

        private async void DeleteProtocol(ReturnProtocolViewModel returnProtocol)
        {
            using var scope = _serviceProvider.CreateScope();
            var service = scope.GetService<IReturnProtocolService>();
            await service.DeleteProtocolAsync(returnProtocol.Id);
            ReturnProtocolUnSelected();
        }

        private void UnselectedProtocol()
        {    
            InitialInformationViewModel.OnLogOut();
            ReturnProtocolUnSelected();
        }
    }
}
