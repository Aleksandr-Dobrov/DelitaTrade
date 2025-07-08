using DelitaTrade.Core.Contracts;
using DelitaTrade.Core.ViewModels;
using DelitaTrade.Common.Extensions;
using DelitaTrade.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static DelitaTrade.Common.Constants.DelitaIdentityConstants.RoleNames;

namespace DelitaTrade.WebApp.Controllers
{

    [Authorize(Roles = Driver)]
    public class ReturnProductController(
            IDescriptionCategoryService descriptionCategoryService,
            IReturnProductService returnProductService, 
            IReturnProtocolService returnProtocolService,
            UserManager<DelitaUser> userManager) : BaseController(userManager)
    {
        [HttpGet]
        public async Task<IActionResult> Create(int id)
        {
            if (await returnProtocolService.IsApproved(id))
            {
                return RedirectToAction(nameof(ReturnProtocolController.Details), nameof(ReturnProtocolController).GetControllerName(), new { Id = id });
            }
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
            var productToEdit = await returnProductService.GetProductByIdAsync(id, await GetUserViewModelAsync());
            if (productToEdit == null)
            {
                return Unauthorized();
            }
            if (await returnProtocolService.IsApproved(productToEdit.ReturnProtocolId))
            {
                return RedirectToAction(nameof(ReturnProtocolController.Details), nameof(ReturnProtocolController).GetControllerName(), new { Id = productToEdit.ReturnProtocolId });
            }

            var model = new ReturnProductEditModel()
            {
                Id = productToEdit.Id,
                Batch = productToEdit.Batch,
                BestBefore = productToEdit.BestBefore,
                Quantity = productToEdit.Quantity,
                ProductName = productToEdit.Product.Name,
                Unit = productToEdit.Product.Unit,
                DescriptionId = productToEdit.Description?.Id,
                Description = productToEdit.Description?.Description,
                DescriptionCategoryId = productToEdit.DescriptionCategory.Id,
                DescriptionCategories = await descriptionCategoryService.GetAllAsync()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ReturnProductEditModel model)
        {
            if (!ModelState.IsValid)
            {
                model.DescriptionCategories = await descriptionCategoryService.GetAllAsync();
                return View(model);
            }
            
            var productToEdit = await returnProductService.GetProductByIdAsync(model.Id, await GetUserViewModelAsync());
            if (productToEdit == null)
            {
                return Unauthorized();
            }

            productToEdit.Batch = model.Batch;
            productToEdit.BestBefore = model.BestBefore;
            productToEdit.Quantity = model.Quantity;
            productToEdit.Product.Name = model.ProductName;
            productToEdit.Product.Unit = model.Unit;
            if(productToEdit.DescriptionCategory.Id != model.DescriptionCategoryId)
            {
                productToEdit.DescriptionCategory = await descriptionCategoryService.GetByIdAsync(model.DescriptionCategoryId);
            }
            productToEdit.Description = model.DescriptionId != null
                    ? new ReturnedProductDescriptionViewModel
                    {
                        Id = model.DescriptionId.Value,
                        Description = model.Description ?? string.Empty
                    }
                    : null;


            await returnProductService.UpdateProductAsync(productToEdit);

            return RedirectToAction("Details", "ReturnProtocol", new { Id = productToEdit.ReturnProtocolId });

        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var userViewModel = await GetUserViewModelAsync();
            var productToEdit = await returnProductService.GetProductByIdAsync(id, await GetUserViewModelAsync());

            if (productToEdit == null)
            {
                return Unauthorized();
            }
            if (await returnProtocolService.IsApproved(productToEdit.ReturnProtocolId))
            {
                return RedirectToAction(nameof(ReturnProtocolController.Details), nameof(ReturnProtocolController).GetControllerName(), new { Id = productToEdit.ReturnProtocolId });
            }

            await returnProductService.DeleteProductAsync(id, userViewModel);

            return RedirectToAction("Details", "ReturnProtocol", new { Id = productToEdit.ReturnProtocolId });
        }
    }
}
