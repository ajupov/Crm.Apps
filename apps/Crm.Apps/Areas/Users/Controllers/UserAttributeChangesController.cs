using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Users.Models;
using Crm.Apps.Areas.Users.RequestParameters;
using Crm.Apps.Areas.Users.Services;
using Crm.Apps.Infrastructure;
using Crm.Common.UserContext;
using Crm.Common.UserContext.Attributes;
using Crm.Common.UserContext.BaseControllers;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Areas.Users.Controllers
{
    [ApiController]
    [RequirePrivileged]
    [IgnoreApiDocumentation]
    [Route("Api/Users/Attributes/Changes")]
    public class UserAttributeChangesController : UserContextController
    {
        private readonly IUserAttributesService _userAttributesService;
        private readonly IUserAttributeChangesService _userAttributeChangesService;

        public UserAttributeChangesController(
            IUserContext userContext,
            IUserAttributeChangesService userAttributeChangesService,
            IUserAttributesService userAttributesService)
            : base(userContext)
        {
            _userAttributeChangesService = userAttributeChangesService;
            _userAttributesService = userAttributesService;
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<List<UserAttributeChange>>> GetPagedList(
            UserAttributeChangeGetPagedListRequestParameter request,
            CancellationToken ct = default)
        {
            var attribute = await _userAttributesService.GetAsync(request.AttributeId, ct);
            var changes = await _userAttributeChangesService.GetPagedListAsync(request, ct);

            return ReturnIfAllowed(changes, Role.AccountOwning, attribute.AccountId);
        }
    }
}