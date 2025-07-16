using DelitaTrade.Core.ViewModels;

namespace DelitaTrade.Core.Services
{
    public class BaseService
    {
        protected static bool IsAtLeastInOneRole(UserViewModel user, params string[] roles)
        {
            bool result = false;

            foreach (string role in roles) 
            {
                if (user.Roles.Contains(role)) 
                {
                    result = true;
                    break;
                }
            }

            return result;
        }

        protected static bool IsInAllRoles(UserViewModel user, params string[] roles)
        {
            bool result = false;

            foreach (string role in roles)
            {
                if (user.Roles.Contains(role))
                {
                    result = true;
                }
                else
                {
                    result = false;
                    break;
                }
            }

            return result;
        }
    }
}
