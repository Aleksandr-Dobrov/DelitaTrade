using DelitaTrade.Infrastructure.Data.Models;
using DelitaTrade.WebApp.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static DelitaTrade.Common.Constants.DelitaIdentityConstants.RoleNames;

namespace DelitaTrade.WebApp.Areas.Admin.Controllers
{
    [Area(AdminRole)]
    [Authorize(Roles = AdminRole)]
    public abstract class BaseAdminController(UserManager<DelitaUser> userManager) : BaseController(userManager)
    {
    }
}
