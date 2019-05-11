using System;

namespace Crm.Common.UserContext.Attributes
{
    public class RequireAnyAttribute : Attribute
    {
        public RequireAnyAttribute(params Permission[] permissions)
        {
        }
    }
}