using DelitaTrade.Components.ComponentsViewModel.ReturnProtocolComponentViewModels;
using Microsoft.Extensions.DependencyInjection;
using DelitaTrade.Extensions;
using DelitaTrade.Core.Services;
using DelitaTrade.Core.ViewModels;

namespace DelitaTrade.ViewModels.ReturnProtocolControllers
{
    public class ReturnProtocolController : ViewModelBase
    {
        private InitialInformationViewModel _initialInformationViewModel;
        private ListViewInputViewModel _listViewInputViewModel;
        private IServiceProvider _serviceProvider;

        public ReturnProtocolController(ViewModelBase addNewCompanyViewModel, IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _initialInformationViewModel = new InitialInformationViewModel(addNewCompanyViewModel, serviceProvider);
            _listViewInputViewModel = new ListViewInputViewModel(serviceProvider, this);
            InitialInformationViewModel.CreateReturnProtocolEvent += CreateProtocol;
            InitialInformationViewModel.SelectedReturnProtocolEvent += SelectProtocol;
            InitialInformationViewModel.DeleteReturnProtocolEvent += DeleteProtocol;
        }
        public event Action<ReturnProtocolViewModel> ReturnProtocolSelected;
        public event Action ReturnProtocolUnSelected;

        public InitialInformationViewModel InitialInformationViewModel => _initialInformationViewModel;

        public ListViewInputViewModel ReturnProductsListViewModel => _listViewInputViewModel;

        private async void CreateProtocol(ReturnProtocolViewModel returnProtocol)
        {
            using var scope = _serviceProvider.CreateScope();
            var service = scope.GetService<ReturnProtocolService>();             
            returnProtocol.Id = await service.CreateProtocolAsync(returnProtocol);
            ReturnProtocolSelected(returnProtocol);
        }

        private async void SelectProtocol(ReturnProtocolViewModel returnProtocol)
        {
            await InitialInformationViewModel.LoadProductsTask;
            ReturnProtocolSelected(returnProtocol);
        }

        private async void DeleteProtocol(ReturnProtocolViewModel returnProtocol)
        {
            using var scope = _serviceProvider.CreateScope();
            var service = scope.GetService<ReturnProtocolService>();
            await service.DeleteProtocol(returnProtocol.Id);
            ReturnProtocolUnSelected();
        }

        //private async Task<int> AddReturnedProduct(ReturnedProductViewModel returnProduct)
        //{
        //    if (_currentProtocol == null) throw new ArgumentNullException("No return protocol loaded");
        //    var service = _serviceProvider.GetService<ReturnProductService>() ?? throw new InvalidOperationException($"Service {nameof(ReturnProductService)} not available");
        //    returnProduct.Id = await service.AddProductAsync(returnProduct, _currentReturnProtocolId);
        //    _currentProtocol.Products.Add(returnProduct);
        //    return returnProduct.Id;
        //}
    }
}
