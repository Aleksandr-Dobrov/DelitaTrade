using DelitaTrade.Common.Constants;
using DelitaTrade.Core.Contracts;
using DelitaTrade.Core.ViewModels;
using DelitaTrade.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static DelitaTrade.Common.Constants.DelitaIdentityConstants.RoleNames;

namespace DelitaTrade.WebApp.Controllers
{
    [Authorize(Roles = $"{Admin},{Driver},{WarehouseManager}")]
    public class ReturnProtocolController(
            ITraderService traderService, 
            IReturnProtocolService returnProtocolService,
            IReturnProductService returnProductService, 
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
                PayMethod = returnProtocolInputModel.PayMethod,
                Trader = await traderService.GetByIdAsync(returnProtocolInputModel.TraderId),
                CompanyObject = await companyObjectService.GetDetailedByIdAsync(returnProtocolInputModel.CompanyObjectId),
            };

            int protocolId = await returnProtocolService.CreateProtocolAsync(protocolViewModel);
            return RedirectToAction(nameof(Details), new { id = protocolId });
        }


    }
}
