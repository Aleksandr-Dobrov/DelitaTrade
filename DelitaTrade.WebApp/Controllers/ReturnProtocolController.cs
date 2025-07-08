using DelitaTrade.Common.Constants;
using DelitaTrade.Core.Contracts;
using DelitaTrade.Core.ModelBinders;
using DelitaTrade.Core.ViewModels;
using DelitaTrade.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using static DelitaTrade.Common.Constants.DelitaIdentityConstants.RoleNames;

namespace DelitaTrade.WebApp.Controllers
{
    [Authorize(Roles = $"{Admin},{Driver},{WarehouseManager}")]
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
            if (searchModel == null)
            {
                searchModel = new SearchReturnProtocolInputModel();
            }
            searchModel.Traders = await traderService.GetAllAsync();
            return View(searchModel);
        }

        [HttpGet]
        public async Task<IActionResult> GetProtocols(SearchReturnProtocolInputModel searchModel)
        {
            var userViewModel = await GetUserViewModelAsync();
            
            searchModel.ReturnProtocols = await returnProtocolService.GetSimpleFilteredAsync(userViewModel, searchModel.TraderName, searchModel.CompanyObjectName, searchModel.StartDate, searchModel.EndDate);               
           
            searchModel.Traders = await traderService.GetAllAsync();
            if (searchModel.ReturnProtocols == null)
            {
                return NotFound();
            }

            return View(nameof(Index), searchModel);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var userViewModel = await GetUserViewModelAsync();
            var returnProtocol = await returnProtocolService.GetByIdAsync(userViewModel, id);
            if (returnProtocol == null)
            {
                return NotFound();
            }
            return View(returnProtocol);
        }

        [HttpGet]
        [Authorize(Roles = Driver)]
        public async Task<IActionResult> Create()
        {
            if(IsUserAuthenticated() == false)
            {
                return RedirectToAction("Index", "Home");
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

        [HttpPost]
        [Authorize(Roles = Driver)]
        public async Task<IActionResult> Create(ReturnProtocolInputModel returnProtocolInputModel)
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
            return RedirectToAction(nameof(Details), new { id = protocolId });
        }

        [HttpGet]
        [Authorize(Roles = Driver)]
        public async Task<IActionResult> Edit(int id) 
        {
            if (IsUserAuthenticated() == false)
            {
                return RedirectToAction("Index", "Home");
            }
            var userViewModel = await GetUserViewModelAsync();
            var returnProtocolToEdit = await returnProtocolService.GetEditableByIdAsync(userViewModel, id);

            if (returnProtocolToEdit == null)
            {
                return NotFound();
            }

            if (IsApproved(returnProtocolToEdit))
            {
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

        [HttpPost]
        [Authorize(Roles = Driver)]
        public async Task<IActionResult> Edit(ReturnProtocolEditModel returnProtocolInputModel)
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

            return RedirectToAction(nameof(Details), new { id = returnProtocolInputModel.Id });
        }

        [HttpGet]
        [Authorize(Roles = Driver)]
        public async Task<IActionResult> Delete(int id)
        {
            var userViewModel = await GetUserViewModelAsync();
            var returnProtocolToDelete = await returnProtocolService.GetEditableByIdAsync(userViewModel, id);
            if (returnProtocolToDelete == null)
            {
                return NotFound();
            }

            if (IsApproved(returnProtocolToDelete))
            {
                return RedirectToAction(nameof(Details), new { id = returnProtocolToDelete.Id });
            }

            var returnProtocol = new ReturnProtocolEditModel();
            returnProtocol.Id = returnProtocolToDelete.Id;
            returnProtocol.ReturnDate = returnProtocolToDelete.ReturnedDate;
            returnProtocol.CompanyObjectName = returnProtocolToDelete.CompanyObject.Name;

            return View(returnProtocol);
        }

        [HttpPost]
        [Authorize(Roles = Driver)]
        public async Task<IActionResult> Delete(ReturnProtocolEditModel returnProtocol)
        {
            var userViewModel = await GetUserViewModelAsync();
            var returnProtocolToDelete = await returnProtocolService.GetEditableByIdAsync(userViewModel, returnProtocol.Id);
            if (returnProtocolToDelete == null)
            {
                return NotFound();
            }
            await returnProtocolService.DeleteProtocolAsync(returnProtocolToDelete.Id);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [Authorize(Roles = WarehouseManager)]
        public async Task<IActionResult> Approve([ModelBinder(typeof(ApproveProductsModelBinder))]ReturnProtocolApproveModel detailReturnProtocol)
        {
            var userViewModel = await GetUserViewModelAsync();
            detailReturnProtocol.Approver = userViewModel;

            if (ModelState.IsValid == false || detailReturnProtocol.Approver == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var returnProtocol = await returnProtocolService.GetByIdAsync(userViewModel, detailReturnProtocol.Id);
            if (returnProtocol == null)
            {
                return NotFound();
            }
            if (IsDateTimeIdentical(detailReturnProtocol.LastChange, returnProtocol.LastChange) == false)
            {
                ModelState.AddModelError(nameof(detailReturnProtocol.LastChange), "The return protocol has been modified by another user. Please refresh the page and try again.");
                return RedirectToAction(nameof(Details), new { detailReturnProtocol.Id });
            }

            await returnProtocolService.ApproveAsync(detailReturnProtocol, userViewModel);

            return RedirectToAction(nameof(Index));
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
