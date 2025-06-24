﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DelitaTrade.Core.ViewModels;

namespace DelitaTrade.Core.Contracts
{
    public interface IProductService
    {
        Task<IEnumerable<ProductViewModel>> GetAllAsync();
        Task<IEnumerable<ProductViewModel>> GetProductsAsync(string name);
        Task<IEnumerable<ProductViewModel>> GetFilteredProductsAsync(string[] args, int limit);
        Task AddProductAsync(ProductViewModel dtoProduct);
        Task<int> AddRangeProductAsync(IEnumerable<ProductViewModel> dtoProduct);
    }
}
