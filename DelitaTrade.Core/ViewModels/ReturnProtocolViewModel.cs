using System.Collections.ObjectModel;

namespace DelitaTrade.Core.ViewModels
{
    public class ReturnProtocolViewModel
    {
        public int Id { get; set; }
        public DateTime ReturnedDate { get; set; }
        public required string PayMethod{ get; set; }
        public required CompanyObjectViewModel CompanyObject { get; set; }
        public required TraderViewModel Trader { get; set; }
        public required UserViewModel User { get; set; }
        public ObservableCollection<ReturnedProductViewModel> Products { get; set; } = new ObservableCollection<ReturnedProductViewModel>();

        public override string ToString()
        {
            return $"{CompanyObject.Name} - {ReturnedDate:dd-MM-yy} - {Trader.Name}";
        }

        public bool RemoveProductById(int id)
        {
            var productToRemove = Products.FirstOrDefault(p => p.Id == id);
            if (productToRemove == null) return false;
            Products.Remove(productToRemove);
            return true;
        }
    }
}
