using DelitaTrade.Common.Utilities;

namespace DelitaTrade.Common.Extensions
{
    public static class ApplicationRoleExtensions
    {
        public static IEnumerable<string?> GetAllApplicationRoles()
        {
            var type = typeof(Constants.DelitaIdentityConstants.RoleNames);
            return type.GetAllPublicConstantValues<string>();
        }
    }
}
