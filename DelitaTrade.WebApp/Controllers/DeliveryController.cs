using DelitaTrade.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DelitaTrade.WebApp.Controllers
{
    public class DeliveryController(UserManager<DelitaUser> userManager) : BaseController(userManager)
    {
        public IActionResult Index()
        {
            return View();
        }

        //[HttpGet]
        //public Task<IActionResult> Create()
        //{


        //    return View();
        //}
    }
}
