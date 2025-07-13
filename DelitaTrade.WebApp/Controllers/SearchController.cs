using DelitaTrade.Core.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace DelitaTrade.WebApp.Controllers
{   
    public class SearchController(IProductService productService, ICompanyObjectService companyObjectService, IProductDescriptionService productDescriptionService) : BaseApiController
    {
        private const int _maxSearchResults = 20;

        [HttpGet]
        [Route("products")]
        public async Task<IActionResult> Products(string? data)
        {
            if (string.IsNullOrEmpty(data))
            {
                return BadRequest();
            }
            var products = await productService.GetFilteredProductsAsync(data.Split(' '), _maxSearchResults);
            if (products.Any() == false)
            {
                return NotFound();
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
                return BadRequest();
            }
            var companyObjects = await companyObjectService.GetFilteredAsync(data.Split(' '), _maxSearchResults);
            if (companyObjects.Any() == false)
            {
                return NotFound();
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
                return BadRequest();
            }
            var descriptions = await productDescriptionService.GetFilteredDescriptions(data.Split(' '));
            if (descriptions.Any() == false)
            {
                return NotFound();
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
