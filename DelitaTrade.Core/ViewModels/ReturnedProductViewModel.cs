using DelitaTrade.Core.ViewModels;

namespace DelitaTrade.Core.ViewModels
{
    public class ReturnedProductViewModel
    {
        public int Id { get; set; }
        public required ProductViewModel Product { get; set; }
        public double Quantity { get; set; }
        public required string Batch {  get; set; }
        public DateTime BestBefore { get; set; }
        public required ReturnedProductDescriptionViewModel Description { get; set; }
    }
}
