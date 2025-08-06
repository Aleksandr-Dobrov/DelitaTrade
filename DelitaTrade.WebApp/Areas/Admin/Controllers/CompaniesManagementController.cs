using DelitaTrade.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static DelitaTrade.Common.Constants.AppMessageConstants;

namespace DelitaTrade.WebApp.Areas.Admin.Controllers
{
    public class CompaniesManagementController(UserManager<DelitaUser> userManager) : BaseAdminController(userManager)
    {
        public IActionResult Index()
        {
            TempData[Info] = "Sorry, the company management is not yet implemented. For import companies and objects try to import payments to day report from json file. This will add automatically not existing companies an objects to data base.";
            return NotFound();
        }
    }
}
