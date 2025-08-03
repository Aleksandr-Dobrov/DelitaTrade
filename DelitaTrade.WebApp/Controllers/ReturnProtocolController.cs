using DelitaTrade.Common.Constants;
using DelitaTrade.Common.Extensions;
using DelitaTrade.Core.Contracts;
using DelitaTrade.Core.ModelBinders;
using DelitaTrade.Core.ViewModels;
using DelitaTrade.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static DelitaTrade.Common.Constants.DelitaIdentityConstants.RoleNames;
using static DelitaTrade.Common.Constants.AppMessageConstants;
using static DelitaTrade.Common.Constants.ApplicationMessages.ReturnProtocolMessages;

namespace DelitaTrade.WebApp.Controllers
{
    [Authorize(Roles = $"{AdminRole},{DriverRole},{WarehouseManagerRole}")]
    public class ReturnProtocolController(
            ITraderService traderService, 
            IReturnProtocolService returnProtocolService,
            ICompanyObjectService companyObjectService, 
            UserManager<DelitaUser> userManager) 
        : BaseController(userManager)
    {
        [HttpGet]
        public async Task<IActionResult> Index(SearchReturnProtocolInputModel? searchModel)
        {
            try
            {
                if (searchModel == null)
                {
                    searchModel = new SearchReturnProtocolInputModel();
                }
                searchModel.Traders = await traderService.GetAllAsync();
                return View(searchModel);
            }
            catch (Exception)
            {
                TempData[Error] = ReturnProtocolError;
                return RedirectToAction(nameof(HomeController.Index), nameof(HomeController).GetControllerName());
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetProtocols(SearchReturnProtocolInputModel searchModel)
        {
            try
            {
                var userViewModel = await GetUserViewModelAsync();
                
                searchModel.ReturnProtocols = await returnProtocolService.GetSimpleFilteredAsync(userViewModel, searchModel.TraderName, searchModel.CompanyObjectName, searchModel.StartDate, searchModel.EndDate);               
           
                searchModel.Traders = await traderService.GetAllAsync();
                if (searchModel.ReturnProtocols == null)
                {
                    return NotFound();
                }
                int resultCount = searchModel.ReturnProtocols.Count();
                if (resultCount == 0)
                {
                    TempData[Info] = SearchNoResult;
                }
                else
                {
                    TempData[Success] = string.Format(SearchComplete, resultCount);
                }
                return View(nameof(Index), searchModel);
            }
            catch (Exception)
            {
                TempData[Error] = SearchError;
                return RedirectToAction(nameof(HomeController.Index), nameof(HomeController).GetControllerName());
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var userViewModel = await GetUserViewModelAsync();
                var returnProtocol = await returnProtocolService.GetByIdAsync(userViewModel, id);
                if (returnProtocol == null)
                {
                    return NotFound();
                }
                return View(returnProtocol);
            }
            catch(Exception)
            {
                TempData[Error] = ReturnProtocolNotFound;
                return Redirect(nameof(Index));
            }
        }

        [HttpGet]
        [Authorize(Roles = DriverRole)]
        public async Task<IActionResult> Create()
        {
            try
            {
                if (IsUserAuthenticated() == false)
                {
                    return Unauthorized();
                }
                var traders = await traderService.GetAllAsync();
                var returnProtocol = new ReturnProtocolInputModel();
                returnProtocol.Traders = traders;
                returnProtocol.PayMethods = new List<string>
                {
                    ReturnProtocolPayMethods.BankPay,
                    ReturnProtocolPayMethods.Deducted,
                    ReturnProtocolPayMethods.NotDeducted,
                    ReturnProtocolPayMethods.ForCancellation
                };
                return View(returnProtocol);
            }
            catch (Exception) 
            {
                TempData[Error] = CreateError;
                return Redirect(nameof(Index));
            }
        }

        [HttpPost]
        [Authorize(Roles = DriverRole)]
        public async Task<IActionResult> Create(ReturnProtocolInputModel returnProtocolInputModel)
        {
            try
            {
                if (ModelState.IsValid == false)
                {
                    var traders = await traderService.GetAllAsync();
                    returnProtocolInputModel.Traders = traders;
                    returnProtocolInputModel.PayMethods = new List<string>
                {
                    ReturnProtocolPayMethods.BankPay,
                    ReturnProtocolPayMethods.Deducted,
                    ReturnProtocolPayMethods.NotDeducted,
                    ReturnProtocolPayMethods.ForCancellation
                };
                    return View(returnProtocolInputModel);
                }
                var userViewModel = await GetUserViewModelAsync();
                var protocolViewModel = new ReturnProtocolViewModel()
                {
                    User = userViewModel,
                    ReturnedDate = returnProtocolInputModel.ReturnDate ?? DateTime.Now,
                    PayMethod = returnProtocolInputModel.PayMethod,
                    Trader = await traderService.GetByIdAsync(returnProtocolInputModel.TraderId),
                    CompanyObject = await companyObjectService.GetDetailedByIdAsync(returnProtocolInputModel.CompanyObjectId),
                };

                int protocolId = await returnProtocolService.CreateProtocolAsync(protocolViewModel);
                TempData[Success] = string.Format(CreateSuccess, protocolViewModel.CompanyObject.Name);
                return RedirectToAction(nameof(Details), new { id = protocolId });
            }
            catch (Exception)
            {
                TempData[Error] = CreateError;
                return Redirect(nameof(Index));
            }
        }

        [HttpGet]
        [Authorize(Roles = DriverRole)]
        public async Task<IActionResult> Edit(int id) 
        {
            try
            {
                if (IsUserAuthenticated() == false)
                {
                    return Unauthorized();
                }
                var userViewModel = await GetUserViewModelAsync();
                var returnProtocolToEdit = await returnProtocolService.GetEditableByIdAsync(userViewModel, id);

                if (returnProtocolToEdit == null)
                {
                    return NotFound();
                }

                if (IsApproved(returnProtocolToEdit))
                {
                    TempData[Warning] = IsAlreadyApproved;
                    return RedirectToAction(nameof(Details), new { id = returnProtocolToEdit.Id });
                }

                var traders = await traderService.GetAllAsync();

                var returnProtocol = new ReturnProtocolEditModel();
                returnProtocol.Id = returnProtocolToEdit.Id;
                returnProtocol.ReturnDate = returnProtocolToEdit.ReturnedDate;
                returnProtocol.PayMethod = returnProtocolToEdit.PayMethod;
                returnProtocol.CompanyObjectName = returnProtocolToEdit.CompanyObject.Name;
                returnProtocol.CompanyObjectId = returnProtocolToEdit.CompanyObject.Id;
                returnProtocol.TraderId = returnProtocolToEdit.Trader.Id;

                returnProtocol.Traders = traders;
                returnProtocol.PayMethods = new List<string>
                {
                    ReturnProtocolPayMethods.BankPay,
                    ReturnProtocolPayMethods.Deducted,
                    ReturnProtocolPayMethods.NotDeducted,
                    ReturnProtocolPayMethods.ForCancellation
                };

                return View(returnProtocol);
            }
            catch (Exception)
            {
                TempData[Error] = UpdateError;
                return RedirectToAction(nameof(Details), new { Id = id });
            }
        }

        [HttpPost]
        [Authorize(Roles = DriverRole)]
        public async Task<IActionResult> Edit(ReturnProtocolEditModel returnProtocolInputModel)
        {
            try
            {
                if (ModelState.IsValid == false)
                {
                    var traders = await traderService.GetAllAsync();
                    returnProtocolInputModel.Traders = traders;
                    returnProtocolInputModel.PayMethods = new List<string>
                    {
                        ReturnProtocolPayMethods.BankPay,
                        ReturnProtocolPayMethods.Deducted,
                        ReturnProtocolPayMethods.NotDeducted,
                        ReturnProtocolPayMethods.ForCancellation
                    };
                    return View(returnProtocolInputModel);
                }

                var userViewModel = await GetUserViewModelAsync();
                var returnProtocolToUpdate = new ReturnProtocolViewModel()
                {
                    Id = returnProtocolInputModel.Id,
                    User = userViewModel,
                    ReturnedDate = returnProtocolInputModel.ReturnDate ?? DateTime.Now,
                    PayMethod = returnProtocolInputModel.PayMethod,
                    Trader = await traderService.GetByIdAsync(returnProtocolInputModel.TraderId),
                    CompanyObject = await companyObjectService.GetDetailedByIdAsync(returnProtocolInputModel.CompanyObjectId),
                };
                await returnProtocolService.UpdateProtocolAsync(returnProtocolToUpdate);

                TempData[Success] = UpdateSuccess;
                return RedirectToAction(nameof(Details), new { id = returnProtocolInputModel.Id });
            }
            catch (Exception)
            {
                TempData[Error] = UpdateError;
                return RedirectToAction(nameof(Details), new { id = returnProtocolInputModel.Id });
            }
        }

        [HttpGet]
        [Authorize(Roles = DriverRole)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var userViewModel = await GetUserViewModelAsync();
                var returnProtocolToDelete = await returnProtocolService.GetEditableByIdAsync(userViewModel, id);
                if (returnProtocolToDelete == null)
                {
                    return NotFound();
                }

                if (IsApproved(returnProtocolToDelete))
                {
                    TempData[Warning] = IsAlreadyApproved;
                    return RedirectToAction(nameof(Details), new { id = returnProtocolToDelete.Id });
                }

                var returnProtocol = new ReturnProtocolEditModel();
                returnProtocol.Id = returnProtocolToDelete.Id;
                returnProtocol.ReturnDate = returnProtocolToDelete.ReturnedDate;
                returnProtocol.CompanyObjectName = returnProtocolToDelete.CompanyObject.Name;

                return View(returnProtocol);
            }
            catch (Exception)
            {
                TempData[Error] = DeleteError;
                return RedirectToAction(nameof(Details), new { Id = id });
            }
        }

        [HttpPost]
        [Authorize(Roles = DriverRole)]
        public async Task<IActionResult> Delete(ReturnProtocolEditModel returnProtocol)
        {
            try
            {
                var userViewModel = await GetUserViewModelAsync();
                var returnProtocolToDelete = await returnProtocolService.GetEditableByIdAsync(userViewModel, returnProtocol.Id);
                if (returnProtocolToDelete == null)
                {
                    return NotFound();
                }
                await returnProtocolService.DeleteProtocolAsync(returnProtocolToDelete.Id);
                TempData[Success] = string.Format(DeleteSuccess, returnProtocolToDelete.CompanyObject.Name);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                TempData[Error] = DeleteError;
                return RedirectToAction(nameof(Details), new { Id = returnProtocol.Id });
            }
        }

        [HttpPost]
        [Authorize(Roles = WarehouseManagerRole)]
        public async Task<IActionResult> Approve([ModelBinder(typeof(ApproveProductsModelBinder))]ReturnProtocolApproveModel detailReturnProtocol)
        {
            try
            {
                var userViewModel = await GetUserViewModelAsync();
                detailReturnProtocol.Approver = userViewModel;

                if (ModelState.IsValid == false || detailReturnProtocol.Approver == null)
                {
                    TempData[Error] = ApproveError;
                    return RedirectToAction(nameof(Details), new { detailReturnProtocol.Id });
                }

                var returnProtocol = await returnProtocolService.GetByIdAsync(userViewModel, detailReturnProtocol.Id);
                if (returnProtocol == null)
                {
                    return NotFound();
                }
                if (IsDateTimeIdentical(detailReturnProtocol.LastChange, returnProtocol.LastChange) == false)
                {
                    ModelState.AddModelError(nameof(detailReturnProtocol.LastChange), "The return protocol has been modified by another user. Please refresh the page and try again.");
                    TempData[Warning] = IsModify;
                    return RedirectToAction(nameof(Details), new { detailReturnProtocol.Id });
                }

                await returnProtocolService.ApproveAsync(detailReturnProtocol, userViewModel);
                TempData[Success] = ApproveSuccess;
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                TempData[Error] = ApproveError;
                return RedirectToAction(nameof(Details), new { detailReturnProtocol.Id });
            }
        }

        private static bool IsApproved(EditableReturnProtocolViewModel returnProtocol)
        {
            return returnProtocol.ApproverName != null;
        }

        private static bool IsDateTimeIdentical(DateTime? first, DateTime? second)
        {
            if (first == null || second == null)
            {
                return true;
            }

            return first.Value.Year == second.Value.Year
                && first.Value.Month == second.Value.Month
                && first.Value.Day == second.Value.Day
                && first.Value.Hour == second.Value.Hour
                && first.Value.Minute == second.Value.Minute
                && first.Value.Second == second.Value.Second;
        }
    }
}
