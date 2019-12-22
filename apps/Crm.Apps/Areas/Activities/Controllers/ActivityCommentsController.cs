using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Activities.Models;
using Crm.Apps.Areas.Activities.RequestParameters;
using Crm.Apps.Areas.Activities.Services;
using Crm.Common.UserContext;
using Crm.Common.UserContext.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Areas.Activities.Controllers
{
    [ApiController]
    [Route("Api/Activities/Comments")]
    public class ActivityCommentsController : ControllerBase
    {
        private readonly IUserContext _userContext;
        private readonly IActivitiesService _activitiesService;
        private readonly IActivityCommentsService _activityCommentsService;

        public ActivityCommentsController(IUserContext userContext, IActivitiesService activitiesService,
            IActivityCommentsService activityCommentsService)
        {
            _userContext = userContext;
            _activitiesService = activitiesService;
            _activityCommentsService = activityCommentsService;
        }

        [RequirePrivileged(Role.AccountOwning, Role.SalesManagement)]
        [HttpPost("GetPagedList")]
        public async Task<ActionResult<ActivityComment[]>> GetPagedList(
            ActivityCommentGetPagedListRequest request,
            CancellationToken ct = default)
        {
            var activity = await _activitiesService.GetAsync(request.ActivityId, ct);
            var comments = await _activityCommentsService.GetPagedListAsync(request, ct);

            return ReturnIfAllowed(comments, new[] {activity.AccountId});
        }

        [RequirePrivileged(Role.AccountOwning, Role.SalesManagement)]
        [HttpPost("Create")]
        public async Task<ActionResult> Create(ActivityCommentCreateRequest request, CancellationToken ct = default)
        {
            var activity = await _activitiesService.GetAsync(request.ActivityId, ct);

            return await ActionIfAllowed(
                () => _activityCommentsService.CreateAsync(_userContext.UserId, request, ct),
                new[] {activity.AccountId});
        }

        [NonAction]
        private ActionResult<TResult> ReturnIfAllowed<TResult>(TResult result, IEnumerable<Guid> accountIds)
        {
            if (_userContext.HasAny(RequirePrivilegedAttribute.PrivilegedRoles))
            {
                return result;
            }

            var accountIdsAsArray = accountIds.ToArray();

            if (_userContext.HasAny(Role.AccountOwning, Role.SalesManagement) &&
                _userContext.Belongs(accountIdsAsArray))
            {
                return result;
            }

            if (_userContext.HasAny(Role.AccountOwning, Role.SalesManagement) &&
                !_userContext.Belongs(accountIdsAsArray))
            {
                return Forbid();
            }

            throw new Exception();
        }

        [NonAction]
        private async Task<ActionResult> ActionIfAllowed(Func<Task> action, IEnumerable<Guid> accountIds)
        {
            if (_userContext.HasAny(RequirePrivilegedAttribute.PrivilegedRoles))
            {
                await action();

                return NoContent();
            }

            var accountIdsAsArray = accountIds.ToArray();

            if (_userContext.HasAny(Role.AccountOwning, Role.ProductsManagement) &&
                _userContext.Belongs(accountIdsAsArray))
            {
                await action();

                return NoContent();
            }

            if (_userContext.HasAny(Role.AccountOwning, Role.ProductsManagement) &&
                !_userContext.Belongs(accountIdsAsArray))
            {
                return Forbid();
            }

            throw new Exception();
        }
    }
}