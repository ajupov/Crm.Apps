namespace Crm.Common.UserContext.Attributes
{
    public class RequirePrivilegedAttribute : RequireAnyAttribute
    {
        public RequirePrivilegedAttribute()
            : base(Permission.System, Permission.Development, Permission.Administration, Permission.TechnicalSupport)
        {
        }
    }
}