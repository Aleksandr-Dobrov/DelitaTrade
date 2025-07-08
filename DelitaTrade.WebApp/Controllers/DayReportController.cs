using DelitaTrade.Core.Contracts;
using DelitaTrade.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DelitaTrade.WebApp.Controllers
{    
    public class DayReportController(IDayReportService dayReportService, UserManager<DelitaUser> userManager) : BaseController(userManager)
    {
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userViewModel = await GetUserViewModelAsync();
            var DayReports = await dayReportService.GetAllDatesAsync(userViewModel);
            return View(DayReports);
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
