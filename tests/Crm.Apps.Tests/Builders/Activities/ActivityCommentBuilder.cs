using System;
using System.Threading.Tasks;
using Crm.Clients.Activities.Clients;
using Crm.Clients.Activities.Models;
using Crm.Utils.Guid;

namespace Crm.Apps.Tests.Builders.Activities
{
    public class ActivityCommentBuilder : IActivityCommentBuilder
    {
        private readonly IActivityCommentsClient _activityCommentsClient;
        private readonly ActivityComment _activityComment;

        public ActivityCommentBuilder(IActivityCommentsClient activityCommentsClient)
        {
            _activityCommentsClient = activityCommentsClient;
            _activityComment = new ActivityComment
            {
                ActivityId = Guid.Empty,
                CommentatorUserId = Guid.Empty,
                Value = "Test"
            };
        }

        public ActivityCommentBuilder WithActivityId(Guid activityId)
        {
            _activityComment.ActivityId = activityId;

            return this;
        }

        public Task BuildAsync()
        {
            if (_activityComment.ActivityId.IsEmpty())
            {
                throw new InvalidOperationException(nameof(_activityComment.ActivityId));
            }

            return _activityCommentsClient.CreateAsync(_activityComment);
        }
    }
}