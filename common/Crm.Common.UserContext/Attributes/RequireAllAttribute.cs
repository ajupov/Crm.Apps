using System;

namespace Crm.Common.UserContext.Attributes
{
    public class RequireAllAttribute : Attribute
    {
        public RequireAllAttribute(params Role[] roles)
        {
        }
    }
}