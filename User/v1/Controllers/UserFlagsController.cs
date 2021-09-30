using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Infrastructure.All.Api.Attributes;
using Ajupov.Infrastructure.All.Jwt;
using Crm.Apps.User.Models;
using Crm.Apps.User.Services;
using Crm.Common.All.BaseControllers;
using Crm.Common.All.Roles.Attributes;
using Crm.Common.All.UserContext;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.User.V1.Controllers
{
    [ApiController]
    [RequestContentTypeApplicationJson]
    [ResponseContentTypeApplicationJson]
    [RequireUserRole(JwtDefaults.AuthenticationScheme)]
    [Route("User/Flags/v1")]
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

        [HttpGet("GetNotSetList")]
        public Task<List<UserFlagType>> GetNotSetList(CancellationToken ct = default)
        {
            return _userFlagsService.GetNotSetListAsync(_userContext.UserId, ct);
        }

        [HttpPut("Set")]
        public Task Set(UserFlagType type, CancellationToken ct = default)
        {
            return _userFlagsService.SetAsync(_userContext.UserId, type, ct);
        }
    }
}
