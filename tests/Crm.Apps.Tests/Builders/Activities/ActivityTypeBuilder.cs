using System;
using System.Threading.Tasks;
using Crm.Clients.Activities.Clients;
using Crm.Clients.Activities.Models;
using Crm.Clients.Activities.RequestParameters;
using Crm.Utils.Guid;

namespace Crm.Apps.Tests.Builders.Activities
{
    public class ActivityTypeBuilder : IActivityTypeBuilder
    {
        private readonly IActivityTypesClient _activityTypesClient;
        private readonly ActivityTypeCreateRequest _request;

        public ActivityTypeBuilder(IActivityTypesClient activityTypesClient)
        {
            _activityTypesClient = activityTypesClient;
            _request = new ActivityTypeCreateRequest
            {
                AccountId = Guid.Empty,
                Name = "Test"
            };
        }

        public ActivityTypeBuilder WithAccountId(Guid accountId)
        {
            _request.AccountId = accountId;

            return this;
        }

        public ActivityTypeBuilder WithName(string name)
        {
            _request.Name = name;

            return this;
        }

        public async Task<ActivityType> BuildAsync()
        {
            if (_request.AccountId.IsEmpty())
            {
                throw new InvalidOperationException(nameof(_request.AccountId));
            }

            var createdId = await _activityTypesClient.CreateAsync(_request);

            return await _activityTypesClient.GetAsync(createdId);
        }
    }
}