using DelitaTrade.Core.Contracts;
using DelitaTrade.Core.ViewModels.DayReportModels;
using DelitaTrade.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static DelitaTrade.Common.Constants.DelitaIdentityConstants.RoleNames;

namespace DelitaTrade.WebApp.Controllers
{    
    public class DayReportController(IDayReportService dayReportService, UserManager<DelitaUser> userManager) : BaseController(userManager)
    {
        [HttpGet]
        public async Task<IActionResult> Index(SearchDayReportInputModel? model)
        {
            model ??= new SearchDayReportInputModel();
            if (User.IsInRole(Admin))
            {
                model.Employees = await dayReportService.GetAllUsersWhitReports(await GetUserViewModelAsync());
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> SearchDayReport(SearchDayReportInputModel model)
        {
            var userViewModel = await GetUserViewModelAsync();
            if (User.IsInRole(Admin))
            {
                model.Employees = await dayReportService.GetAllUsersWhitReports(await GetUserViewModelAsync());
            }
            model.DayReports = await dayReportService.GetSimpleFilteredAsync(userViewModel, model.ReporterId, model.StartDate, model.EndDate);
            return View(nameof(Index), model);
        }

        [HttpGet]        
        public async Task<IActionResult> Details(int id)
        {
            var userViewModel = await GetUserViewModelAsync();
            var dayReport = await dayReportService.GetByIdAsync(userViewModel, id);
            return View(dayReport);            
        }
    }
}
