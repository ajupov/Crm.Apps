using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Crm.Apps.Tests.Creator;
using Crm.Clients.Contacts.Clients;
using Crm.Clients.Contacts.Models;
using Crm.Common.Types;
using Crm.Utils.DateTime;
using Xunit;

namespace Crm.Apps.Tests.Tests.Contacts
{
    public class ContactAttributesTests
    {
        private readonly ICreate _create;
        private readonly IContactAttributesClient _contactAttributesClient;

        public ContactAttributesTests(ICreate create, IContactAttributesClient contactAttributesClient)
        {
            _create = create;
            _contactAttributesClient = contactAttributesClient;
        }

        [Fact]
        public async Task WhenGetTypes_ThenSuccess()
        {
            var types = await _contactAttributesClient.GetTypesAsync().ConfigureAwait(false);

            Assert.NotEmpty(types);
        }

        [Fact]
        public async Task WhenGet_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync().ConfigureAwait(false);
            var attributeId =
                (await _create.ContactAttribute.WithAccountId(account.Id).BuildAsync().ConfigureAwait(false))
                .Id;

            var attribute = await _contactAttributesClient.GetAsync(attributeId).ConfigureAwait(false);

            Assert.NotNull(attribute);
            Assert.Equal(attributeId, attribute.Id);
        }

        [Fact]
        public async Task WhenGetList_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync().ConfigureAwait(false);
            var attributeIds = (await Task.WhenAll(
                    _create.ContactAttribute.WithAccountId(account.Id).WithKey("Test1").BuildAsync(),
                    _create.ContactAttribute.WithAccountId(account.Id).WithKey("Test2").BuildAsync())
                .ConfigureAwait(false)).Select(x => x.Id).ToList();

            var attributes = await _contactAttributesClient.GetListAsync(attributeIds).ConfigureAwait(false);

            Assert.NotEmpty(attributes);
            Assert.Equal(attributeIds.Count, attributes.Count);
        }

        [Fact]
        public async Task WhenGetPagedList_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync().ConfigureAwait(false);
            await Task.WhenAll(_create.ContactAttribute.WithAccountId(account.Id).WithType(AttributeType.Text)
                .WithKey("Test1").BuildAsync()).ConfigureAwait(false);
            var filterTypes = new List<AttributeType> {AttributeType.Text};

            var attributes = await _contactAttributesClient.GetPagedListAsync(account.Id, key: "Test1",
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
            var attribute = new ContactAttribute
            {
                AccountId = account.Id,
                Type = AttributeType.Text,
                Key = "Test",
                IsDeleted = false
            };

            var createdAttributeId = await _contactAttributesClient.CreateAsync(attribute).ConfigureAwait(false);

            var createdAttribute = await _contactAttributesClient.GetAsync(createdAttributeId).ConfigureAwait(false);

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
            var attribute = await _create.ContactAttribute.WithAccountId(account.Id).WithType(AttributeType.Text)
                .WithKey("Test").BuildAsync().ConfigureAwait(false);

            attribute.Type = AttributeType.Link;
            attribute.Key = "test.com";
            attribute.IsDeleted = true;

            await _contactAttributesClient.UpdateAsync(attribute).ConfigureAwait(false);

            var updatedAttribute = await _contactAttributesClient.GetAsync(attribute.Id).ConfigureAwait(false);

            Assert.Equal(attribute.Type, updatedAttribute.Type);
            Assert.Equal(attribute.Key, updatedAttribute.Key);
            Assert.Equal(attribute.IsDeleted, updatedAttribute.IsDeleted);
        }

        [Fact]
        public async Task WhenDelete_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync().ConfigureAwait(false);
            var attributeIds = (await Task.WhenAll(
                    _create.ContactAttribute.WithAccountId(account.Id).WithKey("Test1").BuildAsync(),
                    _create.ContactAttribute.WithAccountId(account.Id).WithKey("Test2").BuildAsync())
                .ConfigureAwait(false)).Select(x => x.Id).ToList();

            await _contactAttributesClient.DeleteAsync(attributeIds).ConfigureAwait(false);

            var attributes = await _contactAttributesClient.GetListAsync(attributeIds).ConfigureAwait(false);

            Assert.All(attributes, x => Assert.True(x.IsDeleted));
        }

        [Fact]
        public async Task WhenRestore_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync().ConfigureAwait(false);
            var attributeIds = (await Task.WhenAll(
                    _create.ContactAttribute.WithAccountId(account.Id).WithKey("Test1").BuildAsync(),
                    _create.ContactAttribute.WithAccountId(account.Id).WithKey("Test2").BuildAsync())
                .ConfigureAwait(false)).Select(x => x.Id).ToList();

            await _contactAttributesClient.RestoreAsync(attributeIds).ConfigureAwait(false);

            var attributes = await _contactAttributesClient.GetListAsync(attributeIds).ConfigureAwait(false);

            Assert.All(attributes, x => Assert.False(x.IsDeleted));
        }
    }
}