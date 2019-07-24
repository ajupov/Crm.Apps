using System.Collections.Generic;
using Crm.Apps.Accounts.Models;
using Crm.Common.UserContext.Attributes;
using Crm.Infrastructure.ApiDocumentation.Attributes;
using Crm.Infrastructure.Mvc;
using Crm.Utils.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Accounts.Controllers
{
    [ApiController]
    [IgnoreApiDocumentation]
    [Route("Api/Accounts/Settings")]
    public class AccountSettingsApiController : DefaultApiController
    {
        [HttpGet("GetTypes")]
        [RequirePrivileged]
        public ActionResult<Dictionary<AccountSettingType, string>> GetTypes()
        {
            return Get(EnumsExtensions.GetAsDictionary<AccountSettingType>());
        }
    }
}