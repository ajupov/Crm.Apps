using System.Collections.Generic;
using Crm.Apps.Areas.Accounts.Models;
using Crm.Common.UserContext;
using Crm.Common.UserContext.Attributes;
using Crm.Utils.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Areas.Users.Controllers
{
    [ApiController]
    [Route("Api/Users/Settings")]
    public class UserSettingsController : ControllerBase
    {
        [HttpGet("GetTypes")]
        [RequireAny(Permission.System, Permission.Development, Permission.Administration, Permission.TechnicalSupport)]
        public ActionResult<ICollection<AccountSettingType>> GetTypes()
        {
            return EnumsExtensions.GetValues<AccountSettingType>();
        }
    }
}