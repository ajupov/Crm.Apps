using System.Linq;

namespace Crm.Common.UserContext.Attributes
{
    public class RequirePrivilegedAttribute : RequireAnyAttribute
    {
        public static readonly Permission[] PrivilegedPermissions =
        {
            Permission.System,
            Permission.Development,
            Permission.Administration,
            Permission.TechnicalSupport
        };

        public RequirePrivilegedAttribute(params Permission[] additionalPermissions)
            : base(PrivilegedPermissions.Concat(additionalPermissions).ToArray())
        {
        }
    }
}