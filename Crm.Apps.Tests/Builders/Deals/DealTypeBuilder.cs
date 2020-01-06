using System;
using System.Threading.Tasks;
using Crm.Apps.Clients.Deals.Clients;
using Crm.Apps.Clients.Deals.Models;

namespace Crm.Apps.Tests.Builders.Deals
{
    public class DealTypeBuilder : IDealTypeBuilder
    {
        private readonly IDealTypesClient _dealTypesClient;
        private readonly DealType _type;

        public DealTypeBuilder(IDealTypesClient dealTypesClient)
        {
            _dealTypesClient = dealTypesClient;
            _type = new DealType
            {
                AccountId = Guid.Empty,
                Name = "Test",
                IsDeleted = false
            };
        }

        public DealTypeBuilder WithName(string name)
        {
            _type.Name = name;

            return this;
        }

        public DealTypeBuilder AsDeleted()
        {
            _type.IsDeleted = true;

            return this;
        }

        public async Task<DealType> BuildAsync()
        {
            var id = await _dealTypesClient.CreateAsync(_type);

            return await _dealTypesClient.GetAsync(id);
        }
    }
}