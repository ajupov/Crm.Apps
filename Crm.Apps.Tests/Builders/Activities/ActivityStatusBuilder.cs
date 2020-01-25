using System;
using System.Threading.Tasks;
using Crm.Apps.Tests.Extensions;
using Crm.Apps.Tests.Services.AccessTokenGetter;
using Crm.Apps.v1.Clients.Activities.Clients;
using Crm.Apps.v1.Clients.Activities.Models;

namespace Crm.Apps.Tests.Builders.Activities
{
    public class ActivityStatusBuilder : IActivityStatusBuilder
    {
        private readonly IAccessTokenGetter _accessTokenGetter;

        private readonly IActivityStatusesClient _activityStatusesClient;
        private readonly ActivityStatus _status;

        public ActivityStatusBuilder(
            IAccessTokenGetter accessTokenGetter,
            IActivityStatusesClient activityStatusesClient)
        {
            _activityStatusesClient = activityStatusesClient;
            _accessTokenGetter = accessTokenGetter;
            _status = new ActivityStatus
            {
                AccountId = Guid.Empty,
                Name = "Test".WithGuid(),
                IsFinish = false,
                IsDeleted = false
            };
        }

        public ActivityStatusBuilder WithName(string name)
        {
            _status.Name = name;

            return this;
        }

        public ActivityStatusBuilder AsFinish()
        {
            _status.IsFinish = true;

            return this;
        }

        public ActivityStatusBuilder AsDeleted()
        {
            _status.IsDeleted = true;

            return this;
        }

        public async Task<ActivityStatus> BuildAsync()
        {
            var accessToken = await _accessTokenGetter.GetAsync();

            var id = await _activityStatusesClient.CreateAsync(accessToken, _status);

            return await _activityStatusesClient.GetAsync(accessToken, id);
        }
    }
}