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
    [Route("User/Settings/v1")]
    public class UserSettingsController : AllowingCheckControllerBase
    {
        private readonly IUserContext _userContext;
        private readonly IUserSettingsService _userSettingsService;

        public UserSettingsController(IUserContext userContext, IUserSettingsService userSettingsService)
            : base(userContext)
        {
            _userContext = userContext;
            _userSettingsService = userSettingsService;
        }

        [HttpGet("Get")]
        public Task<UserSetting> Get(CancellationToken ct = default)
        {
            return _userSettingsService.GetAsync(_userContext.UserId, ct);
        }
    }
}
