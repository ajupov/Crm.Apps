using System.Collections.Generic;
using Crm.Apps.Areas.Users.Models;
using Crm.Apps.Utils;
using Crm.Common.UserContext.Attributes;
using Crm.Infrastructure.ApiDocumentation.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Areas.Users.Controllers
{
    [ApiController]
    [RequirePrivileged]
    [IgnoreApiDocumentation]
    [Route("Api/Users/Settings")]
    public class UserSettingsController : ControllerBase
    {
        [HttpGet("GetTypes")]
        public ActionResult<Dictionary<string, UserSettingType>> GetTypes()
        {
            return EnumsExtensions.GetAsDictionary<UserSettingType>();
        }
    }
}