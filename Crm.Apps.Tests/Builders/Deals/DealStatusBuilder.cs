using System;
using System.Threading.Tasks;
using Crm.Apps.Clients.Deals.Clients;
using Crm.Apps.Clients.Deals.Models;

namespace Crm.Apps.Tests.Builders.Deals
{
    public class DealStatusBuilder : IDealStatusBuilder
    {
        private readonly IDealStatusesClient _dealStatusesClient;
        private readonly DealStatus _status;

        public DealStatusBuilder(IDealStatusesClient dealStatusesClient)
        {
            _dealStatusesClient = dealStatusesClient;
            _status = new DealStatus
            {
                AccountId = Guid.Empty,
                Name = "Test",
                IsFinish = false,
                IsDeleted = false
            };
        }

        public DealStatusBuilder WithAccountId(Guid accountId)
        {
            _status.AccountId = accountId;

            return this;
        }

        public DealStatusBuilder WithName(string name)
        {
            _status.Name = name;

            return this;
        }

        public DealStatusBuilder AsFinish()
        {
            _status.IsFinish = true;

            return this;
        }

        public DealStatusBuilder AsDeleted()
        {
            _status.IsDeleted = true;

            return this;
        }

        public async Task<DealStatus> BuildAsync()
        {
            var id = await _dealStatusesClient.CreateAsync(_status);

            return await _dealStatusesClient.GetAsync(id);
        }
    }
}