using System.Collections.Generic;
using System.Linq;
using Crm.Apps.Areas.Accounts.Models;
using Crm.Common.UserContext;
using Crm.Common.UserContext.Attributes;
using Crm.Utils.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Areas.Accounts.Controllers
{
    [ApiController]
    [Route("Api/Accounts/Settings")]
    public class AccountSettingsController : ControllerBase
    {
        [HttpGet("GetTypes")]
        [RequireAny(Permission.System, Permission.Development, Permission.Administration, Permission.TechnicalSupport,
            Permission.AccountOwning)]
        public ActionResult<List<AccountSettingType>> GetTypes()
        {
            return EnumsExtensions.GetValues<AccountSettingType>().ToList();
        }
    }
}