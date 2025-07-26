using DelitaTrade.Common.Enums;
using DelitaTrade.Core.ViewModels.DeliveryModels;
using DelitaTrade.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static DelitaTrade.Common.Constants.DelitaIdentityConstants.RoleNames;
using DelitaTrade.Common.Extensions;
using DelitaTrade.Core.Contracts;

namespace DelitaTrade.WebApp.Controllers
{
    [Authorize(Roles = $"{Admin},{LogisticsManager},{Driver}")]
    public class DeliveryController(IDeliveryService deliveryService, UserManager<DelitaUser> userManager) : BaseController(userManager)
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles = $"{Admin},{LogisticsManager}")]
        public IActionResult Create(int dayReportId)
        {
            var deliveryInputModel = new DeliveryInputModel
            {
                DayReportId = dayReportId
            };
            deliveryInputModel.PayMethods = [PayMethod.Cash.Translate(), PayMethod.Bank.Translate()];

            return View(deliveryInputModel);
        }

        [HttpPost]
        [Authorize(Roles = $"{Admin},{LogisticsManager}")]
        public async Task<IActionResult> Create(DeliveryInputModel deliveryInputModel)
        {
            var user = await GetUserViewModelAsync();

            var newDelivery = await deliveryService.AddDeliveryAsync(deliveryInputModel, user);

            
            return RedirectToAction(nameof(Details), new { newDelivery.Id });
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var user = await GetUserViewModelAsync();
            var delivery = await deliveryService.GetByIdAsync(user, id);

            if (delivery == null) 
            {
                return RedirectToAction(nameof(DayReportController.Index), nameof(DayReportController).GetControllerName());
            }

            return View(delivery);
        }

        [HttpGet]
        [Authorize(Roles = $"{Admin},{LogisticsManager}")]
        public async Task<IActionResult> AddInvoice(int deliveryId) 
        {
            var user = await GetUserViewModelAsync();

            var delivery = await deliveryService.GetByIdAsync(user, deliveryId);

            if (delivery == null)
            {
                return RedirectToAction(nameof(DayReportController.Index), nameof(DayReportController).GetControllerName());
            }

            InvoiceInputModel invoiceInput = new()
            {
                DeliveryId = deliveryId,
                CompanyObject = delivery.CompanyObjectName,
                CompanyObjectId = delivery.CompanyObjectId,
            };

            return View(invoiceInput); 
        }

        [HttpPost]
        [Authorize(Roles = $"{Admin},{LogisticsManager}")]
        public async Task<IActionResult> AddInvoice(InvoiceInputModel invoiceInput)
        {
            if (ModelState.IsValid == false) 
            {
                return View(invoiceInput);
            }

            var user = await GetUserViewModelAsync();

            await deliveryService.AddInvoiceAsync(user, invoiceInput, invoiceInput.DeliveryId);

            return RedirectToAction(nameof(Details), new { Id = invoiceInput.DeliveryId });
        }

        [HttpGet]
        [Authorize(Roles = Driver)]
        public async Task<IActionResult> Complete(int id)
        {
            var user = await GetUserViewModelAsync();

            await deliveryService.CompleteAllAsync(user, id);

            int dayReportId = await deliveryService.GetDayReportIdAsync(id);

            return RedirectToAction(nameof(DayReportController.Details), nameof(DayReportController).GetControllerName(), new { Id = dayReportId });
        }

        [HttpGet]
        public async Task<IActionResult> AddCreditNote(int deliveryId)
        {
            var user = await GetUserViewModelAsync();

            var delivery = await deliveryService.GetByIdAsync(user, deliveryId);

            if (delivery == null)
            {
                return RedirectToAction(nameof(DayReportController.Index), nameof(DayReportController).GetControllerName());
            }

            CreditNoteInputModel invoiceInput = new()
            {
                DeliveryId = deliveryId,
                CompanyObject = delivery.CompanyObjectName,
                CompanyObjectId = delivery.CompanyObjectId,
                CreditNoteTypeId = delivery.IsBank ? (int)CreditNoteMethod.Bank : (int)CreditNoteMethod.NotDeducted
            };

            return View(invoiceInput);
        }

        [HttpPost]
        public async Task<IActionResult> AddCreditNote(CreditNoteInputModel creditNoteInput)
        {
            if (ModelState.IsValid == false) 
            {
                return View(creditNoteInput);
            }

            var user = await GetUserViewModelAsync();

            await deliveryService.AddCreditNoteAsync(user, creditNoteInput, creditNoteInput.DeliveryId);

            return RedirectToAction(nameof(Details), new { Id = creditNoteInput.DeliveryId });
        }

        [HttpGet]
        [Authorize(Roles = $"{Admin},{LogisticsManager}")]
        public async Task<IActionResult> Delete(int id) 
        {
            try
            {
                var user = await GetUserViewModelAsync();
                var deliveryToDelete = await deliveryService.GetByIdAsync(user, id);

                if (deliveryToDelete == null) 
                {
                    return RedirectToAction(nameof(DayReportController.Index), nameof(DayReportController).GetControllerName());
                }

                var deleteModel = new DeliveryDeleteModel()
                {
                    Id = id,
                    DayReportId = deliveryToDelete.dayReportId,
                    DeliveryAddress = deliveryToDelete.CompanyObjectName,
                    EmployeeName = deliveryToDelete.EmployeeName,
                    Payments = deliveryToDelete.Payments.Select(p => p.InvoiceNumber).Distinct()
                };

                return View(deleteModel);
            }
            catch
            {
                return RedirectToAction(nameof(Details), new { Id = id});
            }
        }

        [HttpPost]
        [Authorize(Roles = $"{Admin},{LogisticsManager}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(DeliveryDeleteModel model)
        {
            try
            {
                var user = await GetUserViewModelAsync();

                await deliveryService.DeleteAsync(user, model.Id);

                return RedirectToAction(nameof(DayReportController.Details), nameof(DayReportController).GetControllerName(), new { Id = model.DayReportId });

            }
            catch
            {
                return RedirectToAction(nameof(Details), new { model.Id });
            }
        }

        [HttpGet]
        [Authorize(Roles = Driver)]
        public async Task<IActionResult> AddExpense(int deliveryId)
        {
            int? vehicleId = await deliveryService.GetVehicleIdFromDeliveryAsync(deliveryId);

            if (vehicleId.HasValue == false) 
            {
                return RedirectToAction(nameof(Details), new { id = deliveryId });
            }

            var expenseModel = new ExpenseInputModel() 
            {
                DeliveryId = deliveryId,
                VehicleId = vehicleId.Value,
                Expenses = await deliveryService.GetAllExpensesAsync(vehicleId.Value)                
            };

            return View(expenseModel);
        }

        [HttpPost]
        [Authorize(Roles = Driver)]
        public async Task<IActionResult> AddExpense(ExpenseInputModel model)
        {
            if (ModelState.IsValid == false)
            {
                model.Expenses = await deliveryService.GetAllExpensesAsync(model.VehicleId);

                return View(model);
            }
            else if (model.ExpenseId == null && model.Expense == null)
            {
                ModelState.AddModelError(nameof(model.Expense), "You must fill in at least one field");
                ModelState.AddModelError(nameof(model.ExpenseId), "You must fill in at least one field");
                model.Expenses = await deliveryService.GetAllExpensesAsync(model.VehicleId);

                return View(model);
            }
            var user = await GetUserViewModelAsync();

            await deliveryService.AddExpenseAsync(user, model, model.DeliveryId);

            return RedirectToAction(nameof(Details), new { Id = model.DeliveryId });
        }
    }
}
