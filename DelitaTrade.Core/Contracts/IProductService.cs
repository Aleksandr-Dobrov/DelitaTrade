using DelitaTrade.Core.ViewModels;
using DelitaTrade.Core.ViewModels.ProductsManagementModels;

namespace DelitaTrade.Core.Contracts
{
    public interface IProductService
    {
        Task<IEnumerable<ProductViewModel>> GetAllAsync();
        Task<IEnumerable<ProductViewModel>> GetProductsAsync(string name);
        Task<IEnumerable<ProductViewModel>> GetFilteredProductsAsync(string[] args, int limit);
        Task AddProductAsync(ProductViewModel dtoProduct);
        Task<int> AddRangeProductAsync(IEnumerable<ProductViewModel> dtoProduct);
        Task CreateProduct(CreateProductInputModel model);
        Task EditProduct(EditProductInputModel model);
    }
}
