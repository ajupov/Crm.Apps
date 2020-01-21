using System;
using System.Threading.Tasks;
using Ajupov.Utils.All.Guid;
using Crm.Apps.v1.Clients.Activities.Clients;
using Crm.Apps.v1.Clients.Activities.Models;

namespace Crm.Apps.Tests.Builders.Activities
{
    public class ActivityCommentBuilder : IActivityCommentBuilder
    {
        private readonly IActivityCommentsClient _activityCommentsClient;
        private readonly ActivityComment _comment;

        public ActivityCommentBuilder(IActivityCommentsClient activityCommentsClient)
        {
            _activityCommentsClient = activityCommentsClient;
            _comment = new ActivityComment
            {
                ActivityId = Guid.Empty,
                Value = "Test"
            };
        }

        public ActivityCommentBuilder WithActivityId(Guid activityId)
        {
            _comment.ActivityId = activityId;

            return this;
        }

        public Task BuildAsync()
        {
            if (_comment.ActivityId.IsEmpty())
            {
                throw new InvalidOperationException(nameof(_comment.ActivityId));
            }

            return _activityCommentsClient.CreateAsync(_comment);
        }
    }
}