using System.Collections.Generic;
using Crm.Apps.Areas.Accounts.Models;
using Crm.Utils.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Areas.Accounts.Controllers
{
    [ApiController]
    [Route("Api/Accounts/Settings")]
    public class AccountSettingsController : ControllerBase
    {
        [HttpGet("GetTypes")]
        public ActionResult<ICollection<AccountSettingType>> GetTypes()
        {
            return EnumsExtension.GetValues<AccountSettingType>();
        }
    }
}