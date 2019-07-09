using System;
using System.Threading.Tasks;
using Crm.Clients.Deals.Clients;
using Crm.Clients.Deals.Models;
using Crm.Utils.Guid;

namespace Crm.Apps.Tests.Builders.Deals
{
    public class DealStatusBuilder : IDealStatusBuilder
    {
        private readonly IDealStatusesClient _dealStatusesClient;
        private readonly DealStatus _dealStatus;

        public DealStatusBuilder(IDealStatusesClient dealStatusesClient)
        {
            _dealStatusesClient = dealStatusesClient;
            _dealStatus = new DealStatus
            {
                AccountId = Guid.Empty,
                Name = "Test"
            };
        }

        public DealStatusBuilder WithAccountId(Guid accountId)
        {
            _dealStatus.AccountId = accountId;

            return this;
        }

        public DealStatusBuilder WithName(string name)
        {
            _dealStatus.Name = name;

            return this;
        }

        public DealStatusBuilder AsFinish()
        {
            _dealStatus.IsFinish = true;

            return this;
        }

        public async Task<DealStatus> BuildAsync()
        {
            if (_dealStatus.AccountId.IsEmpty())
            {
                throw new InvalidOperationException(nameof(_dealStatus.AccountId));
            }

            var createdId = await _dealStatusesClient.CreateAsync(_dealStatus).ConfigureAwait(false);

            return await _dealStatusesClient.GetAsync(createdId).ConfigureAwait(false);
        }
    }
}