using System;
using System.Threading.Tasks;
using Crm.Apps.Clients.Activities.Clients;
using Crm.Apps.Clients.Activities.Models;

namespace Crm.Apps.Tests.Builders.Activities
{
    public class ActivityStatusBuilder : IActivityStatusBuilder
    {
        private readonly IActivityStatusesClient _activityStatusesClient;
        private readonly ActivityStatus _status;

        public ActivityStatusBuilder(IActivityStatusesClient activityStatusesClient)
        {
            _activityStatusesClient = activityStatusesClient;
            _status = new ActivityStatus
            {
                AccountId = Guid.Empty,
                Name = "Test",
                IsFinish = false,
                IsDeleted = false
            };
        }

        public ActivityStatusBuilder WithAccountId(Guid accountId)
        {
            _status.AccountId = accountId;

            return this;
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
            var id = await _activityStatusesClient.CreateAsync(_status);

            return await _activityStatusesClient.GetAsync(id);
        }
    }
}