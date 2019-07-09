using System;
using System.Threading.Tasks;
using Crm.Clients.Deals.Clients;
using Crm.Clients.Deals.Models;
using Crm.Utils.Guid;

namespace Crm.Apps.Tests.Builders.Deals
{
    public class DealTypeBuilder : IDealTypeBuilder
    {
        private readonly IDealTypesClient _dealTypesClient;
        private readonly DealType _dealType;

        public DealTypeBuilder(IDealTypesClient dealTypesClient)
        {
            _dealTypesClient = dealTypesClient;
            _dealType = new DealType
            {
                AccountId = Guid.Empty,
                Name = "Test"
            };
        }

        public DealTypeBuilder WithAccountId(Guid accountId)
        {
            _dealType.AccountId = accountId;

            return this;
        }

        public DealTypeBuilder WithName(string name)
        {
            _dealType.Name = name;

            return this;
        }

        public async Task<DealType> BuildAsync()
        {
            if (_dealType.AccountId.IsEmpty())
            {
                throw new InvalidOperationException(nameof(_dealType.AccountId));
            }

            var createdId = await _dealTypesClient.CreateAsync(_dealType).ConfigureAwait(false);

            return await _dealTypesClient.GetAsync(createdId).ConfigureAwait(false);
        }
    }
}