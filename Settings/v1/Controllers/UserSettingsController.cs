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
    [Route("Settings/User/v1")]
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
        public Task<UserSetting> GetList(CancellationToken ct = default)
        {
            return _userSettingsService.GetAsync(_userContext.UserId, ct);
        }
    }
}
