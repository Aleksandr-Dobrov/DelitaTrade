using DelitaTrade.Common.Enums;
using DelitaTrade.Common.Extensions;
using DelitaTrade.Core.Contracts;
using DelitaTrade.Core.Extensions;
using DelitaTrade.Core.ModelBinders;
using DelitaTrade.Core.ViewModels.InvoiceModels;
using DelitaTrade.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static DelitaTrade.Common.Constants.DelitaIdentityConstants.RoleNames;
using static DelitaTrade.Common.Constants.AppMessageConstants;
using static DelitaTrade.Common.Constants.ApplicationMessages.InvoiceMessages;

namespace DelitaTrade.WebApp.Controllers
{
    [Authorize(Roles = DriverRole)]
    public class InvoiceController(IInvoiceInDayReportService invoiceService, IDeliveryService deliveryService, UserManager<DelitaUser> userManager) : BaseController(userManager)
    {
        public IActionResult Index()
        {
            TempData[Warning] = NotImplemented;
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Complete(int id , int deliveryId) 
        {
            try
            {                
                var completeModel = await invoiceService.GetPaymentCompleteInputModelAsync(id, deliveryId);

                if (completeModel == null)
                { 
                    return RedirectToAction(nameof(DayReportController.Index), nameof(DayReportController).GetControllerName());
                }
                
                return View(completeModel);
            }
            catch (Exception)
            {
                TempData[Error] = CompleteError;
                return RedirectToAction(nameof(DayReportController.Index), nameof(DayReportController).GetControllerName());
            }
        }

        [HttpPost]
        public async Task<IActionResult> Complete(PaymentCompleteInputModel model)
        {
            try
            {
                if (ModelState.IsValid == false)
                {
                    return View(model);
                }

                var user = await GetUserViewModelAsync();

                await invoiceService.CompleteAsync(user, model);

                if(await deliveryService.IsCompleteAsync(user, model.DeliveryId))
                {
                    TempData[Success] = CompleteAllSuccess;
                    int dayReportId = await deliveryService.GetDayReportIdAsync(model.DeliveryId);
                    return RedirectToAction(nameof(DayReportController.Details), nameof(DayReportController).GetControllerName(), new { Id = dayReportId });
                }
                TempData[Success] = string.Format(CompleteSuccess, model.PaymentType.Translate());
                return RedirectToAction(nameof(DeliveryController.Details), nameof(DeliveryController).GetControllerName(), new { Id = model.DeliveryId });
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
            catch (InvalidOperationException)
            {
                ModelState.AddModelError(nameof(model.PaymentTypeId), "Incorrect payment type");
                return View(model);
            }

            catch (ArgumentNullException ex)
            {
                TempData[Error] = ex.Message;
                return RedirectToAction(nameof(DayReportController.Index), nameof(DayReportController).GetControllerName());
            }
            catch (Exception)
            {
                TempData[Error] = CompleteError;
                return RedirectToAction(nameof(DayReportController.Index), nameof(DayReportController).GetControllerName());
            }
        }

        [HttpGet]
        public async Task<IActionResult> AdvancePay(int id) 
        {
            try
            {
                var invoiceInDayReport = await invoiceService.GetAdvanceByIdAsync(id);
                if (invoiceInDayReport == null)
                {
                    return RedirectToAction(nameof(DayReportController.Index) ,nameof(DayReportController).GetControllerName());
                }

                var InputModel = invoiceInDayReport.GetInputModel();

                return View(InputModel);
            }
            catch
            {
                TempData[Error] = AdvancePaymentError;
                return RedirectToAction(nameof(DayReportController.Index), nameof(DayReportController).GetControllerName());
            }
        }

        [HttpPost]
        public async Task<IActionResult> AdvancePay(InvoiceInDayReportAdvanceInputModel model)
        {
            try
            {
                if (model.Income > model.Balance)
                {
                    ModelState.AddModelError(nameof(model.Income), "Income can not be greater than balance.");
                }
                if (model.Reason == InvoiceAdvancePayMethods.Cancelation && model.Balance < model.Amount)
                {
                    ModelState.AddModelError(nameof(model.ReasonId), "Invoice with payments cannot be canceled");
                }

                if (ModelState.IsValid == false)
                {
                    var invoiceInDayReport = await invoiceService.GetAdvanceByIdAsync(model.Id);
                    if (invoiceInDayReport == null)
                    {
                        TempData[Error] = AdvancePaymentError;
                        return RedirectToAction(nameof(DayReportController.Index), nameof(DayReportController).GetControllerName());
                    }

                    var inputModel = invoiceInDayReport.GetInputModel();
                    if (inputModel == null)
                    {
                        TempData[Error] = AdvancePaymentError;
                        return RedirectToAction(nameof(DayReportController.Index), nameof(DayReportController).GetControllerName());
                    }
                    model.PaymentTypes = inputModel.PaymentTypes;
                    model.Reasons = inputModel.Reasons;
                    return View(model);
                }

                var user = await GetUserViewModelAsync();

                await invoiceService.AdvancePayAsync(user, model, model.DeliveryId);

                TempData[Success] = AdvancePaymentSuccess;
                return RedirectToAction(nameof(DeliveryController.Details), nameof(DeliveryController).GetControllerName(), new { Id = model.DeliveryId });
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
            catch (InvalidOperationException)
            {
                ModelState.AddModelError(nameof(model.Income), "Incorrect value");

                return View(model);
            }

            catch (ArgumentNullException ex)
            {
                TempData[Error] = ex.Message;
                return RedirectToAction(nameof(DayReportController.Index), nameof(DayReportController).GetControllerName());
            }
            catch (Exception)
            {
                TempData[Error] = AdvancePaymentError;
                return RedirectToAction(nameof(DayReportController.Index), nameof(DayReportController).GetControllerName());
            }
        }
    }
}
