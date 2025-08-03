using DelitaTrade.Common.Enums;
using DelitaTrade.Common.Extensions;
using DelitaTrade.Core.Contracts;
using DelitaTrade.Core.ViewModels.DeliveryModels;
using DelitaTrade.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using static DelitaTrade.Common.Constants.ApplicationMessages.DeliveryMessages;
using static DelitaTrade.Common.Constants.AppMessageConstants;
using static DelitaTrade.Common.Constants.DelitaIdentityConstants.RoleNames;

namespace DelitaTrade.WebApp.Controllers
{
    [Authorize(Roles = $"{AdminRole},{LogisticsManagerRole},{DriverRole}")]
    public class DeliveryController(IDeliveryService deliveryService, UserManager<DelitaUser> userManager) : BaseController(userManager)
    {
        public IActionResult Index()
        {
            TempData[Warning] = NotImplemented;
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
            try
            {
                var user = await GetUserViewModelAsync();

                var newDelivery = await deliveryService.AddDeliveryAsync(deliveryInputModel, user);
                       
                TempData[Success] = CreateSuccess;
                return RedirectToAction(nameof(Details), new { newDelivery.Id });
            }
            catch (Exception)
            {
                TempData[Error] = CreateError;
                return RedirectToAction(nameof(DayReportController.Details), nameof(DayReportController).GetControllerName(), new { Id = deliveryInputModel.DayReportId });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var user = await GetUserViewModelAsync();
                var delivery = await deliveryService.GetByIdAsync(user, id);

                if (delivery == null) 
                {
                    TempData[Error] = DeliveryNotFound;
                    return RedirectToAction(nameof(DayReportController.Index), nameof(DayReportController).GetControllerName());
                }

                return View(delivery);
            }
            catch (Exception)
            {
                TempData[Error] = DeliveryNotFound;
                return RedirectToAction(nameof(DayReportController.Index), nameof(DayReportController).GetControllerName());
            }
        }

        [HttpGet]
        [Authorize(Roles = $"{AdminRole},{LogisticsManagerRole}")]
        public async Task<IActionResult> AddInvoice(int deliveryId) 
        {
            try
            {
                var user = await GetUserViewModelAsync();

                var delivery = await deliveryService.GetByIdAsync(user, deliveryId);

                if (delivery == null)
                {
                    TempData[Error] = DeliveryNotFound;
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
            catch (Exception)
            {
                TempData[Error] = DeliveryNotFound;
                return RedirectToAction(nameof(DayReportController.Index), nameof(DayReportController).GetControllerName());
            }
        }

        [HttpPost]
        [Authorize(Roles = $"{AdminRole},{LogisticsManagerRole}")]
        public async Task<IActionResult> AddInvoice(InvoiceInputModel invoiceInput)
        {
            try 
            { 
                if (ModelState.IsValid == false) 
                {
                    return View(invoiceInput);
                }

                var user = await GetUserViewModelAsync();

                await deliveryService.AddInvoiceAsync(user, invoiceInput, invoiceInput.DeliveryId);

                TempData[Success] = string.Format(AddInvoiceSuccess, invoiceInput.Number);
                return RedirectToAction(nameof(Details), new { Id = invoiceInput.DeliveryId });
            }
            catch (InvalidOperationException ex)
            {
                TempData[Warning] = ex.Message;
                return RedirectToAction(nameof(Details), new { Id = invoiceInput.DeliveryId });
            }
            catch (Exception)
            {
                TempData[Error] = AddInvoiceError;
                return RedirectToAction(nameof(Details), new { Id = invoiceInput.DeliveryId });
            }
        }

        [HttpGet]
        [Authorize(Roles = $"{AdminRole},{LogisticsManagerRole},{DriverRole}")]
        public async Task<IActionResult> AddOldInvoice(int deliveryId)
        {
            try
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
            catch (Exception)
            {
                TempData[Error] = DeliveryNotFound;
                return RedirectToAction(nameof(DayReportController.Index), nameof(DayReportController).GetControllerName());
            }
        }

        [HttpPost]
        [Authorize(Roles = $"{AdminRole},{LogisticsManagerRole},{DriverRole}")]
        public async Task<IActionResult> AddOldInvoice(OldInvoiceInputModel invoiceInput)
        {
            try 
            {     
                if (ModelState.IsValid == false)
                {
                    return View(invoiceInput);
                }

                var user = await GetUserViewModelAsync();

                await deliveryService.AddOldInvoiceAsync(user, invoiceInput, invoiceInput.DeliveryId);

                TempData[Success] = string.Format(AddInvoiceSuccess, invoiceInput.Number);

                return RedirectToAction(nameof(Details), new { Id = invoiceInput.DeliveryId });
            }
            catch (InvalidOperationException ex)
            {
                TempData[Warning] = ex.Message;
                return RedirectToAction(nameof(Details), new { Id = invoiceInput.DeliveryId });
            }
            catch (Exception)
            {
                TempData[Error] = AddInvoiceError;
                return RedirectToAction(nameof(Details), new { Id = invoiceInput.DeliveryId });
            }
        }

        [HttpGet]
        [Authorize(Roles = DriverRole)]
        public async Task<IActionResult> Complete(int id)
        {
            try
            {
                var user = await GetUserViewModelAsync();

                await deliveryService.CompleteAllAsync(user, id);

                int dayReportId = await deliveryService.GetDayReportIdAsync(id);
                TempData[Success] = CompleteSuccess;
                return RedirectToAction(nameof(DayReportController.Details), nameof(DayReportController).GetControllerName(), new { Id = dayReportId });
            }            
            catch (Exception)
            {
                TempData[Error] = CompleteError;
                return RedirectToAction(nameof(Details), new { Id = id });
            }
        }

        [HttpGet]
        public async Task<IActionResult> AddCreditNote(int deliveryId)
        {
            try 
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
            catch (Exception)
            {
                TempData[Error] = DeliveryNotFound;
                return RedirectToAction(nameof(DayReportController.Index), nameof(DayReportController).GetControllerName());
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddCreditNote(CreditNoteInputModel creditNoteInput)
        {
            try 
            { 
                if (ModelState.IsValid == false) 
                {
                    return View(creditNoteInput);
                }

                var user = await GetUserViewModelAsync();

                await deliveryService.AddCreditNoteAsync(user, creditNoteInput, creditNoteInput.DeliveryId);

                TempData[Success] = string.Format(AddInvoiceSuccess, creditNoteInput.Number);
                return RedirectToAction(nameof(Details), new { Id = creditNoteInput.DeliveryId });
            }
            catch (InvalidOperationException ex)
            {
                TempData[Warning] = ex.Message;
                return RedirectToAction(nameof(Details), new { Id = creditNoteInput.DeliveryId });
            }
            catch (Exception)
            {
                TempData[Error] = AddInvoiceError;
                return RedirectToAction(nameof(Details), new { Id = creditNoteInput.DeliveryId });
            }
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
                    TempData[Error] = DeliveryNotFound;
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
            catch(Exception)
            {
                TempData[Error] = DeleteError;
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

                TempData[Success] = DeleteSuccess;
                return RedirectToAction(nameof(DayReportController.Details), nameof(DayReportController).GetControllerName(), new { Id = model.DayReportId });
            }
            catch(Exception)
            {
                TempData[Error] = DeleteError;
                return RedirectToAction(nameof(Details), new { model.Id });
            }
        }                
    }
}
