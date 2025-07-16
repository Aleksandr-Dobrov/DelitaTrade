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

namespace DelitaTrade.WebApp.Controllers
{
    [Authorize(Roles = Driver)]
    public class InvoiceController(IInvoiceInDayReportService invoiceService, IDeliveryService deliveryService, UserManager<DelitaUser> userManager) : BaseController(userManager)
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Complete(int id , int deliveryId) 
        {
            try
            {
                if(await invoiceService.IsBankPayAsync(id))
                {
                    var user = await GetUserViewModelAsync();
                    await invoiceService.CompleteAsync(user, new PaymentCompleteInputModel() 
                    {
                        Id = id,
                        DeliveryId = deliveryId,
                        PaymentTypeId = (int)PayMethod.Bank
                    });
                
                    if (await deliveryService.IsCompleteAsync(user, deliveryId))
                    {
                        int dayReportId = await deliveryService.GetDayReportIdAsync(deliveryId);
                        return RedirectToAction(nameof(DayReportController.Details), nameof(DayReportController).GetControllerName(), new { Id = dayReportId });
                    }
                
                    return RedirectToAction(nameof(DeliveryController.Details), nameof(DeliveryController).GetControllerName(), new { Id = deliveryId });
                }
                
                var completeModel = new PaymentCompleteInputModel() 
                {
                    Id = id,
                    DeliveryId = deliveryId
                };
                
                return View(completeModel);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
            catch (InvalidOperationException)
            {
                var completeModel = new PaymentCompleteInputModel()
                {
                    Id = id,
                    DeliveryId = deliveryId
                };
                ModelState.AddModelError(nameof(completeModel.PaymentTypeId), "Incorrect payment type");
                return View(completeModel);
            }

            catch (ArgumentNullException)
            {
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
                    int dayReportId = await deliveryService.GetDayReportIdAsync(model.DeliveryId);
                    return RedirectToAction(nameof(DayReportController.Details), nameof(DayReportController).GetControllerName(), new { Id = dayReportId });
                }

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

            catch (ArgumentNullException)
            {
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
                    return View(model);
                }

                var user = await GetUserViewModelAsync();

                await invoiceService.AdvancePayAsync(user, model, model.DeliveryId);

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

            catch (ArgumentNullException)
            {
                return RedirectToAction(nameof(DayReportController.Index), nameof(DayReportController).GetControllerName());
            }
        }
    }
}
