using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DelitaTrade.Core.ViewModels;

namespace DelitaTrade.Core.Contracts
{
    public interface IProductDescriptionService
    {
        Task<IEnumerable<ReturnedProductDescriptionViewModel>> GetAllAsync();

        Task<IEnumerable<ReturnedProductDescriptionViewModel>> GetFilteredDescriptions(string[] args);

        Task<ReturnedProductDescriptionViewModel> AddDescriptionAsync(ReturnedProductDescriptionViewModel description);
    }
}
