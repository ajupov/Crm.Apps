using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Users.Models;
using Crm.Apps.Users.Parameters;
using Crm.Apps.Users.Services;
using Crm.Common.UserContext;
using Crm.Common.UserContext.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Users.Controllers
{
    [ApiController]
    [Route("Api/Users/Attributes/Changes")]
    public class UserAttributeChangesController : ControllerBase
    {
        private readonly IUserAttributeChangesService _userAttributeChangesService;

        public UserAttributeChangesController(IUserAttributeChangesService userAttributeChangesService)
        {
            _userAttributeChangesService = userAttributeChangesService;
        }

        [HttpPost("GetPagedList")]
        [RequireAny(Permission.System, Permission.Development, Permission.Administration, Permission.TechnicalSupport)]
        public async Task<ActionResult<List<UserAttributeChange>>> GetPagedList(
            UserAttributeChangeGetPagedListParameter parameter, CancellationToken ct = default)
        {
            return await _userAttributeChangesService.GetPagedListAsync(parameter, ct).ConfigureAwait(false);
        }
    }
}