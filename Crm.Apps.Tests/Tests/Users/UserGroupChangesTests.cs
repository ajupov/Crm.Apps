using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Crm.Apps.Tests.Creator;
using Crm.Clients.Users.Clients;
using Crm.Clients.Users.Models;
using Crm.Common.UserContext;
using Crm.Utils.DateTime;
using Crm.Utils.Guid;
using Crm.Utils.Json;
using Crm.Utils.String;
using Xunit;

namespace Crm.Apps.Tests.Tests.Users
{
    public class UserGroupChangesTests
    {
        private readonly ICreate _create;
        private readonly IUserGroupsClient _userGroupsClient;
        private readonly IUserGroupChangesClient _groupChangesClient;

        public UserGroupChangesTests(ICreate create, IUserGroupsClient userGroupsClient,
            IUserGroupChangesClient groupChangesClient)
        {
            _create = create;
            _userGroupsClient = userGroupsClient;
            _groupChangesClient = groupChangesClient;
        }

        [Fact]
        public async Task WhenGetPagedList_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync();
            var group = await _create.UserGroup.WithAccountId(account.Id).BuildAsync();
            group.Name = "Test2";
            group.Permissions = new List<UserGroupPermission> {new UserGroupPermission {Role = Role.None}};
            group.IsDeleted = true;
            await _userGroupsClient.UpdateAsync(group);

            var changes = await _groupChangesClient
                .GetPagedListAsync(groupId: group.Id, sortBy: "CreateDateTime", orderBy: "asc")
                ;

            Assert.NotEmpty(changes);
            Assert.True(changes.All(x => !x.ChangerUserId.IsEmpty()));
            Assert.True(changes.All(x => x.GroupId == group.Id));
            Assert.True(changes.All(x => x.CreateDateTime.IsMoreThanMinValue()));
            Assert.True(changes.First().OldValueJson.IsEmpty());
            Assert.True(!changes.First().NewValueJson.IsEmpty());
            Assert.NotNull(changes.First().NewValueJson.FromJsonString<UserGroup>());
            Assert.True(!changes.Last().OldValueJson.IsEmpty());
            Assert.True(!changes.Last().NewValueJson.IsEmpty());
            Assert.False(changes.Last().OldValueJson.FromJsonString<UserGroup>().IsDeleted);
            Assert.True(changes.Last().NewValueJson.FromJsonString<UserGroup>().IsDeleted);
            Assert.Equal(changes.Last().NewValueJson.FromJsonString<UserGroup>().Name, group.Name);
            Assert.Equal(changes.Last().NewValueJson.FromJsonString<UserGroup>().Permissions.Single().Permission,
                group.Permissions.Single().Role);
        }
    }
}