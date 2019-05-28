using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Crm.Apps.Tests.Dsl.Creator;
using Crm.Clients.Users.Clients;
using Crm.Clients.Users.Models;
using Crm.Common.UserContext;
using Crm.Utils.DateTime;
using Xunit;

namespace Crm.Apps.Tests.Tests.Users
{
    public class UserGroupsTests
    {
        private readonly ICreate _create;

        private readonly IUserGroupsClient _userGroupsClient;

        public UserGroupsTests(ICreate create, IUserGroupsClient userGroupsClient)
        {
            _create = create;
            _userGroupsClient = userGroupsClient;
        }

        [Fact]
        public async Task WhenGet_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync().ConfigureAwait(false);
            var groupId = (await _create.UserGroup.WithAccountId(account.Id).BuildAsync().ConfigureAwait(false)).Id;

            var group = await _userGroupsClient.GetAsync(groupId).ConfigureAwait(false);

            Assert.NotNull(group);
            Assert.Equal(groupId, group.Id);
        }

        [Fact]
        public async Task WhenGetList_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync().ConfigureAwait(false);
            var groupIds = (await Task.WhenAll(
                    _create.UserGroup.WithAccountId(account.Id).WithName("Test1").BuildAsync(),
                    _create.UserGroup.WithAccountId(account.Id).WithName("Test2").BuildAsync())
                .ConfigureAwait(false)).Select(x => x.Id).ToList();

            var groups = await _userGroupsClient.GetListAsync(groupIds).ConfigureAwait(false);

            Assert.NotEmpty(groups);
            Assert.Equal(groupIds.Count, groups.Count);
        }

        [Fact]
        public async Task WhenGetPagedList_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync().ConfigureAwait(false);
            await Task.WhenAll(_create.UserGroup.WithAccountId(account.Id).WithName("Test1").BuildAsync())
                .ConfigureAwait(false);

            var groups = await _userGroupsClient
                .GetPagedListAsync(account.Id, "Test1", sortBy: "CreateDateTime", orderBy: "desc")
                .ConfigureAwait(false);

            var results = groups.Skip(1).Zip(groups,
                (previous, current) => current.CreateDateTime >= previous.CreateDateTime);

            Assert.NotEmpty(groups);
            Assert.All(results, Assert.True);
        }

        [Fact]
        public async Task WhenCreate_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync().ConfigureAwait(false);
            var group = new UserGroup
            {
                AccountId = account.Id,
                Name = "Test",
                IsDeleted = false,
                Permissions = new List<UserGroupPermission>
                {
                    new UserGroupPermission
                    {
                        Permission = Permission.None
                    }
                }
            };

            var createdGroupId = await _userGroupsClient.CreateAsync(group).ConfigureAwait(false);

            var createdGroup = await _userGroupsClient.GetAsync(createdGroupId).ConfigureAwait(false);

            Assert.NotNull(createdGroup);
            Assert.Equal(createdGroupId, createdGroup.Id);
            Assert.Equal(group.AccountId, createdGroup.AccountId);
            Assert.Equal(group.Name, createdGroup.Name);
            Assert.Equal(group.IsDeleted, createdGroup.IsDeleted);
            Assert.Equal(group.Permissions.Single().Permission, createdGroup.Permissions.Single().Permission);
            Assert.True(createdGroup.CreateDateTime.IsMoreThanMinValue());
        }

        [Fact]
        public async Task WhenUpdate_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync().ConfigureAwait(false);
            var group = await _create.UserGroup.WithAccountId(account.Id).WithName("Test1").BuildAsync()
                .ConfigureAwait(false);

            group.Name = "Test2";
            group.IsDeleted = true;
            group.Permissions.Add(new UserGroupPermission {Permission = Permission.None});

            await _userGroupsClient.UpdateAsync(group).ConfigureAwait(false);

            var updatedGroup = await _userGroupsClient.GetAsync(group.Id).ConfigureAwait(false);

            Assert.Equal(group.Name, updatedGroup.Name);
            Assert.Equal(group.IsDeleted, updatedGroup.IsDeleted);
            Assert.Equal(group.Permissions.Single().Permission, updatedGroup.Permissions.Single().Permission);
        }

        [Fact]
        public async Task WhenDelete_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync().ConfigureAwait(false);
            var groupIds = (await Task.WhenAll(
                    _create.UserGroup.WithAccountId(account.Id).WithName("Test1").BuildAsync(),
                    _create.UserGroup.WithAccountId(account.Id).WithName("Test2").BuildAsync())
                .ConfigureAwait(false)).Select(x => x.Id).ToList();

            await _userGroupsClient.DeleteAsync(groupIds).ConfigureAwait(false);

            var groups = await _userGroupsClient.GetListAsync(groupIds).ConfigureAwait(false);

            Assert.All(groups, x => Assert.True(x.IsDeleted));
        }

        [Fact]
        public async Task WhenRestore_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync().ConfigureAwait(false);
            var groupIds = (await Task.WhenAll(
                    _create.UserGroup.WithAccountId(account.Id).WithName("Test1").BuildAsync(),
                    _create.UserGroup.WithAccountId(account.Id).WithName("Test2").BuildAsync())
                .ConfigureAwait(false)).Select(x => x.Id).ToList();

            await _userGroupsClient.RestoreAsync(groupIds).ConfigureAwait(false);

            var groups = await _userGroupsClient.GetListAsync(groupIds).ConfigureAwait(false);

            Assert.All(groups, x => Assert.False(x.IsDeleted));
        }
    }
}