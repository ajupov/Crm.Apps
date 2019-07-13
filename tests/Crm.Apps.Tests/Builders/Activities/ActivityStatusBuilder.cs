using System;
using System.Threading.Tasks;
using Crm.Clients.Activities.Clients;
using Crm.Clients.Activities.Models;
using Crm.Utils.Guid;

namespace Crm.Apps.Tests.Builders.Activities
{
    public class ActivityStatusBuilder : IActivityStatusBuilder
    {
        private readonly IActivityStatusesClient _activityStatusesClient;
        private readonly ActivityStatus _activityStatus;

        public ActivityStatusBuilder(IActivityStatusesClient activityStatusesClient)
        {
            _activityStatusesClient = activityStatusesClient;
            _activityStatus = new ActivityStatus
            {
                AccountId = Guid.Empty,
                Name = "Test"
            };
        }

        public ActivityStatusBuilder WithAccountId(Guid accountId)
        {
            _activityStatus.AccountId = accountId;

            return this;
        }

        public ActivityStatusBuilder WithName(string name)
        {
            _activityStatus.Name = name;

            return this;
        }

        public ActivityStatusBuilder AsFinish()
        {
            _activityStatus.IsFinish = true;

            return this;
        }

        public async Task<ActivityStatus> BuildAsync()
        {
            if (_activityStatus.AccountId.IsEmpty())
            {
                throw new InvalidOperationException(nameof(_activityStatus.AccountId));
            }

            var createdId = await _activityStatusesClient.CreateAsync(_activityStatus);

            return await _activityStatusesClient.GetAsync(createdId);
        }
    }
}