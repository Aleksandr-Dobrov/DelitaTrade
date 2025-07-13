using DelitaTrade.Core.Contracts;
using DelitaTrade.Core.ViewModels.DayReportModels;
using DelitaTrade.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using static DelitaTrade.Common.Constants.DelitaIdentityConstants.RoleNames;
using DelitaTrade.Core.ViewModels;

namespace DelitaTrade.WebApp.Controllers
{
    [Authorize(Roles = $"{Driver},{Admin},{LogisticsManager},{Cashier},{Accountant}")]
    public class DayReportController(IDayReportService dayReportService, IVehicleService vehicleService, UserManager<DelitaUser> userManager) : BaseController(userManager)
    {
        [HttpGet]
        public async Task<IActionResult> Index(SearchDayReportInputModel? model)
        {
            model ??= new SearchDayReportInputModel();
            if (User.IsInRole(Admin) 
                || User.IsInRole(LogisticsManager)
                || User.IsInRole(Cashier)
                || User.IsInRole(Accountant))
            {
                model.Employees = await dayReportService.GetAllUsersWhitDayReports(await GetUserViewModelAsync());
            }
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = $"{Admin},{LogisticsManager}")]
        public async Task<IActionResult> Create()
        {
            var user = await GetUserViewModelAsync();
           
            var dayReportInput = new DayReportInputModel()
            {
                Users = await dayReportService.GetAllDrivers(user),
                Vehicles = await vehicleService.AllAsync()
            };

            return View(dayReportInput);
        }

        [HttpPost]
        [Authorize(Roles = $"{Admin},{LogisticsManager}")]
        public async Task<IActionResult> Create(DayReportInputModel dayReportInput)
        {
            if (ModelState.IsValid == false) 
            {
                return RedirectToAction(nameof(Create));
            }
            var user = await GetUserViewModelByUserNameAsync(dayReportInput.UserName);
            if (user == null) 
            {
                RedirectToAction(nameof(Index));//TODO: add error if user not found
            }

            var newDayReport = new DayReportViewModel()
            {
                Date = dayReportInput.ReportedDate.HasValue ? dayReportInput.ReportedDate.Value : DateTime.Now,
                User = user!,
                Vehicle = dayReportInput.VehicleId != 0 ? await vehicleService.GetByIdAsync(dayReportInput.VehicleId) : null,                   
            };

            var createdDayReport = await dayReportService.CreateAsync(newDayReport);

            return RedirectToAction(nameof(Details), new { createdDayReport.Id });
        }

        [HttpGet]
        public async Task<IActionResult> SearchDayReport(SearchDayReportInputModel model)
        {
            var userViewModel = await GetUserViewModelAsync();
            if (ModelState.IsValid == false)
            {
                model.Employees = await dayReportService.GetAllUsersWhitDayReports(userViewModel);
                return View(model);
            }
            if (User.IsInRole(Admin) 
                || User.IsInRole(LogisticsManager)
                || User.IsInRole(Cashier)
                || User.IsInRole(Accountant))
            {
                model.Employees = await dayReportService.GetAllUsersWhitDayReports(userViewModel);
            }
            model.DayReports = await dayReportService.GetSimpleFilteredAsync(userViewModel, model.ReporterUserName, model.StartDate, model.EndDate);
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
