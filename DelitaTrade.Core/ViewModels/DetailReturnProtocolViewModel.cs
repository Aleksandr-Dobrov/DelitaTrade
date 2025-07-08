namespace DelitaTrade.Core.ViewModels
{
    public class DetailReturnProtocolViewModel : SimpleReturnProtocolViewModel
    {
        public DateTime? LastChange { get; set; }
        public IEnumerable<ReturnedProductViewModel> ReturnedProducts { get; set; } = new List<ReturnedProductViewModel>();

        public IEnumerable<ProductConditionsViewModel> Conditions { get; set; } = new List<ProductConditionsViewModel>()
        {
            new()
            {
                IsScrapped = false,
                Condition = "Годно"
            },
            new()
            {
                IsScrapped = true,
                Condition = "Брак"
            }
        };
    }
}
