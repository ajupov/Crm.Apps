using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Crm.Apps.Tests.Creator;
using Crm.Clients.Users.Clients;
using Crm.Clients.Users.Models;
using Crm.Common.Types;
using Crm.Utils.DateTime;
using Xunit;

namespace Crm.Apps.Tests.Tests.Users
{
    public class UserAttributesTests
    {
        private readonly ICreate _create;
        private readonly IUserAttributesClient _userAttributesClient;

        public UserAttributesTests(ICreate create, IUserAttributesClient userAttributesClient)
        {
            _create = create;
            _userAttributesClient = userAttributesClient;
        }

        [Fact]
        public async Task WhenGetTypes_ThenSuccess()
        {
            var types = await _userAttributesClient.GetTypesAsync();

            Assert.NotEmpty(types);
        }

        [Fact]
        public async Task WhenGet_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync();
            var attributeId = (await _create.UserAttribute.WithAccountId(account.Id).BuildAsync())
                .Id;

            var attribute = await _userAttributesClient.GetAsync(attributeId);

            Assert.NotNull(attribute);
            Assert.Equal(attributeId, attribute.Id);
        }

        [Fact]
        public async Task WhenGetList_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync();
            var attributeIds = (await Task.WhenAll(
                    _create.UserAttribute.WithAccountId(account.Id).WithKey("Test1").BuildAsync(),
                    _create.UserAttribute.WithAccountId(account.Id).WithKey("Test2").BuildAsync())
                ).Select(x => x.Id).ToList();

            var attributes = await _userAttributesClient.GetListAsync(attributeIds);

            Assert.NotEmpty(attributes);
            Assert.Equal(attributeIds.Count, attributes.Count);
        }

        [Fact]
        public async Task WhenGetPagedList_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync();
            await Task.WhenAll(_create.UserAttribute.WithAccountId(account.Id).WithType(AttributeType.Text)
                .WithKey("Test1").BuildAsync());
            var filterTypes = new List<AttributeType> {AttributeType.Text};

            var attributes = await _userAttributesClient.GetPagedListAsync(account.Id, key: "Test1", types: filterTypes,
                sortBy: "CreateDateTime", orderBy: "desc");

            var results = attributes.Skip(1).Zip(attributes,
                (previous, current) => current.CreateDateTime >= previous.CreateDateTime);

            Assert.NotEmpty(attributes);
            Assert.All(results, Assert.True);
        }

        [Fact]
        public async Task WhenCreate_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync();
            var attribute = new UserAttribute
            {
                AccountId = account.Id,
                Type = AttributeType.Text,
                Key = "Test",
                IsDeleted = false
            };

            var createdAttributeId = await _userAttributesClient.CreateAsync(attribute);

            var createdAttribute = await _userAttributesClient.GetAsync(createdAttributeId);

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
            var account = await _create.Account.BuildAsync();
            var attribute = await _create.UserAttribute.WithAccountId(account.Id).WithType(AttributeType.Text)
                .WithKey("Test").BuildAsync();

            attribute.Type = AttributeType.Link;
            attribute.Key = "test.com";
            attribute.IsDeleted = true;

            await _userAttributesClient.UpdateAsync(attribute);

            var updatedAttribute = await _userAttributesClient.GetAsync(attribute.Id);

            Assert.Equal(attribute.Type, updatedAttribute.Type);
            Assert.Equal(attribute.Key, updatedAttribute.Key);
            Assert.Equal(attribute.IsDeleted, updatedAttribute.IsDeleted);
        }

        [Fact]
        public async Task WhenDelete_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync();
            var attributeIds = (await Task.WhenAll(
                    _create.UserAttribute.WithAccountId(account.Id).WithKey("Test1").BuildAsync(),
                    _create.UserAttribute.WithAccountId(account.Id).WithKey("Test2").BuildAsync())
                ).Select(x => x.Id).ToList();

            await _userAttributesClient.DeleteAsync(attributeIds);

            var attributes = await _userAttributesClient.GetListAsync(attributeIds);

            Assert.All(attributes, x => Assert.True(x.IsDeleted));
        }

        [Fact]
        public async Task WhenRestore_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync();
            var attributeIds = (await Task.WhenAll(
                    _create.UserAttribute.WithAccountId(account.Id).WithKey("Test1").BuildAsync(),
                    _create.UserAttribute.WithAccountId(account.Id).WithKey("Test2").BuildAsync())
                ).Select(x => x.Id).ToList();

            await _userAttributesClient.RestoreAsync(attributeIds);

            var attributes = await _userAttributesClient.GetListAsync(attributeIds);

            Assert.All(attributes, x => Assert.False(x.IsDeleted));
        }
    }
}