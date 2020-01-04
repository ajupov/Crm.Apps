using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Crm.Apps.Tests.Creator;
using Crm.Clients.Activities.Clients;
using Crm.Clients.Activities.RequestParameters;
using Crm.Common.Types;
using Crm.Utils.DateTime;
using Xunit;

namespace Crm.Apps.Tests.Tests.Activities
{
    public class ActivityAttributesTests
    {
        private readonly ICreate _create;
        private readonly IActivityAttributesClient _activityAttributesClient;

        public ActivityAttributesTests(ICreate create, IActivityAttributesClient activityAttributesClient)
        {
            _create = create;
            _activityAttributesClient = activityAttributesClient;
        }

        [Fact]
        public async Task WhenGetTypes_ThenSuccess()
        {
            var types = await _activityAttributesClient.GetTypesAsync();

            Assert.NotEmpty(types);
        }

        [Fact]
        public async Task WhenGet_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync();
            var attributeId = (await _create.ActivityAttribute.WithAccountId(account.Id).BuildAsync()).Id;

            var attribute = await _activityAttributesClient.GetAsync(attributeId);

            Assert.NotNull(attribute);
            Assert.Equal(attributeId, attribute.Id);
        }

        [Fact]
        public async Task WhenGetList_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync();
            var attributeIds = (await Task.WhenAll(
                    _create.ActivityAttribute.WithAccountId(account.Id).WithKey("Test1").BuildAsync(),
                    _create.ActivityAttribute.WithAccountId(account.Id).WithKey("Test2").BuildAsync())
                )
                .Select(x => x.Id)
                .ToArray();

            var attributes = await _activityAttributesClient.GetListAsync(attributeIds);

            Assert.NotEmpty(attributes);
            Assert.Equal(attributeIds.Length, attributes.Length);
        }

        [Fact]
        public async Task WhenGetPagedList_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync();
            await Task.WhenAll(_create.ActivityAttribute.WithAccountId(account.Id).WithType(AttributeType.Text)
                .WithKey("Test1").BuildAsync());
            var filterTypes = new List<AttributeType> {AttributeType.Text};

            var request = new ActivityAttributeGetPagedListRequest
            {
                AccountId = account.Id,
                Key = "Test1",
                Types = filterTypes,
            };

            var attributes = await _activityAttributesClient.GetPagedListAsync(request);

            var results = attributes
                .Skip(1)
                .Zip(attributes, (previous, current) => current.CreateDateTime >= previous.CreateDateTime);

            Assert.NotEmpty(attributes);
            Assert.All(results, Assert.True);
        }

        [Fact]
        public async Task WhenCreate_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync();
            var request = new ActivityAttributeCreateRequest
            {
                AccountId = account.Id,
                Type = AttributeType.Text,
                Key = "Test",
                IsDeleted = false
            };

            var createdAttributeId = await _activityAttributesClient.CreateAsync(request);

            var createdAttribute = await _activityAttributesClient.GetAsync(createdAttributeId);

            Assert.NotNull(createdAttribute);
            Assert.Equal(createdAttributeId, createdAttribute.Id);
            Assert.Equal(request.AccountId, createdAttribute.AccountId);
            Assert.Equal(request.Type, createdAttribute.Type);
            Assert.Equal(request.Key, createdAttribute.Key);
            Assert.Equal(request.IsDeleted, createdAttribute.IsDeleted);
            Assert.True(createdAttribute.CreateDateTime.IsMoreThanMinValue());
        }

        [Fact]
        public async Task WhenUpdate_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync();
            var attribute = await _create.ActivityAttribute.WithAccountId(account.Id).WithType(AttributeType.Text)
                .WithKey("Test").BuildAsync();

            var request = new ActivityAttributeUpdateRequest
            {
                Type = AttributeType.Link,
                Key = "test.com",
                IsDeleted = true
            };

            await _activityAttributesClient.UpdateAsync(request);

            var updatedAttribute = await _activityAttributesClient.GetAsync(attribute.Id);

            Assert.Equal(attribute.Type, updatedAttribute.Type);
            Assert.Equal(attribute.Key, updatedAttribute.Key);
            Assert.Equal(attribute.IsDeleted, updatedAttribute.IsDeleted);
        }

        [Fact]
        public async Task WhenDelete_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync();
            var attributeIds = (await Task.WhenAll(
                    _create.ActivityAttribute.WithAccountId(account.Id).WithKey("Test1").BuildAsync(),
                    _create.ActivityAttribute.WithAccountId(account.Id).WithKey("Test2").BuildAsync())
                ).Select(x => x.Id).ToList();

            await _activityAttributesClient.DeleteAsync(attributeIds);

            var attributes = await _activityAttributesClient.GetListAsync(attributeIds);

            Assert.All(attributes, x => Assert.True(x.IsDeleted));
        }

        [Fact]
        public async Task WhenRestore_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync();
            var attributeIds = (await Task.WhenAll(
                    _create.ActivityAttribute.WithAccountId(account.Id).WithKey("Test1").BuildAsync(),
                    _create.ActivityAttribute.WithAccountId(account.Id).WithKey("Test2").BuildAsync())
                ).Select(x => x.Id).ToList();

            await _activityAttributesClient.RestoreAsync(attributeIds);

            var attributes = await _activityAttributesClient.GetListAsync(attributeIds);

            Assert.All(attributes, x => Assert.False(x.IsDeleted));
        }
    }
}