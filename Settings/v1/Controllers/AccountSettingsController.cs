using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Infrastructure.All.Api.Attributes;
using Ajupov.Infrastructure.All.Jwt;
using Ajupov.Utils.All.Enums;
using Crm.Apps.Settings.Models;
using Crm.Apps.Settings.Services;
using Crm.Common.All.BaseControllers;
using Crm.Common.All.Roles.Attributes;
using Crm.Common.All.UserContext;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Settings.V1.Controllers
{
    [ApiController]
    [RequestContentTypeApplicationJson]
    [ResponseContentTypeApplicationJson]
    [RequireAnyRole(JwtDefaults.AuthenticationScheme)]
    [Route("Settings/Account/v1")]
    public class AccountSettingsController : AllowingCheckControllerBase
    {
        private readonly IUserContext _userContext;
        private readonly IAccountSettingsService _accountSettingsService;

        public AccountSettingsController(IUserContext userContext, IAccountSettingsService accountSettingsService)
            : base(userContext)
        {
            _userContext = userContext;
            _accountSettingsService = accountSettingsService;
        }

        [HttpGet("Get")]
        public Task<AccountSetting> Get(CancellationToken ct = default)
        {
            return _accountSettingsService.GetAsync(_userContext.AccountId, ct);
        }

        [HttpGet("GetActivityIndustries")]
        public Dictionary<string, AccountSettingActivityIndustry> GetActivityIndustries()
        {
            return EnumsExtensions.GetAsDictionary<AccountSettingActivityIndustry>();
        }

        [HttpPatch("SetActivityIndustry")]
        public Task SetActivityIndustry(AccountSettingActivityIndustry industry, CancellationToken ct = default)
        {
            return _accountSettingsService.SetActivityIndustryAsync(
                _userContext.UserId,
                _userContext.AccountId,
                industry,
                ct);
        }
    }
}
