using System.Linq;

namespace Crm.Common.UserContext.Attributes
{
    public class RequirePrivilegedAttribute : RequireAnyAttribute
    {
        public static readonly Role[] PrivilegedRoles =
        {
            Role.System,
            Role.Development,
            Role.Administration,
            Role.TechnicalSupport
        };

        public RequirePrivilegedAttribute(params Role[] additionalRoles)
            : base(PrivilegedRoles.Concat(additionalRoles).ToArray())
        {
        }
    }
}