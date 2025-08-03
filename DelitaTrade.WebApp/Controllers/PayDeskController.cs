using DelitaTrade.Core.Contracts;
using DelitaTrade.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using DelitaTrade.Core.ViewModels.DayReportModels;
using static DelitaTrade.Common.Constants.DelitaIdentityConstants.RoleNames;
using static DelitaTrade.Common.Constants.AppMessageConstants;
using static DelitaTrade.Common.Constants.ApplicationMessages.PayDeskMessages;
using DelitaTrade.Common.Extensions;

namespace DelitaTrade.WebApp.Controllers
{
    [Authorize(Roles = DriverRole)]
    public class PayDeskController(IDayReportService dayReportService, UserManager<DelitaUser> userManager) : BaseController(userManager)
    {
        [HttpGet]
        [Authorize(Roles = DriverRole)]
        public async Task<IActionResult> Index(int dayReportId)
        {
            try
            {
                var user = await GetUserViewModelAsync();
                var dayReport = await dayReportService.GetBanknotesReadonlyAsync(user, dayReportId);

                var banknoteInputModel = new BanknoteInputModel
                {
                    Id = dayReport.Id,
                    Date = dayReport.Date,
                    BanknoteOldValues = dayReport.Banknotes,
                    TotalIncome = dayReport.TotalIncome,
                };

                return View(banknoteInputModel);
            }
            catch (Exception) 
            {
                TempData[Error] = PayDeskNotFound;
                return RedirectToAction(nameof(DayReportController.Details), nameof(DayReportController).GetControllerName(), new { Id = dayReportId });
            }
        }

        [HttpPost]
        [Authorize(Roles = DriverRole)]
        public async Task<IActionResult> ApplyChanges(BanknoteInputModel banknotesViewModel)
        {           
            try
            {
                var banknoteModel = banknotesViewModel.GetCalculatedBanknotes();

                var user = await GetUserViewModelAsync();

                await dayReportService.UpdateBanknotesAsync(user, banknoteModel);

                if (banknotesViewModel.Balance != 0)
                {
                    TempData[Success] = string.Format(
                    ApplySuccess,
                    banknotesViewModel.Balance > 0 ? "added" : "remove",
                    Math.Abs(banknotesViewModel.Balance),
                    "лв.");
                }
                else
                {
                    TempData[Info] = NoChange;
                }

                return RedirectToAction(nameof(Index), new { DayReportId = banknotesViewModel.Id });
            }
            catch (InvalidOperationException ex)
            {
                TempData[Error] = ex.Message;
                return View(nameof(Index), banknotesViewModel);
            }
            catch (Exception)
            {
                TempData[Error] = ApplyError;
                return View(nameof(Index), banknotesViewModel);
            }
        }
    }
}
