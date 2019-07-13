using System;
using System.Threading.Tasks;
using Crm.Clients.Activities.Clients;
using Crm.Clients.Activities.Models;
using Crm.Utils.Guid;

namespace Crm.Apps.Tests.Builders.Activities
{
    public class ActivityTypeBuilder : IActivityTypeBuilder
    {
        private readonly IActivityTypesClient _activityTypesClient;
        private readonly ActivityType _activityType;

        public ActivityTypeBuilder(IActivityTypesClient activityTypesClient)
        {
            _activityTypesClient = activityTypesClient;
            _activityType = new ActivityType
            {
                AccountId = Guid.Empty,
                Name = "Test"
            };
        }

        public ActivityTypeBuilder WithAccountId(Guid accountId)
        {
            _activityType.AccountId = accountId;

            return this;
        }

        public ActivityTypeBuilder WithName(string name)
        {
            _activityType.Name = name;

            return this;
        }

        public async Task<ActivityType> BuildAsync()
        {
            if (_activityType.AccountId.IsEmpty())
            {
                throw new InvalidOperationException(nameof(_activityType.AccountId));
            }

            var createdId = await _activityTypesClient.CreateAsync(_activityType).ConfigureAwait(false);

            return await _activityTypesClient.GetAsync(createdId).ConfigureAwait(false);
        }
    }
}