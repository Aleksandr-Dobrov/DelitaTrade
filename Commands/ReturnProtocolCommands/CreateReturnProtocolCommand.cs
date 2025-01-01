using DelitaTrade.Components.ComponentsViewModel.ReturnProtocolComponentViewModels;

namespace DelitaTrade.Commands.ReturnProtocolCommands
{
    public class CreateReturnProtocolCommand : CommandBase
    {
        private InitialInformationViewModel _initialInformationViewModel;
        public CreateReturnProtocolCommand(InitialInformationViewModel informationViewModel) 
        {
            _initialInformationViewModel = informationViewModel;
        }

        public override void Execute(object? parameter)
        {
            _initialInformationViewModel.CreateReturnProtocol(this);
        }
    }
}
