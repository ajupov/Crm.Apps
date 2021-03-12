using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Infrastructure.All.Api.Attributes;
using Ajupov.Infrastructure.All.Jwt;
using Crm.Apps.Flags.Models;
using Crm.Apps.Flags.Services;
using Crm.Common.All.BaseControllers;
using Crm.Common.All.Roles.Attributes;
using Crm.Common.All.UserContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

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
        private readonly ILogger<AccountFlagsController> _logger;

        public AccountFlagsController(
            IUserContext userContext,
            IAccountFlagsService accountFlagsService,
            ILogger<AccountFlagsController> logger)
            : base(userContext)
        {
            _userContext = userContext;
            _accountFlagsService = accountFlagsService;
            _logger = logger;
        }

        [HttpGet("IsSet")]
        public Task<bool> IsSet(AccountFlagType type, CancellationToken ct = default)
        {
            return _accountFlagsService.IsSetAsync(_userContext.AccountId, type, ct);
        }

        [HttpGet("GetNotSetList")]
        public Task<IEnumerable<AccountFlagType>> GetNotSetList(CancellationToken ct = default)
        {
            return _accountFlagsService.GetNotSetListAsync(_userContext.AccountId, ct);
        }

        [HttpPut("Set")]
        public Task Set(AccountFlagType type, CancellationToken ct = default)
        {
            return _accountFlagsService.SetAsync(_userContext.AccountId, type, ct);
        }
    }
}
