using DelitaTrade.Core.Contracts;
using DelitaTrade.Core.ViewModels;
using DelitaTrade.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DelitaTrade.WebApp.Controllers
{
    [Authorize]
    public class DayReportController(IDayReportService dayReportService, UserManager<DelitaUser> userManager) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await userManager.GetUserAsync(User) ?? throw new ArgumentNullException("No user found");
            UserViewModel userViewModel = new UserViewModel
            {
                Id = user.Id, 
                UserName = user.UserName,
                Name = $"{user.Name} {user.LastName}"

            };
            var DayReports = await dayReportService.GetAllDatesAsync(userViewModel);
            return View(DayReports);
        }

        [HttpGet]        
        public async Task<IActionResult> Details(int id)
        {
            var user = await userManager.GetUserAsync(User) ?? throw new ArgumentNullException("No user found");
            UserViewModel userViewModel = new UserViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Name = $"{user.Name} {user.LastName}"
            };
            var dayReport = await dayReportService.GetByIdAsync(userViewModel, id);
            return View(dayReport);
        }
    }
}
