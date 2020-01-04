using System.Collections.Generic;
using Ajupov.Infrastructure.All.ApiDocumentation.Attributes;
using Ajupov.Utils.All.Enums;
using Crm.Apps.Accounts.Models;
using Crm.Common.All.UserContext.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Accounts.Controllers
{
    [ApiController]
    [RequirePrivileged]
    [IgnoreApiDocumentation]
    [Route("Api/Accounts/Settings")]
    public class AccountSettingsApiController : ControllerBase
    {
        [HttpGet("GetTypes")]
        public ActionResult<Dictionary<string, AccountSettingType>> GetTypes()
        {
            return EnumsExtensions.GetAsDictionary<AccountSettingType>();
        }
    }
}