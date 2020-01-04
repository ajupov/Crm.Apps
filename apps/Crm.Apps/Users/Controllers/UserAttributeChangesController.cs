using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Infrastructure.All.ApiDocumentation.Attributes;
using Crm.Apps.Users.Models;
using Crm.Apps.Users.RequestParameters;
using Crm.Apps.Users.Services;
using Crm.Common.UserContext;
using Crm.Common.UserContext.Attributes;
using Crm.Common.UserContext.BaseControllers;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Users.Controllers
{
    [ApiController]
    [RequirePrivileged]
    [IgnoreApiDocumentation]
    [Route("Api/Users/Attributes/Changes")]
    public class UserAttributeChangesController : AllowingCheckControllerBase
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