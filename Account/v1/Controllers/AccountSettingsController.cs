using System.Threading;
using System.Threading.Tasks;
using Ajupov.Infrastructure.All.Api.Attributes;
using Ajupov.Infrastructure.All.Jwt;
using Crm.Apps.Account.Models;
using Crm.Apps.Account.Services;
using Crm.Common.All.BaseControllers;
using Crm.Common.All.Roles.Attributes;
using Crm.Common.All.UserContext;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Account.V1.Controllers
{
    [ApiController]
    [RequestContentTypeApplicationJson]
    [ResponseContentTypeApplicationJson]
    [RequireAccountRole(JwtDefaults.AuthenticationScheme)]
    [Route("Account/Settings/v1")]
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

        [HttpPut("SetActivityIndustry")]
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
