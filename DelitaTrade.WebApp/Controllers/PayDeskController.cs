using DelitaTrade.Core.Contracts;
using DelitaTrade.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static DelitaTrade.Common.Constants.DelitaIdentityConstants.RoleNames;
using Microsoft.AspNetCore.Identity;
using DelitaTrade.Core.ViewModels;
using DelitaTrade.Core.ModelBinders;
using DelitaTrade.Core.ViewModels.DayReportModels;

namespace DelitaTrade.WebApp.Controllers
{
    [Authorize(Roles = Driver)]
    public class PayDeskController(IDayReportService dayReportService, UserManager<DelitaUser> userManager) : BaseController(userManager)
    {
        [HttpGet]
        [Authorize(Roles = Driver)]
        public async Task<IActionResult> Index(int dayReportId)
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

        [HttpPost]
        [Authorize(Roles = Driver)]
        public async Task<IActionResult> ApplyChanges(BanknoteInputModel banknotesViewModel)
        {
            var banknoteModel = new DayReportBanknotesViewModel
            {
                Id = banknotesViewModel.Id,
                Date = banknotesViewModel.Date,
                TotalIncome = banknotesViewModel.TotalIncome,
                
            };

            foreach (var banknote in banknotesViewModel.BanknoteOldValues)
            {
                banknoteModel.Banknotes[banknote.Key] = banknote.Value + banknotesViewModel.Banknotes[banknote.Key];

                if(banknoteModel.Banknotes[banknote.Key] < 0)
                {
                    //TODO: implement custom error page
                    return View(nameof(Index), banknotesViewModel);
                }
            }

            var user = await GetUserViewModelAsync();

            await dayReportService.UpdateBanknotesAsync(user, banknoteModel);

            return RedirectToAction(nameof(Index), new { DayReportId = banknotesViewModel.Id });
        }
    }
}
