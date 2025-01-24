﻿using System;
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

        Task<IEnumerable<ReturnedProductViewModel>> GetAllProductsAsync(int protocolId);

        Task DeleteProductAsync(int productId);
    }
}
