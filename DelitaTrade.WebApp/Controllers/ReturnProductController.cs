using DelitaTrade.Core.Contracts;
using DelitaTrade.Core.ViewModels;
using DelitaTrade.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DelitaTrade.WebApp.Controllers
{
    public class ReturnProductController(IDescriptionCategoryService descriptionCategoryService,IReturnProductService returnProductService, UserManager<DelitaUser> userManager) : BaseController(userManager)
    {
        [HttpGet]
        public async Task<IActionResult> Create(int id)
        {
            ReturnedProductInputModel model = new ReturnedProductInputModel();
            model.ReturnProtocolId = id;
            model.DescriptionCategories = await descriptionCategoryService.GetAllAsync();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ReturnedProductInputModel model)
        {
            if (!ModelState.IsValid)
            {
                model.DescriptionCategories = await descriptionCategoryService.GetAllAsync();
                return View(model);
            }
            var userViewModel = await GetUserViewModelAsync();
            var returnedProduct = new ReturnedProductViewModel
            {
                Batch = model.Batch,
                BestBefore = model.BestBefore,
                Quantity = model.Quantity,
                Product = new ProductViewModel()
                {
                    Name = model.ProductName,
                    Unit = model.Unit
                },
                DescriptionCategory = await descriptionCategoryService.GetByIdAsync(model.DescriptionCategoryId),
                Description = model.DescriptionId != null 
                    ? new ReturnedProductDescriptionViewModel
                    {
                        Id = model.DescriptionId.Value,
                        Description = model.Description ?? string.Empty
                    } 
                    : null
            };

            await returnProductService.AddProductAsync(returnedProduct, model.ReturnProtocolId, userViewModel);

            return RedirectToAction("Details", "ReturnProtocol", new { Id = model.ReturnProtocolId });

        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            // Assuming you have a service to get the return product by id
            // var returnProduct = await _returnProductService.GetByIdAsync(id);
            // if (returnProduct == null)
            // {
            //     return NotFound();
            // }
            // var model = new ReturnProductInputModel
            // {
            //     Id = returnProduct.Id,
            //     Name = returnProduct.Name,
            //     Quantity = returnProduct.Quantity,
            //     Price = returnProduct.Price
            // };
            // return View(model);
            return View(); // Placeholder for actual implementation
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ReturnedProductInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var userViewModel = await GetUserViewModelAsync();
            // Assuming you have a service to update the return product
            // var returnProduct = await _returnProductService.GetByIdAsync(model.Id);
            // if (returnProduct == null)
            // {
            //     return NotFound();
            // }
            // returnProduct.Name = model.Name;
            // returnProduct.Quantity = model.Quantity;
            // returnProduct.Price = model.Price;
            // returnProduct.ModifiedBy = userViewModel.Id;
            // returnProduct.ModifiedOn = DateTime.UtcNow;
            // await _returnProductService.UpdateAsync(returnProduct);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var userViewModel = await GetUserViewModelAsync();


            return View(); // Placeholder for actual implementation
        }
    }
}
