using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ajupov.Utils.All.DateTime;
using Crm.Apps.Tests.Services.Creator;
using Crm.Apps.v1.Clients.Activities.Clients;
using Crm.Apps.v1.Clients.Activities.Models;
using Crm.Apps.v1.Clients.Activities.RequestParameters;
using Crm.Common.All.Types.AttributeType;
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
            var types = await _activityAttributesClient.GetTypesAsync(TODO);

            Assert.NotEmpty(types);
        }

        [Fact]
        public async Task WhenGet_ThenSuccess()
        {
            var attributeId = (await _create.ActivityAttribute.BuildAsync()).Id;

            var attribute = await _activityAttributesClient.GetAsync(attributeId);

            Assert.NotNull(attribute);
            Assert.Equal(attributeId, attribute.Id);
        }

        [Fact]
        public async Task WhenGetList_ThenSuccess()
        {
            var attributeIds = (
                    await Task.WhenAll(
                        _create.ActivityAttribute
                            .WithKey("Test1")
                            .BuildAsync(),
                        _create.ActivityAttribute
                            .WithKey("Test2")
                            .BuildAsync())
                )
                .Select(x => x.Id)
                .ToList();

            var attributes = await _activityAttributesClient.GetListAsync(attributeIds);

            Assert.NotEmpty(attributes);
            Assert.Equal(attributeIds.Count, attributes.Count);
        }

        [Fact]
        public async Task WhenGetPagedList_ThenSuccess()
        {
            await Task.WhenAll(
                _create.ActivityAttribute
                    .WithType(AttributeType.Text)
                    .WithKey("Test1")
                    .BuildAsync());
            var filterTypes = new List<AttributeType> {AttributeType.Text};

            var request = new ActivityAttributeGetPagedListRequestParameter
            {
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
            var attribute = new ActivityAttribute
            {
                Type = AttributeType.Text,
                Key = "Test",
                IsDeleted = false
            };
            var createdAttributeId = await _activityAttributesClient.CreateAsync(attribute);

            var createdAttribute = await _activityAttributesClient.GetAsync(createdAttributeId);

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
            var attribute = await _create.ActivityAttribute
                .WithType(AttributeType.Text)
                .WithKey("Test")
                .BuildAsync();

            attribute.Type = AttributeType.Link;
            attribute.Key = "test.com";
            attribute.IsDeleted = true;

            await _activityAttributesClient.UpdateAsync(attribute);

            var updatedAttribute = await _activityAttributesClient.GetAsync(attribute.Id);

            Assert.Equal(attribute.Type, updatedAttribute.Type);
            Assert.Equal(attribute.Key, updatedAttribute.Key);
            Assert.Equal(attribute.IsDeleted, updatedAttribute.IsDeleted);
        }

        [Fact]
        public async Task WhenDelete_ThenSuccess()
        {
            var attributeIds = (
                    await Task.WhenAll(
                        _create.ActivityAttribute
                            .WithKey("Test1")
                            .BuildAsync(),
                        _create.ActivityAttribute
                            .WithKey("Test2")
                            .BuildAsync())
                )
                .Select(x => x.Id)
                .ToList();

            await _activityAttributesClient.DeleteAsync(attributeIds);

            var attributes = await _activityAttributesClient.GetListAsync(attributeIds);

            Assert.All(attributes, x => Assert.True(x.IsDeleted));
        }

        [Fact]
        public async Task WhenRestore_ThenSuccess()
        {
            var attributeIds = (
                    await Task.WhenAll(
                        _create.ActivityAttribute
                            .WithKey("Test1")
                            .BuildAsync(),
                        _create.ActivityAttribute
                            .WithKey("Test2")
                            .BuildAsync())
                )
                .Select(x => x.Id)
                .ToList();

            await _activityAttributesClient.RestoreAsync(attributeIds);

            var attributes = await _activityAttributesClient.GetListAsync(attributeIds);

            Assert.All(attributes, x => Assert.False(x.IsDeleted));
        }
    }
}