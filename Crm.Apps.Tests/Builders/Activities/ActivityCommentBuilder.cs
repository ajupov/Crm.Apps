using System;
using System.Threading.Tasks;
using Crm.Clients.Activities.Clients;
using Crm.Clients.Activities.Models;
using Crm.Clients.Activities.RequestParameters;
using Crm.Utils.Guid;

namespace Crm.Apps.Tests.Builders.Activities
{
    public class ActivityCommentBuilder : IActivityCommentBuilder
    {
        private readonly IActivityCommentsClient _activityCommentsClient;
        private readonly ActivityCommentCreateRequest _request;

        public ActivityCommentBuilder(IActivityCommentsClient activityCommentsClient)
        {
            _activityCommentsClient = activityCommentsClient;
            _request = new ActivityCommentCreateRequest
            {
                ActivityId = Guid.Empty,
                Value = "Test"
            };
        }

        public ActivityCommentBuilder WithActivityId(Guid activityId)
        {
            _request.ActivityId = activityId;

            return this;
        }

        public Task BuildAsync()
        {
            if (_request.ActivityId.IsEmpty())
            {
                throw new InvalidOperationException(nameof(_request.ActivityId));
            }

            return _activityCommentsClient.CreateAsync(_request);
        }
    }
}