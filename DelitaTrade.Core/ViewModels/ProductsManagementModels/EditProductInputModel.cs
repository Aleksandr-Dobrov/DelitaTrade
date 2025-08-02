namespace DelitaTrade.Core.ViewModels.ProductsManagementModels
{
    public class EditProductInputModel : CreateProductInputModel
    {
        public string? EditProductName { get; set; }
        public string? EditProductUnit { get; set; }
        public string? EditProductNumber { get; set; }
    }
}
