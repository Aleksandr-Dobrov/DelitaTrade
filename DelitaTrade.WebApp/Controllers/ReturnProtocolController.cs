using DelitaTrade.Core.Contracts;
using DelitaTrade.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DelitaTrade.WebApp.Controllers
{
    public class ReturnProtocolController(IProductService productService, UserManager<DelitaUser> userManager) : BaseController(userManager)
    {
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> SearchProduct(string? data)
        {
            if (string.IsNullOrEmpty(data))
            {
                return Json(new { success = false, message = "No data provided." });
            }
            var products = await productService.GetFilteredProductsAsync(data.Split(' '));

            object result = products.Select(p => new
            {                
                p.Name,
                p.Unit,
                p.Number
            });

            return Json(result);
        }
    }
}
