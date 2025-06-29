namespace DelitaTrade.Core.ViewModels
{
    public class DetailReturnProtocolViewModel : SimpleReturnProtocolViewModel
    {
        public IEnumerable<ReturnedProductViewModel> ReturnedProducts { get; set; } = new List<ReturnedProductViewModel>();
    }
}
