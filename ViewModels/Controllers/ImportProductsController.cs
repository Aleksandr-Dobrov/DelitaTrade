using DelitaTrade.Commands;
using DelitaTrade.Common;
using DelitaTrade.Core.Contracts;
using DelitaTrade.Core.ViewModels;
using DelitaTrade.Infrastructure.Data.Models;
using DelitaTrade.Models.JsonModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DelitaTrade.ViewModels.Controllers
{
    public class ImportProductsController
    {
        private IServiceProvider _serviceProvider;
        public ImportProductsController(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            ImportFile = new DefaultCommand(ImportProducts);
        }
        public ICommand ImportFile { get; }

        private async Task ImportProducts()
        {
            string filePath = GetFilePath()!;
            if (filePath == null) return;

            string jsonProducts = File.ReadAllText(filePath);

            var products = JsonSerializer.Deserialize<ProductJsonModel[]>(jsonProducts);

            if (products == null) return;

            var DelitaProducts = ParseProducts(products);
            if (DelitaProducts == null || DelitaProducts.Any() == false) return;
            using var scope = _serviceProvider.CreateScope();
            var productService = scope.GetService<IProductService>();

            int changes = await productService.AddRangeProductAsync(DelitaProducts);
            string message = string.Empty;

            if (changes > 0)
            {
                message = $"Successes import {changes} products";
            }
            else 
            {
                message = "No new product to import";
            }
            MessageBox.Show(message);           
        }

        private string? GetFilePath()
        {
            OpenFileDialog ofd = new OpenFileDialog();

            bool? result = ofd.ShowDialog();

            if (result == true)
            {
                FileInfo fileInfo = new FileInfo(ofd.FileName);
                return fileInfo.FullName;
            }
            return null;
        }

        private IEnumerable<ProductViewModel> ParseProducts(ProductJsonModel[] products)
        {            
            List<ProductViewModel> result = new List<ProductViewModel>();
            foreach (var product in products) 
            {
                result.Add(product.Parse());                
            }
            return result;            
        }
    }
}
