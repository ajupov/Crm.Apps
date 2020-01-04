using System.Collections.Generic;
using Ajupov.Infrastructure.All.ApiDocumentation.Attributes;
using Ajupov.Utils.All.Enums;
using Crm.Apps.Users.Models;
using Crm.Common.UserContext.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Users.Controllers
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