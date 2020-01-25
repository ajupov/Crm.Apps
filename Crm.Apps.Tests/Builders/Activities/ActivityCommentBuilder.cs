using System;
using System.Threading.Tasks;
using Ajupov.Utils.All.Guid;
using Crm.Apps.Tests.Extensions;
using Crm.Apps.Tests.Services.AccessTokenGetter;
using Crm.Apps.v1.Clients.Activities.Clients;
using Crm.Apps.v1.Clients.Activities.Models;

namespace Crm.Apps.Tests.Builders.Activities
{
    public class ActivityCommentBuilder : IActivityCommentBuilder
    {
        private readonly IAccessTokenGetter _accessTokenGetter;
        private readonly IActivityCommentsClient _activityCommentsClient;
        private readonly ActivityComment _comment;

        public ActivityCommentBuilder(
            IAccessTokenGetter accessTokenGetter,
            IActivityCommentsClient activityCommentsClient)
        {
            _activityCommentsClient = activityCommentsClient;
            _accessTokenGetter = accessTokenGetter;
            _comment = new ActivityComment
            {
                ActivityId = Guid.Empty,
                Value = "Test".WithGuid()
            };
        }

        public ActivityCommentBuilder WithActivityId(Guid activityId)
        {
            _comment.ActivityId = activityId;

            return this;
        }

        public async Task BuildAsync()
        {
            var accessToken = await _accessTokenGetter.GetAsync();

            if (_comment.ActivityId.IsEmpty())
            {
                throw new InvalidOperationException(nameof(_comment.ActivityId));
            }

            await _activityCommentsClient.CreateAsync(accessToken, _comment);
        }
    }
}