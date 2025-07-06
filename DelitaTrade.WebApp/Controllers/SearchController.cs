using DelitaTrade.Core.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace DelitaTrade.WebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SearchController(IProductService productService, ICompanyObjectService companyObjectService, IProductDescriptionService productDescriptionService) : Controller
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
            if (products.Any() == false)
            {
                return NoContent();
            }
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
            if (companyObjects.Any() == false)
            {
                return NoContent();
            }
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

        [HttpGet]
        [Route("descriptions")]
        public async Task<IActionResult> Descriptions(string? data)
        {
            if (string.IsNullOrEmpty(data))
            {
                return Json(new { success = false, message = "No description provided." });
            }
            var descriptions = await productDescriptionService.GetFilteredDescriptions(data.Split(' '));
            if (descriptions.Any() == false)
            {
                return NoContent();
            }
            object result = descriptions.Select(d => new
            {
                d.Id,
                d.Description
            });            
            return Ok(result);
        }
    }
}
