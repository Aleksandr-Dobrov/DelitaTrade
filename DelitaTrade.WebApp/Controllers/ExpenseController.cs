using DelitaTrade.Core.Contracts;
using DelitaTrade.Core.ViewModels.ExpenseModels;
using DelitaTrade.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static DelitaTrade.Common.Constants.DelitaIdentityConstants.RoleNames;
using static DelitaTrade.Common.Constants.AppMessageConstants;
using static DelitaTrade.Common.Constants.ApplicationMessages.ExpenseMessages;
using DelitaTrade.Common.Extensions;

namespace DelitaTrade.WebApp.Controllers
{
    [Authorize(Roles = DriverRole)]
    public class ExpenseController(IExpenseService expenseService,
            IDayReportService dayReportService,
            ICompanyObjectService companyObjectService,
            UserManager<DelitaUser> userManager) : BaseController(userManager)
    {
        public async Task<IActionResult> Index(int dayReportId)
        {
            try
            {
                var user = await GetUserViewModelAsync();

                var expenseModel = await expenseService.GetExpenseByDayReportIdAsync(user, dayReportId);
                return View(expenseModel);
            }
            catch (UnauthorizedAccessException ex)
            {
                TempData[Error] = ex.Message;
                return RedirectToAction(nameof(DayReportController.Details), nameof(DayReportController).GetControllerName(), new { dayReportId });
            }
            catch (Exception)
            {
                TempData[Error] = ExpenseNotFound;
                return RedirectToAction(nameof(DayReportController.Details), nameof(DayReportController).GetControllerName(), new { dayReportId });
            }
        }

        [HttpGet]
        [Authorize(Roles = DriverRole)]
        public async Task<IActionResult> AddExpense(int dayReportId)
        {
            try
            {
                int? vehicleId = await dayReportService.GetVehicleIdFromDayReportAsync(dayReportId);

                if (vehicleId.HasValue == false)
                {
                    return RedirectToAction(nameof(Index), new { dayReportId });
                }

                var expenseModel = new ExpenseInputModel()
                {
                    DayReportId = dayReportId,
                    VehicleId = vehicleId.Value,
                    Expenses = await companyObjectService.GetAllExpensesAsync(vehicleId.Value)
                };

                return View(expenseModel);
            }
            catch (Exception)
            {
                TempData[Error] = ExpenseNotFound;
                return RedirectToAction(nameof(DayReportController.Details), nameof(DayReportController).GetControllerName(), new { dayReportId });
            }
        }

        [HttpPost]
        [Authorize(Roles = DriverRole)]
        public async Task<IActionResult> AddExpense(ExpenseInputModel model)
        {
            try
            {
                if (ModelState.IsValid == false)
                {
                    model.Expenses = await companyObjectService.GetAllExpensesAsync(model.VehicleId);

                    return View(model);
                }
                else if (model.ExpenseId.HasValue == false && model.Expense == null)
                {
                    ModelState.AddModelError(nameof(model.Expense), "You must fill in at least one field");
                    ModelState.AddModelError(nameof(model.ExpenseId), "You must fill in at least one field");
                    model.Expenses = await companyObjectService.GetAllExpensesAsync(model.VehicleId);

                    return View(model);
                }
                var user = await GetUserViewModelAsync();

                await expenseService.AddExpenseAsync(user, model);
                TempData[Success] = CreateSuccess;
                return RedirectToAction(nameof(Index), new { model.DayReportId });
            }
            catch (UnauthorizedAccessException ex)
            {
                TempData[Error] = ex.Message;
                return RedirectToAction(nameof(DayReportController.Details), nameof(DayReportController).GetControllerName(), new { model.DayReportId });
            }
            catch (InvalidOperationException ex)
            {
                TempData[Error] = ex.Message;
                model.Expenses = await companyObjectService.GetAllExpensesAsync(model.VehicleId);
                return View(model);
            }
            catch (ArgumentNullException ex)
            {
                TempData[Error] = ex.Message;
                model.Expenses = await companyObjectService.GetAllExpensesAsync(model.VehicleId);
                return View(model);
            }
            catch (Exception)
            {
                TempData[Error] = CreateError;
                return RedirectToAction(nameof(DayReportController.Details), nameof(DayReportController).GetControllerName(), new { model.DayReportId });
            }
        }
        [HttpGet]
        [Authorize(Roles = DriverRole)]
        public async Task<IActionResult> UpdateExpense(int expenseId)
        {
            try
            {
                var user = await GetUserViewModelAsync();
                var expenseModel = await expenseService.GetExpenseByIdAsync(user, expenseId);
                if (expenseModel == null)
                {
                    return NotFound();
                }
                if (expenseModel.ExpenseId.HasValue == false)
                {
                    RedirectToAction(nameof(Index), new { expenseModel.DayReportId });
                }
                expenseModel.Expenses = await companyObjectService.GetAllExpensesAsync(expenseModel.VehicleId);
                
                return View(expenseModel);
            }
            catch (Exception)
            {
                TempData[Error] = ExpenseNotFound;
                return RedirectToAction(nameof(DayReportController.Details), nameof(DayReportController).GetControllerName(), new { expenseId });
            }
        }

        [HttpPost]
        [Authorize(Roles = DriverRole)]
        public async Task<IActionResult> UpdateExpense(ExpenseUpdateModel model)
        {
            try
            {
                if (ModelState.IsValid == false)
                {
                    model.Expenses = await companyObjectService.GetAllExpensesAsync(model.VehicleId);

                    return View(model);
                }
                else if (model.ExpenseId.HasValue == false && model.Expense == null)
                {
                    ModelState.AddModelError(nameof(model.Expense), "You must fill in at least one field");
                    ModelState.AddModelError(nameof(model.ExpenseId), "You must fill in at least one field");
                    model.Expenses = await companyObjectService.GetAllExpensesAsync(model.VehicleId);

                    return View(model);
                }
                var user = await GetUserViewModelAsync();

                await expenseService.UpdateExpenseAsync(user, model);
                TempData[Success] = UpdateSuccess;
                return RedirectToAction(nameof(Index), new { model.DayReportId });
            }
            catch (UnauthorizedAccessException ex)
            {
                TempData[Error] = ex.Message;
                return RedirectToAction(nameof(DayReportController.Details), nameof(DayReportController).GetControllerName(), new { model.DayReportId });
            }
            catch (InvalidOperationException ex)
            {
                TempData[Error] = ex.Message;
                model.Expenses = await companyObjectService.GetAllExpensesAsync(model.VehicleId);
                return View(model);
            }
            catch (ArgumentNullException ex)
            {
                TempData[Error] = ex.Message;
                model.Expenses = await companyObjectService.GetAllExpensesAsync(model.VehicleId);
                return View(model);
            }
            catch (Exception)
            {
                TempData[Error] = UpdateError;
                return RedirectToAction(nameof(DayReportController.Details), nameof(DayReportController).GetControllerName(), new { model.DayReportId });
            }
        }

        [HttpGet]
        [Authorize(Roles = DriverRole)]
        public async Task<IActionResult> DeleteExpense(int id, int dayReportId)
        {
            try
            {
                var user = await GetUserViewModelAsync();
                await expenseService.DeleteExpenseAsync(user, id, dayReportId);
                TempData[Success] = DeleteSuccess;
                return RedirectToAction(nameof(Index), new { dayReportId });
            }
            catch (UnauthorizedAccessException ex)
            {
                TempData[Error] = ex.Message;
                return RedirectToAction(nameof(DayReportController.Details), nameof(DayReportController).GetControllerName(), new { dayReportId });
            }
            catch (InvalidOperationException ex)
            {
                TempData[Error] = ex.Message;
                return RedirectToAction(nameof(DayReportController.Details), nameof(DayReportController).GetControllerName(), new { dayReportId });
            }
            catch (Exception)
            {
                TempData[Error] = DeleteError;
                return RedirectToAction(nameof(DayReportController.Details), nameof(DayReportController).GetControllerName(), new { dayReportId });
            }
        }

    }
}
