using DelitaTrade.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DelitaTrade.WebApp.Areas.Admin.Controllers
{
    public class HomeController(UserManager<DelitaUser> userManager) : BaseAdminController(userManager)
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
