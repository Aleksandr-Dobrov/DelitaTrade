using DelitaTrade.Core.Interfaces;
using DelitaTrade.Infrastructure.Data.Models;

namespace DelitaTrade.Core.ViewModels
{
    public class ProductViewModel : INamed
    {
        public ProductViewModel() { }
        public ProductViewModel(Product product)
        {
            Name = product.Name;
            Unit = product.Unit;
            Number = product.Number;
        }
        public string Name { get; set; }

        public string Unit {  get; set; }

        public string? Number { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
