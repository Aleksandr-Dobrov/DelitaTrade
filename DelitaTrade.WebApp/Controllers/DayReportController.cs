using DelitaTrade.Core.Contracts;
using DelitaTrade.Core.Models.ImportModels;
using DelitaTrade.Core.ViewModels;
using DelitaTrade.Core.ViewModels.DayReportModels;
using DelitaTrade.Core.ViewModels.ExpenseModels;
using DelitaTrade.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using static DelitaTrade.Common.Constants.DelitaIdentityConstants.RoleNames;

namespace DelitaTrade.WebApp.Controllers
{
    [Authorize(Roles = $"{Driver},{Admin},{LogisticsManager},{Cashier},{Accountant}")]
    public class DayReportController(IDayReportService dayReportService,
            IVehicleService vehicleService,
            IDeliveryService deliveryService,
            UserManager<DelitaUser> userManager) : BaseController(userManager)
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
                return RedirectToAction(nameof(Index));//TODO: add error if user not found
            }

            var newDayReport = new DayReportViewModel()
            {
                Date = dayReportInput.ReportedDate.HasValue ? dayReportInput.ReportedDate.Value : DateTime.Now,
                User = user!,
                Vehicle = await vehicleService.GetByIdAsync(dayReportInput.VehicleId)                 
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
            var dayReport = await dayReportService.GetDetailDayReportByIdAsync(userViewModel, id);
            return View(dayReport);            
        }

        [HttpGet]
        [Authorize(Roles = Admin)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var user = await GetUserViewModelAsync();
                var dayReport = await dayReportService.GetByIdAsync(user, id);

                var dayReportToDelete = new DayReportDeleteModel 
                {
                    Id = dayReport.Id,
                    ReportedDate = dayReport.Date,
                    EmployeeName = dayReport.User.Name
                };

                return View(dayReportToDelete);
            }
            catch
            {
                return RedirectToAction(nameof(Details), new { Id = id });
            }
        }

        [HttpPost]
        [Authorize(Roles = Admin)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(DayReportDeleteModel model)
        {
            try
            {
                var user = await GetUserViewModelAsync();

                await dayReportService.DeleteAsync(user, model.Id);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [Authorize(Roles = $"{Admin},{LogisticsManager}")]
        public async Task<IActionResult> ImportPayments(IFormFile file, int dayReportId)
        {
            if (file != null && file.Length > 0)
            {
                var user = await GetUserViewModelAsync();

                using (var stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream);
                    stream.Position = 0;

                    var importModel = await JsonSerializer.DeserializeAsync<DayReportJsonImportModel>(stream);
                    if (importModel != null)
                    {
                        await deliveryService.ImportPaymentsToDayReportAsync(user, dayReportId, importModel);
                    }
                }
            
            }
            return RedirectToAction(nameof(Details), new { Id = dayReportId });
        }
    }
}
