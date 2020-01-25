using System;
using System.Threading.Tasks;
using Crm.Apps.Tests.Extensions;
using Crm.Apps.Tests.Services.AccessTokenGetter;
using Crm.Apps.v1.Clients.Deals.Clients;
using Crm.Apps.v1.Clients.Deals.Models;

namespace Crm.Apps.Tests.Builders.Deals
{
    public class DealStatusBuilder : IDealStatusBuilder
    {
        private readonly IAccessTokenGetter _accessTokenGetter;
        private readonly IDealStatusesClient _dealStatusesClient;
        private readonly DealStatus _status;

        public DealStatusBuilder(IAccessTokenGetter accessTokenGetter, IDealStatusesClient dealStatusesClient)
        {
            _dealStatusesClient = dealStatusesClient;
            _accessTokenGetter = accessTokenGetter;
            _status = new DealStatus
            {
                AccountId = Guid.Empty,
                Name = "Test".WithGuid(),
                IsFinish = false,
                IsDeleted = false
            };
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
            var accessToken = await _accessTokenGetter.GetAsync();

            var id = await _dealStatusesClient.CreateAsync(accessToken, _status);

            return await _dealStatusesClient.GetAsync(accessToken, id);
        }
    }
}