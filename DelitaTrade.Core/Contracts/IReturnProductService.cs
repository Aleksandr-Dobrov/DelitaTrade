using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DelitaTrade.Core.ViewModels;

namespace DelitaTrade.Core.Contracts
{
    public interface IReturnProductService
    {
        Task<int> AddProductAsync(ReturnedProductViewModel returnedProduct, int protocolId);

        Task<int> AddProductAsync(ReturnedProductViewModel returnedProduct, int protocolId, UserViewModel user);

        Task<IEnumerable<ReturnedProductViewModel>> GetAllProductsAsync(int protocolId);
        Task<ReturnedProductViewModel?> GetProductByIdAsync(int Id, UserViewModel userViewModel);

        Task UpdateProductAsync(ReturnedProductViewModel returnedProduct);

        Task DeleteProductAsync(int productId);

        Task DeleteProductAsync(int productId, UserViewModel user);
    }
}
