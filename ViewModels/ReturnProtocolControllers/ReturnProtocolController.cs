using DelitaTrade.Components.ComponentsViewModel.ReturnProtocolComponentViewModels;
using Microsoft.Extensions.DependencyInjection;
using DelitaReturnProtocolProvider.ViewModels;
using DBDelitaTrade.Infrastructure.Data;
using DelitaReturnProtocolProvider.Services;

namespace DelitaTrade.ViewModels.ReturnProtocolControllers
{
    public class ReturnProtocolController : ViewModelBase
    {
        private InitialInformationViewModel _initialInformationViewModel;
        private ListViewInputViewModel _listViewInputViewModel;
        private ReturnProtocolViewModel _currentProtocol;
        private int _currentReturnProtocolId;
        private IServiceProvider _serviceProvider;

        public ReturnProtocolController(ViewModelBase addNewCompanyViewModel, IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _initialInformationViewModel = new InitialInformationViewModel(addNewCompanyViewModel, serviceProvider);
            _listViewInputViewModel = new ListViewInputViewModel(serviceProvider, this);
            InitialInformationViewModel.CreateReturnProtocolEvent += CreateProtocol;
            ReturnProductsListViewModel.ReturnedProductCreate += AddReturnedProduct;
            InitialInformationViewModel.SelectedReturnProtocolEvent += SelectProtocol;
            InitialInformationViewModel.DeleteReturnProtocolEvent += DeleteProtocol;
        }
        public event Action<ReturnProtocolViewModel> ReturnProtocolSelected;
        public event Action ReturnProtocolUnSelected;

        public InitialInformationViewModel InitialInformationViewModel => _initialInformationViewModel;

        public ListViewInputViewModel ReturnProductsListViewModel => _listViewInputViewModel;

        private async void CreateProtocol(ReturnProtocolViewModel returnProtocol)
        {
            _currentProtocol = returnProtocol;
            ReturnProtocolSelected(returnProtocol);
            var service = _serviceProvider.GetService<ReturnProtocolService>() ?? throw new InvalidOperationException($"Service {nameof(ReturnProtocolService)} not available");
            _currentReturnProtocolId = await service.CreateProtocolAsync(returnProtocol);
            returnProtocol.Id = _currentReturnProtocolId;
        }

        private async void SelectProtocol(ReturnProtocolViewModel returnProtocol)
        {
            _currentProtocol = returnProtocol;
            _currentReturnProtocolId = returnProtocol.Id;
            await InitialInformationViewModel.LoadProductsTask;
            ReturnProtocolSelected(returnProtocol);
        }

        private void DeleteProtocol(ReturnProtocolViewModel returnProtocol)
        {
            var service = _serviceProvider.GetService<ReturnProtocolService>() ?? throw new InvalidOperationException($"Service {nameof(ReturnProtocolService)} not available");
            service.DeleteProtocol(returnProtocol.Id);
            _currentProtocol = null;
            ReturnProtocolUnSelected();
        }

        private async Task<int> AddReturnedProduct(ReturnedProductViewModel returnProduct)
        {
            _currentProtocol.Products.Add(returnProduct);
            var service = _serviceProvider.GetService<ReturnProductService>() ?? throw new InvalidOperationException($"Service {nameof(ReturnProductService)} not available");
            returnProduct.Id = await service.AddProductAsync(returnProduct, _currentReturnProtocolId);
            return returnProduct.Id;
        }
    }
}
