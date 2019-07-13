using System.Linq;
using System.Threading.Tasks;
using Crm.Apps.Tests.Creator;
using Crm.Clients.Activities.Clients;
using Crm.Clients.Activities.Models;
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
            var account = await _create.Account.BuildAsync().ConfigureAwait(false);
            var type = await _create.ActivityType.WithAccountId(account.Id).BuildAsync().ConfigureAwait(false);
            var status = await _create.ActivityStatus.WithAccountId(account.Id).BuildAsync().ConfigureAwait(false);
            var activity = await _create.Activity.WithAccountId(account.Id).WithTypeId(type.Id).WithStatusId(status.Id)
                .BuildAsync().ConfigureAwait(false);
            await Task.WhenAll(
                    _create.ActivityComment.WithActivityId(activity.Id).BuildAsync(),
                    _create.ActivityComment.WithActivityId(activity.Id).BuildAsync())
                .ConfigureAwait(false);

            var comments = await _activityCommentsClient
                .GetPagedListAsync(activity.Id, sortBy: "CreateDateTime", orderBy: "desc").ConfigureAwait(false);

            var results = comments.Skip(1)
                .Zip(comments, (previous, current) => current.CreateDateTime >= previous.CreateDateTime);

            Assert.NotEmpty(comments);
            Assert.All(results, Assert.True);
        }

        [Fact]
        public async Task WhenCreate_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync().ConfigureAwait(false);
            var type = await _create.ActivityType.WithAccountId(account.Id).BuildAsync().ConfigureAwait(false);
            var status = await _create.ActivityStatus.WithAccountId(account.Id).BuildAsync().ConfigureAwait(false);
            var activity = await _create.Activity.WithAccountId(account.Id).WithTypeId(type.Id).WithStatusId(status.Id)
                .BuildAsync().ConfigureAwait(false);

            var comment = new ActivityComment
            {
                ActivityId = activity.Id,
                Value = "Test"
            };

            await _activityCommentsClient.CreateAsync(comment).ConfigureAwait(false);

            var createdComment = (await _activityCommentsClient.GetPagedListAsync(activity.Id, sortBy: "CreateDateTime",
                orderBy: "asc").ConfigureAwait(false)).First();

            Assert.NotNull(createdComment);
            Assert.Equal(comment.ActivityId, createdComment.ActivityId);
            Assert.True(!createdComment.CommentatorUserId.IsEmpty());
            Assert.Equal(comment.Value, createdComment.Value);
            Assert.True(createdComment.CreateDateTime.IsMoreThanMinValue());
        }
    }
}