using System;
using System.Threading.Tasks;
using Crm.Clients.Activities.Clients;
using Crm.Clients.Activities.Models;
using Crm.Clients.Activities.RequestParameters;
using Crm.Utils.Guid;

namespace Crm.Apps.Tests.Builders.Activities
{
    public class ActivityStatusBuilder : IActivityStatusBuilder
    {
        private readonly IActivityStatusesClient _activityStatusesClient;
        private readonly ActivityStatusCreateRequest _request;

        public ActivityStatusBuilder(IActivityStatusesClient activityStatusesClient)
        {
            _activityStatusesClient = activityStatusesClient;
            _request = new ActivityStatusCreateRequest
            {
                AccountId = Guid.Empty,
                Name = "Test"
            };
        }

        public ActivityStatusBuilder WithAccountId(Guid accountId)
        {
            _request.AccountId = accountId;

            return this;
        }

        public ActivityStatusBuilder WithName(string name)
        {
            _request.Name = name;

            return this;
        }

        public ActivityStatusBuilder AsFinish()
        {
            _request.IsFinish = true;

            return this;
        }

        public async Task<ActivityStatus> BuildAsync()
        {
            if (_request.AccountId.IsEmpty())
            {
                throw new InvalidOperationException(nameof(_request.AccountId));
            }

            var createdId = await _activityStatusesClient.CreateAsync(_request);

            return await _activityStatusesClient.GetAsync(createdId);
        }
    }
}