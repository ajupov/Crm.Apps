using System.Threading;
using System.Threading.Tasks;
using Ajupov.Infrastructure.All.Api.Attributes;
using Ajupov.Infrastructure.All.Jwt;
using Crm.Apps.User.Services;
using Crm.Apps.User.V1.Requests;
using Crm.Apps.User.V1.Responses;
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
    [Route("User/Settings/Changes/v1")]
    public class UserSettingChangesController : AllowingCheckControllerBase
    {
        private readonly IUserContext _userContext;
        private readonly IUserSettingChangesService _userSettingChangesService;

        public UserSettingChangesController(
            IUserContext userContext,
            IUserSettingChangesService userSettingChangesService)
            : base(userContext)
        {
            _userContext = userContext;
            _userSettingChangesService = userSettingChangesService;
        }

        [HttpPost("GetPagedList")]
        public Task<UserSettingChangeGetPagedListResponse> GetPagedList(
            UserSettingChangeGetPagedListRequest request,
            CancellationToken ct = default)
        {
            return _userSettingChangesService.GetPagedListAsync(_userContext.UserId, request, ct);
        }
    }
}
