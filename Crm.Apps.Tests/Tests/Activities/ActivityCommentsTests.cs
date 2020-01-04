using System.Linq;
using System.Threading.Tasks;
using Crm.Apps.Tests.Creator;
using Crm.Clients.Activities.Clients;
using Crm.Clients.Activities.RequestParameters;
using Crm.Utils.DateTime;
using Crm.Utils.Guid;
using Xunit;

namespace Crm.Apps.Tests.Tests.Activities
{
    public class ActivityCommentsTests
    {
        private readonly ICreate _create;
        private readonly IActivityCommentsClient _activityCommentsClient;

        public ActivityCommentsTests(ICreate create, IActivityCommentsClient activityCommentsClient)
        {
            _create = create;
            _activityCommentsClient = activityCommentsClient;
        }

        [Fact]
        public async Task WhenGetPagedList_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync();
            var type = await _create.ActivityType.WithAccountId(account.Id).BuildAsync();
            var status = await _create.ActivityStatus.WithAccountId(account.Id).BuildAsync();
            var activity = await _create.Activity
                .WithAccountId(account.Id)
                .WithTypeId(type.Id)
                .WithStatusId(status.Id)
                .BuildAsync();

            await Task.WhenAll(
                _create.ActivityComment.WithActivityId(activity.Id).BuildAsync(),
                _create.ActivityComment.WithActivityId(activity.Id).BuildAsync());

            var request = new ActivityCommentGetPagedListRequest
            {
                ActivityId = activity.Id,
                SortBy = "CreateDateTime",
                OrderBy = "desc"
            };
            
            var comments = await _activityCommentsClient.GetPagedListAsync(request);

            var results = comments
                .Skip(1)
                .Zip(comments, (previous, current) => current.CreateDateTime >= previous.CreateDateTime);

            Assert.NotEmpty(comments);
            Assert.All(results, Assert.True);
        }

        [Fact]
        public async Task WhenCreate_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync();
            var type = await _create.ActivityType.WithAccountId(account.Id).BuildAsync();
            var status = await _create.ActivityStatus.WithAccountId(account.Id).BuildAsync();
            var activity = await _create.Activity.WithAccountId(account.Id).WithTypeId(type.Id).WithStatusId(status.Id)
                .BuildAsync();

            var createRequest = new ActivityCommentCreateRequest
            {
                ActivityId = activity.Id,
                Value = "Test"
            };

            await _activityCommentsClient.CreateAsync(createRequest);

            var getPagedListRequest = new ActivityCommentGetPagedListRequest
            {
                ActivityId = activity.Id,
                SortBy = "CreateDateTime",
                OrderBy = "asc"
            };

            var createdComment = (await _activityCommentsClient.GetPagedListAsync(getPagedListRequest)).First();

            Assert.NotNull(createdComment);
            Assert.Equal(createRequest.ActivityId, createdComment.ActivityId);
            Assert.True(!createdComment.CommentatorUserId.IsEmpty());
            Assert.Equal(createRequest.Value, createdComment.Value);
            Assert.True(createdComment.CreateDateTime.IsMoreThanMinValue());
        }
    }
}