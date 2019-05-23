using System.Linq;
using System.Threading.Tasks;
using Crm.Apps.Tests.Dsl.Creator;
using Crm.Clients.Users.Clients.UserGroupChanges;
using Crm.Clients.Users.Clients.Users;
using Crm.Clients.Users.Models;
using Crm.Utils.DateTime;
using Crm.Utils.Guid;
using Crm.Utils.Json;
using Crm.Utils.String;
using Xunit;

namespace Crm.Apps.Tests.Users
{
    public class UserGroupChangesTests
    {
        private readonly ICreate _create;
        private readonly IUsersClient _groupsClient;
        private readonly IUserGroupChangesClient _groupChangesClient;

        public UserGroupChangesTests(ICreate create, IUsersClient groupsClient,
            IUserGroupChangesClient groupChangesClient)
        {
            _create = create;
            _groupsClient = groupsClient;
            _groupChangesClient = groupChangesClient;
        }

        [Fact]
        public async Task WhenGetPagedList_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync().ConfigureAwait(false);
            var group = await _create.User.WithAccountId(account.Id).BuildAsync().ConfigureAwait(false);
            group.IsLocked = true;
            await _groupsClient.UpdateAsync(group).ConfigureAwait(false);

            var changes = await _groupChangesClient
                .GetPagedListAsync(groupId: group.Id, sortBy: "CreateDateTime", orderBy: "asc")
                .ConfigureAwait(false);

            Assert.NotEmpty(changes);
            Assert.True(changes.All(x => !x.ChangerUserId.IsEmpty()));
            Assert.True(changes.All(x => x.GroupId == group.Id));
            Assert.True(changes.All(x => x.CreateDateTime.IsMoreThanMinValue()));
            Assert.True(changes.First().OldValueJson.IsEmpty());
            Assert.True(!changes.First().NewValueJson.IsEmpty());
            Assert.NotNull(changes.First().NewValueJson.FromJsonString<User>());
            Assert.True(!changes.Last().OldValueJson.IsEmpty());
            Assert.True(!changes.Last().NewValueJson.IsEmpty());
            Assert.False(changes.Last().OldValueJson.FromJsonString<User>().IsLocked);
            Assert.True(changes.Last().NewValueJson.FromJsonString<User>().IsLocked);
        }
    }
}