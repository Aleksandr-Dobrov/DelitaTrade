using DelitaTrade.Core.Exporters.ExportedModels;
using DelitaTrade.Core.Interfaces;
using DelitaTrade.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelitaTrade.Core.Factories
{
    public static class ReturnProtocolFactory
    {
        public static IExportReturnProtocol CreateExportedReturnProtocol(this ReturnProtocolViewModel returnProtocolViewModel)
        {
            return new ReturnProtocolExportedModel(returnProtocolViewModel.Id.ToString(),
                                                   returnProtocolViewModel.ReturnedDate.ToString("yyyy-MM-dd"),
                                                   returnProtocolViewModel.CompanyObject.Name,
                                                   $"{returnProtocolViewModel.CompanyObject.Address?.Town ?? ""}" +
                                                   $" {returnProtocolViewModel.CompanyObject.Address?.StreetName ?? ""}" +
                                                   $" {returnProtocolViewModel.CompanyObject.Address?.Number ?? ""}" + 
                                                   $" {returnProtocolViewModel.CompanyObject.Address?.Description ?? ""}",
                                                   returnProtocolViewModel.Trader.Name,
                                                   returnProtocolViewModel.PayMethod,
                                                   returnProtocolViewModel.User.Name,
                                                   GetProducts(returnProtocolViewModel.Products));
        }
        private static IEnumerable<IExportedReturnedProduct> GetProducts(this IEnumerable<ReturnedProductViewModel> ReturnProduct)
        {
            List<ReturnProductExportedModel> productsToExport = [];
            foreach (var product in ReturnProduct)
            {
                productsToExport.Add(new ReturnProductExportedModel(product.Product.Number != null ? $"({product.Product.Number})" + product.Product.Name : product.Product.Name,
                                                                    product.Quantity,
                                                                    product.Product.Unit,
                                                                    product.Batch,
                                                                    product.BestBefore.ToString("yyyy-MM-dd"),
                                                                    product.Description?.Description ?? string.Empty,
                                                                    product.DescriptionCategory.Name));
            }
            return productsToExport;
        }
    }
}
