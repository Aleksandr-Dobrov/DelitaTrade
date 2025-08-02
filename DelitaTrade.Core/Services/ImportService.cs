using DelitaTrade.Core.Contracts;
using DelitaTrade.Core.Models.ImportModels;
using DelitaTrade.Core.ViewModels;
using DelitaTrade.Core.ViewModels.ProductsManagementModels;
using DelitaTrade.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace DelitaTrade.Core.Services
{
    public class ImportService : IImportService
    {
        public async Task<DayReportJsonImportModel?> ImportDeliveriesAsync(IFormFile file)
        {
            DayReportJsonImportModel? importModel = null;
            if (file != null && file.Length > 0)
            {
                using (var stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream);
                    stream.Position = 0;

                    importModel = await JsonSerializer.DeserializeAsync<DayReportJsonImportModel>(stream);                    
                }

            }
            return importModel;
        }

        public async Task<IEnumerable<ProductViewModel>> ImportProductsAsync(IFormFile file)
        {
            List<ProductViewModel> products = new List<ProductViewModel>();

            if (file != null && file.Length > 0)
            {                
                using (var stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream);
                    stream.Position = 0;

                    var importModel = await JsonSerializer.DeserializeAsync<ProductJsonModel[]>(stream);

                    if (importModel == null || importModel.Length == 0)
                    {
                        return products; // Return empty list if no products found
                    }

                    foreach (var product in importModel)
                    {
                        if (string.IsNullOrEmpty(product.Name) || string.IsNullOrEmpty(product.Unit) || string.IsNullOrEmpty(product.Number))
                        {
                            continue; // Skip invalid products
                        }
                        products.Add( new ProductViewModel
                        {
                            Name = product.Name,
                            Unit = product.Unit,
                            Number = product.Number
                        });                        
                    }
                }
            }

            return products;
        }

        public async Task<IEnumerable<ProductViewModel>> ImportProductsAsync(string filePath)
        {
            string jsonProducts = File.ReadAllText(filePath);
            ProductJsonModel[]? productJsonModels = null;
            List<ProductViewModel> products = new List<ProductViewModel>();

            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                // Deserialize the JSON content into an array of ProductJsonModel

                productJsonModels = await JsonSerializer.DeserializeAsync<ProductJsonModel[]>(stream);
            }

            if (productJsonModels == null || productJsonModels.Length == 0)
            {
                return products; // Return empty list if no products found
            }


            foreach (var product in productJsonModels)
            {
                if (string.IsNullOrEmpty(product.Name) || string.IsNullOrEmpty(product.Unit) || string.IsNullOrEmpty(product.Number))
                {
                    continue; // Skip invalid products
                }
                products.Add(new ProductViewModel
                {
                    Name = product.Name,
                    Unit = product.Unit,
                    Number = product.Number
                });
            }
            return products;
        }
    }
}
