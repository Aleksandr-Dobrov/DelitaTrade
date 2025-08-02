using DelitaTrade.Core.Contracts;
using DelitaTrade.Core.ViewModels.ProductsManagementModels;
using DelitaTrade.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DelitaTrade.WebApp.Areas.Admin.Controllers
{
    public class ProductsManagementController(IProductService productService,
            IImportService importProductsService,
            UserManager<DelitaUser> userManager) : BaseAdminController(userManager)
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProductInputModel model)
        {
            await productService.CreateProduct(model);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditProductInputModel model)
        {
            try
            {
                await productService.EditProduct(model);
                return RedirectToAction(nameof(Index));
            }
            catch (NotImplementedException)
            {
                ModelState.AddModelError(string.Empty, "Edit functionality is not implemented yet.");
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Import(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError(string.Empty, "Please select a file to upload.");
                return RedirectToAction(nameof(Index));
            }
            try
            {
                var products = await importProductsService.ImportProductsAsync(file);
                if (products == null || !products.Any())
                {
                    ModelState.AddModelError(string.Empty, "No valid products found in the file.");
                    return RedirectToAction(nameof(Index));
                }
                int changes = await productService.AddRangeProductAsync(products);
                //TODO: Add a success message to the view
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"An error occurred while importing products: {ex.Message}");
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
