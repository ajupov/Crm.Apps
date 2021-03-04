using System.Threading;
using System.Threading.Tasks;
using Ajupov.Infrastructure.All.Jwt;
using Ajupov.Infrastructure.All.Mvc.Attributes;
using Crm.Apps.Flags.Models;
using Crm.Apps.Flags.Services;
using Crm.Common.All.BaseControllers;
using Crm.Common.All.Roles.Attributes;
using Crm.Common.All.UserContext;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Flags.V1.Controllers
{
    [ApiController]
    [RequestContentTypeApplicationJson]
    [ResponseContentTypeApplicationJson]
    [RequireAnyRole(JwtDefaults.AuthenticationScheme)]
    [Route("Flags/Account/v1")]
    public class AccountFlagsController : AllowingCheckControllerBase
    {
        private readonly IUserContext _userContext;
        private readonly IAccountFlagsService _accountFlagsService;

        public AccountFlagsController(IUserContext userContext, IAccountFlagsService accountFlagsService)
            : base(userContext)
        {
            _userContext = userContext;
            _accountFlagsService = accountFlagsService;
        }

        [HttpGet("IsSet")]
        public Task<bool> IsSet(AccountFlagType type, CancellationToken ct = default)
        {
            return _accountFlagsService.IsSetAsync(_userContext.AccountId, type, ct);
        }

        [HttpPut("Set")]
        public Task Set(AccountFlagType type, CancellationToken ct = default)
        {
            return _accountFlagsService.SetAsync(_userContext.AccountId, type, ct);
        }
    }
}
