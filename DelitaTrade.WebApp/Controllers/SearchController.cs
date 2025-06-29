using DelitaTrade.Core.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace DelitaTrade.WebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SearchController(IProductService productService, ICompanyObjectService companyObjectService) : Controller
    {
        private const int _maxSearchResults = 20;

        [HttpGet]
        [Route("products")]
        public async Task<IActionResult> Products(string? data)
        {
            if (string.IsNullOrEmpty(data))
            {
                return Json(new { success = false, message = "No data provided." });
            }
            var products = await productService.GetFilteredProductsAsync(data.Split(' '), _maxSearchResults);

            object result = products.Select(p => new
            {
                p.Name,
                p.Unit,
                p.Number
            });

            return Ok(result);
        }

        [HttpGet]
        [Route("company-objects")]
        public async Task<IActionResult> CompanyObjects(string? data)
        {
            if (string.IsNullOrEmpty(data))
            {
                return Json(new { success = false, message = "No name provided." });
            }
            var companyObjects = await companyObjectService.GetFilteredAsync(data, _maxSearchResults);
            object result = companyObjects.Select(p => new
            {
                p.Id,
                p.Name,
                CompanyName = $"{p.Company.Name} {p.Company.Type}",
                p.IsBankPay,
                TraderId = p.Trader?.Id,
            });
            return Ok(result);
        }
    }
}
