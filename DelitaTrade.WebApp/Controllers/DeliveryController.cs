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
    [Authorize(Roles = $"{AdminRole},{LogisticsManagerRole},{DriverRole}")]
    public class DeliveryController(IDeliveryService deliveryService, UserManager<DelitaUser> userManager) : BaseController(userManager)
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles = $"{AdminRole},{LogisticsManagerRole}")]
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
        [Authorize(Roles = $"{AdminRole},{LogisticsManagerRole}")]
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
        [Authorize(Roles = $"{AdminRole},{LogisticsManagerRole}")]
        public async Task<IActionResult> AddInvoice(int deliveryId) 
        {
            var user = await GetUserViewModelAsync();

            var delivery = await deliveryService.GetByIdAsync(user, deliveryId);

            if (delivery == null)
            {
                return RedirectToAction(nameof(DayReportController.Index), nameof(DayReportController).GetControllerName());
            }

            var invoiceInput = new InvoiceInputModel()
            {
                DeliveryId = deliveryId,
                CompanyObject = delivery.CompanyObjectName,
                CompanyObjectId = delivery.CompanyObjectId,
            };

            return View(invoiceInput); 
        }

        [HttpPost]
        [Authorize(Roles = $"{AdminRole},{LogisticsManagerRole}")]
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
        [Authorize(Roles = $"{AdminRole},{LogisticsManagerRole},{DriverRole}")]
        public async Task<IActionResult> AddOldInvoice(int deliveryId)
        {
            var user = await GetUserViewModelAsync();

            var delivery = await deliveryService.GetByIdAsync(user, deliveryId);

            if (delivery == null)
            {
                return RedirectToAction(nameof(DayReportController.Index), nameof(DayReportController).GetControllerName());
            }

            var invoiceInput = new OldInvoiceInputModel()
            {
                DeliveryId = deliveryId,
                CompanyObject = delivery.CompanyObjectName,
                CompanyObjectId = delivery.CompanyObjectId,
            };

            return View(invoiceInput);
        }

        [HttpPost]
        [Authorize(Roles = $"{AdminRole},{LogisticsManagerRole},{DriverRole}")]
        public async Task<IActionResult> AddOldInvoice(OldInvoiceInputModel invoiceInput)
        {
            if (ModelState.IsValid == false)
            {
                return View(invoiceInput);
            }

            var user = await GetUserViewModelAsync();

            await deliveryService.AddOldInvoiceAsync(user, invoiceInput, invoiceInput.DeliveryId);

            return RedirectToAction(nameof(Details), new { Id = invoiceInput.DeliveryId });
        }

        [HttpGet]
        [Authorize(Roles = DriverRole)]
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
        [Authorize(Roles = $"{AdminRole},{LogisticsManagerRole}")]
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
        [Authorize(Roles = $"{AdminRole},{LogisticsManagerRole}")]
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
    }
}
