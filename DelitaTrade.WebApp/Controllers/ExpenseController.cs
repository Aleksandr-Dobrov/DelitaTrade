using DelitaTrade.Core.Contracts;
using DelitaTrade.Core.Services;
using DelitaTrade.Core.ViewModels.ExpenseModels;
using DelitaTrade.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using System.Threading.Tasks;
using static DelitaTrade.Common.Constants.DelitaIdentityConstants.RoleNames;
using static DelitaTrade.Common.Constants.FormatConstant.DateTimeFormat;

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
            var user = await GetUserViewModelAsync();

            var expenseModel = await expenseService.GetExpenseByDayReportIdAsync(user, dayReportId);
            return View(expenseModel);
        }

        [HttpGet]
        [Authorize(Roles = DriverRole)]
        public async Task<IActionResult> AddExpense(int dayReportId)
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

        [HttpPost]
        [Authorize(Roles = DriverRole)]
        public async Task<IActionResult> AddExpense(ExpenseInputModel model)
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

            return RedirectToAction(nameof(Index), new { model.DayReportId });
        }
        [HttpGet]
        [Authorize(Roles = DriverRole)]
        public async Task<IActionResult> UpdateExpense(int expenseId)
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

        [HttpPost]
        [Authorize(Roles = DriverRole)]
        public async Task<IActionResult> UpdateExpense(ExpenseUpdateModel model)
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

            return RedirectToAction(nameof(Index), new { model.DayReportId });
        }

        [HttpGet]
        [Authorize(Roles = DriverRole)]
        public async Task<IActionResult> DeleteExpense(int id, int dayReportId)
        {
            var user = await GetUserViewModelAsync();
            await expenseService.DeleteExpenseAsync(user, id, dayReportId);

            return RedirectToAction(nameof(Index), new { dayReportId });
        }

    }
}
