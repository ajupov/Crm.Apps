using System;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Infrastructure.ApiDocumentation.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class IgnoreApiDocumentationAttribute : ApiExplorerSettingsAttribute
    {
        public IgnoreApiDocumentationAttribute()
        {
            base.IgnoreApi = true;
        }
    }
}