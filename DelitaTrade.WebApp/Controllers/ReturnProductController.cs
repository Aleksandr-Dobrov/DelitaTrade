using DelitaTrade.Core.Contracts;
using DelitaTrade.Core.ViewModels;
using DelitaTrade.Common.Extensions;
using DelitaTrade.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static DelitaTrade.Common.Constants.DelitaIdentityConstants.RoleNames;
using static DelitaTrade.Common.Constants.AppMessageConstants;
using static DelitaTrade.Common.Constants.ApplicationMessages.ReturnProductMessages;

namespace DelitaTrade.WebApp.Controllers
{

    [Authorize(Roles = DriverRole)]
    public class ReturnProductController(
            IDescriptionCategoryService descriptionCategoryService,
            IReturnProductService returnProductService, 
            IReturnProtocolService returnProtocolService,
            UserManager<DelitaUser> userManager) : BaseController(userManager)
    {
        [HttpGet]
        public async Task<IActionResult> Create(int id)
        {
            try
            {
                if (await returnProtocolService.IsApproved(id))
                {
                    TempData[Warning] = IsApproved;
                    return RedirectToAction(nameof(ReturnProtocolController.Details), nameof(ReturnProtocolController).GetControllerName(), new { Id = id });
                }
                ReturnedProductInputModel model = new ReturnedProductInputModel();
                model.ReturnProtocolId = id;
                model.DescriptionCategories = await descriptionCategoryService.GetAllAsync();

                return View(model);
            }
            catch (Exception)
            {
                TempData[Error] = AddError;
                return RedirectToAction(nameof(ReturnProtocolController.Details), nameof(ReturnProtocolController).GetControllerName(), new { Id = id });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(ReturnedProductInputModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    model.DescriptionCategories = await descriptionCategoryService.GetAllAsync();
                    return View(model);
                }

                var userViewModel = await GetUserViewModelAsync();            

                await returnProductService.AddProductAsync(model, model.ReturnProtocolId, userViewModel);

                TempData[Success] = AddSuccess;
                return RedirectToAction(nameof(ReturnProtocolController.Details), nameof(ReturnProtocolController).GetControllerName(), new { Id = model.ReturnProtocolId });
            }
            catch (Exception)
            {
                TempData[Error] = AddError;
                return RedirectToAction(nameof(ReturnProtocolController.Details), nameof(ReturnProtocolController).GetControllerName(), new { Id = model.ReturnProtocolId });
            }

        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var productToEdit = await returnProductService.GetProductByIdAsync(id, await GetUserViewModelAsync());
                if (productToEdit == null)
                {
                    return Unauthorized();
                }
                if (await returnProtocolService.IsApproved(productToEdit.ReturnProtocolId))
                {
                    TempData[Warning] = IsApproved;
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
            catch (Exception)
            {
                TempData[Error] = UpdateError;
                return RedirectToAction(nameof(ReturnProtocolController.Details), nameof(ReturnProtocolController).GetControllerName(), new { Id = id });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ReturnProductEditModel model)
        {
            try
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
                        : model.Description != null ? 
                        new ReturnedProductDescriptionViewModel 
                        {
                            Description = model.Description,
                        }
                        : null;


                await returnProductService.UpdateProductAsync(productToEdit);
                TempData[Success] = UpdateSuccess;
                return RedirectToAction(nameof(ReturnProtocolController.Details), nameof(ReturnProtocolController).GetControllerName(), new { Id = productToEdit.ReturnProtocolId });
            }
            catch (Exception)
            {
                TempData[Error] = UpdateError;
                return RedirectToAction(nameof(ReturnProtocolController.Details), nameof(ReturnProtocolController).GetControllerName(), new { Id = model.ReturnProtocolId });
            }

        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var userViewModel = await GetUserViewModelAsync();
                var productToDelete = await returnProductService.GetProductByIdAsync(id, userViewModel);

                if (productToDelete == null)
                {
                    return Unauthorized();
                }
                if (await returnProtocolService.IsApproved(productToDelete.ReturnProtocolId))
                {
                    TempData[Warning] = IsApproved;
                    return RedirectToAction(nameof(ReturnProtocolController.Details), nameof(ReturnProtocolController).GetControllerName(), new { Id = productToDelete.ReturnProtocolId });
                }

                await returnProductService.DeleteProductAsync(id, userViewModel);
                TempData[Success] = string.Format(DeleteSuccess, productToDelete.Product.Name);
                return RedirectToAction(nameof(ReturnProtocolController.Details), nameof(ReturnProtocolController).GetControllerName(), new { Id = productToDelete.ReturnProtocolId });
            }
            catch (Exception)
            {
                TempData[Error] = DeleteError;
                return RedirectToAction(nameof(ReturnProtocolController.Index), nameof(ReturnProtocolController).GetControllerName());
            }
        }
    }
}
