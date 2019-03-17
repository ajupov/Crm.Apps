using System.Collections.Generic;
using Crm.Areas.Accounts.Models;
using Crm.Utils.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Areas.Accounts.Controllers
{
    [ApiController]
    [Route("api/accounts/settings")]
    public class AccountSettingsController : ControllerBase
    {
        [HttpGet("GetTypes")]
        public ActionResult<ICollection<AccountSettingType>> GetTypes()
        {
            return EnumsExtension.GetValues<AccountSettingType>();
        }
    }
}