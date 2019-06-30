using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Crm.Apps.Tests.Creator;
using Crm.Clients.Leads.Clients;
using Crm.Clients.Leads.Models;
using Crm.Common.Types;
using Crm.Utils.DateTime;
using Xunit;

namespace Crm.Apps.Tests.Tests.Leads
{
    public class LeadAttributesTests
    {
        private readonly ICreate _create;

        private readonly ILeadAttributesClient _leadAttributesClient;

        public LeadAttributesTests(ICreate create, ILeadAttributesClient leadAttributesClient)
        {
            _create = create;
            _leadAttributesClient = leadAttributesClient;
        }

        [Fact]
        public async Task WhenGetTypes_ThenSuccess()
        {
            var types = await _leadAttributesClient.GetTypesAsync().ConfigureAwait(false);

            Assert.NotEmpty(types);
        }

        [Fact]
        public async Task WhenGet_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync().ConfigureAwait(false);
            var attributeId = (await _create.LeadAttribute.WithAccountId(account.Id).BuildAsync().ConfigureAwait(false))
                .Id;

            var attribute = await _leadAttributesClient.GetAsync(attributeId).ConfigureAwait(false);

            Assert.NotNull(attribute);
            Assert.Equal(attributeId, attribute.Id);
        }

        [Fact]
        public async Task WhenGetList_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync().ConfigureAwait(false);
            var attributeIds = (await Task.WhenAll(
                    _create.LeadAttribute.WithAccountId(account.Id).WithKey("Test1").BuildAsync(),
                    _create.LeadAttribute.WithAccountId(account.Id).WithKey("Test2").BuildAsync())
                .ConfigureAwait(false)).Select(x => x.Id).ToList();

            var attributes = await _leadAttributesClient.GetListAsync(attributeIds).ConfigureAwait(false);

            Assert.NotEmpty(attributes);
            Assert.Equal(attributeIds.Count, attributes.Count);
        }

        [Fact]
        public async Task WhenGetPagedList_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync().ConfigureAwait(false);
            await Task.WhenAll(_create.LeadAttribute.WithAccountId(account.Id).WithType(AttributeType.Text)
                .WithKey("Test1").BuildAsync()).ConfigureAwait(false);
            var filterTypes = new List<AttributeType> {AttributeType.Text};

            var attributes = await _leadAttributesClient.GetPagedListAsync(account.Id, key: "Test1",
                types: filterTypes,
                sortBy: "CreateDateTime", orderBy: "desc").ConfigureAwait(false);

            var results = attributes.Skip(1).Zip(attributes,
                (previous, current) => current.CreateDateTime >= previous.CreateDateTime);

            Assert.NotEmpty(attributes);
            Assert.All(results, Assert.True);
        }

        [Fact]
        public async Task WhenCreate_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync().ConfigureAwait(false);
            var attribute = new LeadAttribute
            {
                AccountId = account.Id,
                Type = AttributeType.Text,
                Key = "Test",
                IsDeleted = false
            };

            var createdAttributeId = await _leadAttributesClient.CreateAsync(attribute).ConfigureAwait(false);

            var createdAttribute = await _leadAttributesClient.GetAsync(createdAttributeId).ConfigureAwait(false);

            Assert.NotNull(createdAttribute);
            Assert.Equal(createdAttributeId, createdAttribute.Id);
            Assert.Equal(attribute.AccountId, createdAttribute.AccountId);
            Assert.Equal(attribute.Type, createdAttribute.Type);
            Assert.Equal(attribute.Key, createdAttribute.Key);
            Assert.Equal(attribute.IsDeleted, createdAttribute.IsDeleted);
            Assert.True(createdAttribute.CreateDateTime.IsMoreThanMinValue());
        }

        [Fact]
        public async Task WhenUpdate_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync().ConfigureAwait(false);
            var attribute = await _create.LeadAttribute.WithAccountId(account.Id).WithType(AttributeType.Text)
                .WithKey("Test").BuildAsync().ConfigureAwait(false);

            attribute.Type = AttributeType.Link;
            attribute.Key = "test.com";
            attribute.IsDeleted = true;

            await _leadAttributesClient.UpdateAsync(attribute).ConfigureAwait(false);

            var updatedAttribute = await _leadAttributesClient.GetAsync(attribute.Id).ConfigureAwait(false);

            Assert.Equal(attribute.Type, updatedAttribute.Type);
            Assert.Equal(attribute.Key, updatedAttribute.Key);
            Assert.Equal(attribute.IsDeleted, updatedAttribute.IsDeleted);
        }

        [Fact]
        public async Task WhenDelete_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync().ConfigureAwait(false);
            var attributeIds = (await Task.WhenAll(
                    _create.LeadAttribute.WithAccountId(account.Id).WithKey("Test1").BuildAsync(),
                    _create.LeadAttribute.WithAccountId(account.Id).WithKey("Test2").BuildAsync())
                .ConfigureAwait(false)).Select(x => x.Id).ToList();

            await _leadAttributesClient.DeleteAsync(attributeIds).ConfigureAwait(false);

            var attributes = await _leadAttributesClient.GetListAsync(attributeIds).ConfigureAwait(false);

            Assert.All(attributes, x => Assert.True(x.IsDeleted));
        }

        [Fact]
        public async Task WhenRestore_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync().ConfigureAwait(false);
            var attributeIds = (await Task.WhenAll(
                    _create.LeadAttribute.WithAccountId(account.Id).WithKey("Test1").BuildAsync(),
                    _create.LeadAttribute.WithAccountId(account.Id).WithKey("Test2").BuildAsync())
                .ConfigureAwait(false)).Select(x => x.Id).ToList();

            await _leadAttributesClient.RestoreAsync(attributeIds).ConfigureAwait(false);

            var attributes = await _leadAttributesClient.GetListAsync(attributeIds).ConfigureAwait(false);

            Assert.All(attributes, x => Assert.False(x.IsDeleted));
        }
    }
}