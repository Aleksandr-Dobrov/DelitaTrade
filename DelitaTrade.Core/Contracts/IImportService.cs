using DelitaTrade.Core.Models.ImportModels;
using DelitaTrade.Core.ViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelitaTrade.Core.Contracts
{
    public interface IImportService
    {
        Task<IEnumerable<ProductViewModel>> ImportProductsAsync(IFormFile file);
        Task<DayReportJsonImportModel?> ImportDeliveriesAsync(IFormFile file);
        Task<IEnumerable<ProductViewModel>> ImportProductsAsync(string filePath);
    }
}
