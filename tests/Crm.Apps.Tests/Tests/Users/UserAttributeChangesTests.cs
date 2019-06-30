using System.Linq;
using System.Threading.Tasks;
using Crm.Apps.Tests.Creator;
using Crm.Clients.Users.Clients;
using Crm.Clients.Users.Models;
using Crm.Common.Types;
using Crm.Utils.DateTime;
using Crm.Utils.Guid;
using Crm.Utils.Json;
using Crm.Utils.String;
using Xunit;

namespace Crm.Apps.Tests.Tests.Users
{
    public class UserAttributeChangesTests
    {
        private readonly ICreate _create;
        private readonly IUserAttributesClient _userAttributesClient;
        private readonly IUserAttributeChangesClient _attributeChangesClient;

        public UserAttributeChangesTests(ICreate create, IUserAttributesClient userAttributesClient,
            IUserAttributeChangesClient attributeChangesClient)
        {
            _create = create;
            _userAttributesClient = userAttributesClient;
            _attributeChangesClient = attributeChangesClient;
        }

        [Fact]
        public async Task WhenGetPagedList_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync().ConfigureAwait(false);
            var attribute = await _create.UserAttribute.WithAccountId(account.Id).BuildAsync().ConfigureAwait(false);
            attribute.Type = AttributeType.Link;
            attribute.Key = "TestLink";
            attribute.IsDeleted = true;
            await _userAttributesClient.UpdateAsync(attribute).ConfigureAwait(false);

            var changes = await _attributeChangesClient
                .GetPagedListAsync(attributeId: attribute.Id, sortBy: "CreateDateTime", orderBy: "asc")
                .ConfigureAwait(false);

            Assert.NotEmpty(changes);
            Assert.True(changes.All(x => !x.ChangerUserId.IsEmpty()));
            Assert.True(changes.All(x => x.AttributeId == attribute.Id));
            Assert.True(changes.All(x => x.CreateDateTime.IsMoreThanMinValue()));
            Assert.True(changes.First().OldValueJson.IsEmpty());
            Assert.True(!changes.First().NewValueJson.IsEmpty());
            Assert.NotNull(changes.First().NewValueJson.FromJsonString<UserAttribute>());
            Assert.True(!changes.Last().OldValueJson.IsEmpty());
            Assert.True(!changes.Last().NewValueJson.IsEmpty());
            Assert.False(changes.Last().OldValueJson.FromJsonString<UserAttribute>().IsDeleted);
            Assert.True(changes.Last().NewValueJson.FromJsonString<UserAttribute>().IsDeleted);
            Assert.Equal(changes.Last().NewValueJson.FromJsonString<UserAttribute>().Key, attribute.Key);
            Assert.Equal(changes.Last().NewValueJson.FromJsonString<UserAttribute>().Type, attribute.Type);
        }
    }
}