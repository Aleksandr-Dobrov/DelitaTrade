using DelitaTrade.Core.Contracts;
using DelitaTrade.Core.ModelBinders;
using DelitaTrade.Core.ViewModels;
using DelitaTrade.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DelitaTrade.WebApp.Controllers
{
    public class InvoiceController(IInvoiceInDayReportService invoiceService ,UserManager<DelitaUser> userManager) : BaseController(userManager)
    {
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Details([ModelBinder(typeof(InvoiceDetailModelBinder))]IEnumerable<int> invoiceIds) 
        {

            var invoices = await invoiceService.GetById(invoiceIds);
            

            return View(invoices); 
        }
    }
}
