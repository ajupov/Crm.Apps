using System;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Infrastructure
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class IgnoreApiDocumentationAttribute : ApiExplorerSettingsAttribute
    {
        public IgnoreApiDocumentationAttribute()
        {
            IgnoreApi = true;
        }
    }
}