using DelitaTrade.Infrastructure.Data.Models;

namespace DelitaTrade.Core.ViewModels
{
    public class ProductViewModel
    {
        public ProductViewModel() { }
        public ProductViewModel(Product product)
        {
            Name = product.Name;
            Unit = product.Unit;
        }
        public string Name { get; set; }

        public string Unit {  get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
