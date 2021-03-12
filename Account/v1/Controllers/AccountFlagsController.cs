using System.Collections.Generic;
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
    [Route("Account/Flags/v1")]
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
