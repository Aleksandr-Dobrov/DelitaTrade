using static DelitaTrade.Common.Constants.DelitaIdentityConstants.RoleNames;

namespace DelitaTrade.WebApp.Middlewares
{
    public class AdminRedirectionMiddleware(RequestDelegate next)
    {
        private const string indexPath = "/";
        private const string AdminIndexPath = "/Admin";
        public async Task InvokeAsync(HttpContext context)
        {
            if (context.User.Identity?.IsAuthenticated ?? false)
            {
                if (context.Request.Path == indexPath &&
                    context.User.IsInRole(AdminRole))
                {                   
                    // Redirect to the Admin area home page
                    context.Response.Redirect(AdminIndexPath);
                    return;
                }
            }
            await next(context);
        }
    }
}
