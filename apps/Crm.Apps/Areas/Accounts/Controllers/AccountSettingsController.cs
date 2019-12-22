using System.Collections.Generic;
using Crm.Apps.Areas.Accounts.Models;
using Crm.Apps.Utils;
using Crm.Common.UserContext.Attributes;
using Crm.Infrastructure.ApiDocumentation.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Areas.Accounts.Controllers
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