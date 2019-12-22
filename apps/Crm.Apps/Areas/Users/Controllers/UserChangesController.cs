using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Users.Models;
using Crm.Apps.Areas.Users.Parameters;
using Crm.Apps.Areas.Users.Services;
using Crm.Common.UserContext;
using Crm.Common.UserContext.Attributes;
using Crm.Common.UserContext.Extensions;
using Crm.Infrastructure.ApiDocumentation.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Areas.Users.Controllers
{
    [ApiController]
    [RequirePrivileged]
    [IgnoreApiDocumentation]
    [Route("Api/Users/Changes")]
    public class UserChangesController : UserContextController
    {
        private readonly IUsersService _usersService;
        private readonly IUserChangesService _userChangesService;

        public UserChangesController(IUserContext userContext, IUsersService usersService,
            IUserChangesService userChangesService)
            : base(userContext)
        {
            _usersService = usersService;
            _userChangesService = userChangesService;
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<List<UserChange>>> GetPagedList(
            UserChangeGetPagedListParameter parameter,
            CancellationToken ct = default)
        {
            var user = await _usersService.GetAsync(parameter.UserId, ct);
            var changes = await _userChangesService.GetPagedListAsync(parameter, ct);

            return ReturnIfAllowed(changes, Role.AccountOwning, user.AccountId);
        }
    }
}