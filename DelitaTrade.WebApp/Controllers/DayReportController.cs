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
using static DelitaTrade.Common.Constants.AppMessageConstants;
using static DelitaTrade.Common.Constants.AppErrorMessage.DayReportMessages;
using static DelitaTrade.Common.Constants.FormatConstant.DateTimeFormat;
using DelitaTrade.Common.Extensions;

namespace DelitaTrade.WebApp.Controllers
{
    [Authorize(Roles = $"{DriverRole},{AdminRole},{LogisticsManagerRole},{CashierRole},{AccountantRole}")]
    public class DayReportController(IDayReportService dayReportService,
            IVehicleService vehicleService,
            IDeliveryService deliveryService,
            IImportService importService,
            UserManager<DelitaUser> userManager) : BaseController(userManager)
    {
        [HttpGet]
        public async Task<IActionResult> Index(SearchDayReportInputModel? model)
        {
            try
            {
                model ??= new SearchDayReportInputModel();
                var previousSearch = TempData.Peek("dayReports") as IEnumerable<int>;
                if (previousSearch != null && previousSearch.Any())
                {
                    var user = await GetUserViewModelAsync();
                    model.DayReports = await dayReportService.GetSimpleByIdAsync(user, previousSearch);
                }

                if (User.IsInRole(AdminRole) 
                    || User.IsInRole(LogisticsManagerRole)
                    || User.IsInRole(CashierRole)
                    || User.IsInRole(AccountantRole))
                {
                    model.Employees = await dayReportService.GetAllUsersWhitDayReports(await GetUserViewModelAsync());
                }

                return View(model);
            }
            catch (Exception)
            {
                TempData[Error] = SearchPageError;
                return RedirectToAction(nameof(HomeController.Index), nameof(HomeController).GetControllerName());
            }
        }

        [HttpGet]
        [Authorize(Roles = $"{AdminRole},{LogisticsManagerRole}")]
        public async Task<IActionResult> Create()
        {
            try
            {
                var user = await GetUserViewModelAsync();
           
                var dayReportInput = new DayReportInputModel()
                {
                    Users = await dayReportService.GetAllDrivers(user),
                    Vehicles = await vehicleService.AllAsync()
                };

                return View(dayReportInput);
            }
            catch (Exception)
            {
                TempData[Error] = CreateError;
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [Authorize(Roles = $"{AdminRole},{LogisticsManagerRole}")]
        public async Task<IActionResult> Create(DayReportInputModel dayReportInput)
        {
            try
            {
                if (ModelState.IsValid == false) 
                {
                    return View();
                }
                var user = await GetUserViewModelByUserNameAsync(dayReportInput.UserName);
                if (user == null) 
                {
                    TempData[Error] = UserNotFound;
                    return RedirectToAction(nameof(Index));
                }

                var newDayReport = new DayReportViewModel()
                {
                    Date = dayReportInput.ReportedDate.HasValue ? dayReportInput.ReportedDate.Value : DateTime.Now,
                    User = user!,
                    Vehicle = await vehicleService.GetByIdAsync(dayReportInput.VehicleId)                 
                };

                var createdDayReport = await dayReportService.CreateAsync(newDayReport);
                TempData[Success] = string.Format(CreateSuccess, createdDayReport.Date.ToString(AppDateFormat), createdDayReport.User.Name);

                return RedirectToAction(nameof(Details), new { createdDayReport.Id });
            }
            catch (Exception)
            {
                TempData[Error] = CreateError;
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        public async Task<IActionResult> SearchDayReport(SearchDayReportInputModel model)
        {
            try
            {
                var userViewModel = await GetUserViewModelAsync();
                if (ModelState.IsValid == false)
                {
                    model.Employees = await dayReportService.GetAllUsersWhitDayReports(userViewModel);
                    return View(model);
                }
                if (User.IsInRole(AdminRole) 
                    || User.IsInRole(LogisticsManagerRole)
                    || User.IsInRole(CashierRole)
                    || User.IsInRole(AccountantRole))
                {
                    model.Employees = await dayReportService.GetAllUsersWhitDayReports(userViewModel);
                }
                model.DayReports = await dayReportService.GetSimpleFilteredAsync(userViewModel, model.ReporterUserName, model.StartDate, model.EndDate);
                if (model.DayReports != null)
                {
                    TempData["dayReports"] = model.DayReports.Select(d => d.Id).ToList();
                    TempData[Success] = string.Format(SearchComplete, model.DayReports.Count());
                }
                return View(nameof(Index), model);
            }
            catch (Exception)
            {
                TempData[Error] = SearchError;
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]        
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var userViewModel = await GetUserViewModelAsync();            
                var dayReport = await dayReportService.GetDetailDayReportByIdAsync(userViewModel, id);
                return View(dayReport);  
            }
            catch (ArgumentNullException ex)
            {
                TempData[Error] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        [Authorize(Roles = AdminRole)]
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
        [Authorize(Roles = AdminRole)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(DayReportDeleteModel model)
        {
            try
            {
                var user = await GetUserViewModelAsync();

                await dayReportService.DeleteAsync(user, model.Id);

                TempData[Success] = string.Format(DeleteSuccess, model.ReportedDate.ToString(AppDateFormat), model.EmployeeName);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                TempData[Error] = DeleteError;
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [Authorize(Roles = $"{AdminRole},{LogisticsManagerRole}")]
        public async Task<IActionResult> ImportPayments(IFormFile file, int dayReportId)
        {
            try
            {
                int importedPaymentsCount = 0;
                if (file != null && file.Length > 0)
                {
                    var user = await GetUserViewModelAsync();
                    
                    var importModel = await importService.ImportDeliveriesAsync(file);
                    if (importModel != null)
                    {
                        importedPaymentsCount = await deliveryService.ImportPaymentsToDayReportAsync(user, dayReportId, importModel);
                    }
                }
                if (importedPaymentsCount > 0)
                {
                    TempData[Success] = string.Format(ImportPaymentsSuccess, importedPaymentsCount);
                }
                else
                {
                    TempData[Info] = NoPaymentsToImport;
                }
                return RedirectToAction(nameof(Details), new { Id = dayReportId });
            }
            catch (Exception)
            {
                TempData[Error] = ImportPaymentsError;
                return RedirectToAction(nameof(Details), new { Id = dayReportId });
            }
        }
    }
}
