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
    [Route("Flags/User/v1")]
    public class UserFlagsController : AllowingCheckControllerBase
    {
        private readonly IUserContext _userContext;
        private readonly IUserFlagsService _userFlagsService;

        public UserFlagsController(IUserContext userContext, IUserFlagsService userFlagsService)
            : base(userContext)
        {
            _userContext = userContext;
            _userFlagsService = userFlagsService;
        }

        [HttpGet("IsSet")]
        public Task<bool> IsSet(UserFlagType type, CancellationToken ct = default)
        {
            return _userFlagsService.IsSetAsync(_userContext.UserId, type, ct);
        }

        [HttpPut("Set")]
        public Task Set(UserFlagType type, CancellationToken ct = default)
        {
            return _userFlagsService.SetAsync(_userContext.UserId, type, ct);
        }
    }
}
