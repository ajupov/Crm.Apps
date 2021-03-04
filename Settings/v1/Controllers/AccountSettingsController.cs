using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Infrastructure.All.Jwt;
using Ajupov.Infrastructure.All.Mvc.Attributes;
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

        [HttpGet("GetList")]
        public Task<List<AccountSetting>> GetList(CancellationToken ct = default)
        {
            return _accountSettingsService.GetListAsync(_userContext.AccountId, ct);
        }
    }
}
