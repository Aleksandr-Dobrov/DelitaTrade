using DelitaTrade.Components.ComponentsViewModel.ReturnProtocolComponentViewModels;
using DelitaTrade.Models.ReturnProtocol;

namespace DelitaTrade.ViewModels.ReturnProtocolViewModels
{
    public class ReturnProtocolViewModel : ViewModelBase
    {
        private ReturnProtocolServices returnProtocolServices;

        private InitialInformationViewModel _initialInformationViewModel;

        public ReturnProtocolViewModel(ViewModelBase addNewCompanyViewModel)
        {
            _initialInformationViewModel = new InitialInformationViewModel(addNewCompanyViewModel);
        }

        public InitialInformationViewModel InitialInformationViewModel => _initialInformationViewModel;
    }
}
