using System.Linq;
using System.Threading.Tasks;
using Ajupov.Utils.All.DateTime;
using Ajupov.Utils.All.Guid;
using Crm.Apps.Tests.Services.Creator;
using Crm.Apps.v1.Clients.Activities.Clients;
using Crm.Apps.v1.Clients.Activities.Models;
using Crm.Apps.v1.Clients.Activities.RequestParameters;
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
            var type = await _create.ActivityType.BuildAsync();
            var status = await _create.ActivityStatus.BuildAsync();
            var activity = await _create.Activity
                .WithTypeId(type.Id)
                .WithStatusId(status.Id)
                .BuildAsync();

            await Task.WhenAll(
                _create.ActivityComment
                    .WithActivityId(activity.Id)
                    .BuildAsync(),
                _create.ActivityComment
                    .WithActivityId(activity.Id)
                    .BuildAsync());

            var request = new ActivityCommentGetPagedListRequestParameter
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
            var type = await _create.ActivityType.BuildAsync();
            var status = await _create.ActivityStatus.BuildAsync();
            var activity = await _create.Activity
                .WithTypeId(type.Id)
                .WithStatusId(status.Id)
                .BuildAsync();

            var comment = new ActivityComment
            {
                ActivityId = activity.Id,
                Value = "Test"
            };

            await _activityCommentsClient.CreateAsync(comment);

            var request = new ActivityCommentGetPagedListRequestParameter
            {
                ActivityId = activity.Id,
                SortBy = "CreateDateTime",
                OrderBy = "asc"
            };

            var createdComment = (await _activityCommentsClient.GetPagedListAsync(request)).First();

            Assert.NotNull(createdComment);
            Assert.Equal(comment.ActivityId, createdComment.ActivityId);
            Assert.True(!createdComment.CommentatorUserId.IsEmpty());
            Assert.Equal(comment.Value, createdComment.Value);
            Assert.True(createdComment.CreateDateTime.IsMoreThanMinValue());
        }
    }
}