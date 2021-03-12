using System.Threading;
using System.Threading.Tasks;
using Ajupov.Infrastructure.All.Api.Attributes;
using Ajupov.Infrastructure.All.Jwt;
using Crm.Apps.Account.Services;
using Crm.Apps.Account.V1.Requests;
using Crm.Apps.Account.V1.Responses;
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
    [Route("Account/Settings/Changes/v1")]
    public class AccountSettingChangesController : AllowingCheckControllerBase
    {
        private readonly IUserContext _userContext;
        private readonly IAccountSettingChangesService _accountSettingChangesService;

        public AccountSettingChangesController(
            IUserContext userContext,
            IAccountSettingChangesService accountSettingChangesService)
            : base(userContext)
        {
            _userContext = userContext;
            _accountSettingChangesService = accountSettingChangesService;
        }

        [HttpPost("GetPagedList")]
        public Task<AccountSettingChangeGetPagedListResponse> GetPagedList(
            AccountSettingChangeGetPagedListRequest request,
            CancellationToken ct = default)
        {
            return _accountSettingChangesService.GetPagedListAsync(_userContext.AccountId, request, ct);
        }
    }
}
