using System.Collections.Generic;
using Crm.Apps.Accounts.Models;
using Crm.Common.UserContext.Attributes;
using Crm.Infrastructure.ApiDocumentation.Attributes;
using Crm.Utils.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Accounts.Controllers
{
    [ApiController]
    [IgnoreApiDocumentation]
    [Route("Api/Accounts/Settings")]
    public class AccountSettingsApiController : ControllerBase
    {
        [HttpGet("GetTypes")]
        [RequirePrivileged]
        public ActionResult<Dictionary<string, AccountSettingType>> GetTypes()
        {
            return EnumsExtensions.GetAsDictionary<AccountSettingType>();
        }
    }
}