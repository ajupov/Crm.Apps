using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Crm.Apps.Tests.Creator;
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
            var account = await _create.Account.BuildAsync();
            var groupId = (await _create.UserGroup.WithAccountId(account.Id).BuildAsync()).Id;

            var group = await _userGroupsClient.GetAsync(groupId);

            Assert.NotNull(group);
            Assert.Equal(groupId, group.Id);
        }

        [Fact]
        public async Task WhenGetList_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync();
            var groupIds = (await Task.WhenAll(
                    _create.UserGroup.WithAccountId(account.Id).WithName("Test1").BuildAsync(),
                    _create.UserGroup.WithAccountId(account.Id).WithName("Test2").BuildAsync())
                ).Select(x => x.Id).ToList();

            var groups = await _userGroupsClient.GetListAsync(groupIds);

            Assert.NotEmpty(groups);
            Assert.Equal(groupIds.Count, groups.Count);
        }

        [Fact]
        public async Task WhenGetPagedList_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync();
            await Task.WhenAll(_create.UserGroup.WithAccountId(account.Id).WithName("Test1").BuildAsync())
                ;

            var groups = await _userGroupsClient
                .GetPagedListAsync(account.Id, "Test1", sortBy: "CreateDateTime", orderBy: "desc")
                ;

            var results = groups.Skip(1).Zip(groups,
                (previous, current) => current.CreateDateTime >= previous.CreateDateTime);

            Assert.NotEmpty(groups);
            Assert.All(results, Assert.True);
        }

        [Fact]
        public async Task WhenCreate_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync();
            var group = new UserGroup
            {
                AccountId = account.Id,
                Name = "Test",
                IsDeleted = false,
                Roles = new List<UserGroupRole>
                {
                    new UserGroupRole
                    {
                        Role = Role.None
                    }
                }
            };

            var createdGroupId = await _userGroupsClient.CreateAsync(group);

            var createdGroup = await _userGroupsClient.GetAsync(createdGroupId);

            Assert.NotNull(createdGroup);
            Assert.Equal(createdGroupId, createdGroup.Id);
            Assert.Equal(group.AccountId, createdGroup.AccountId);
            Assert.Equal(group.Name, createdGroup.Name);
            Assert.Equal(group.IsDeleted, createdGroup.IsDeleted);
            Assert.Equal(group.Roles.Single().Role, createdGroup.Roles.Single().Role);
            Assert.True(createdGroup.CreateDateTime.IsMoreThanMinValue());
        }

        [Fact]
        public async Task WhenUpdate_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync();
            var group = await _create.UserGroup.WithAccountId(account.Id).WithName("Test1").BuildAsync()
                ;

            group.Name = "Test2";
            group.IsDeleted = true;
            group.Roles.Add(new UserGroupRole {Role = Role.None});

            await _userGroupsClient.UpdateAsync(group);

            var updatedGroup = await _userGroupsClient.GetAsync(group.Id);

            Assert.Equal(group.Name, updatedGroup.Name);
            Assert.Equal(group.IsDeleted, updatedGroup.IsDeleted);
            Assert.Equal(group.Roles.Single().Role, updatedGroup.Roles.Single().Role);
        }

        [Fact]
        public async Task WhenDelete_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync();
            var groupIds = (await Task.WhenAll(
                    _create.UserGroup.WithAccountId(account.Id).WithName("Test1").BuildAsync(),
                    _create.UserGroup.WithAccountId(account.Id).WithName("Test2").BuildAsync())
                ).Select(x => x.Id).ToList();

            await _userGroupsClient.DeleteAsync(groupIds);

            var groups = await _userGroupsClient.GetListAsync(groupIds);

            Assert.All(groups, x => Assert.True(x.IsDeleted));
        }

        [Fact]
        public async Task WhenRestore_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync();
            var groupIds = (await Task.WhenAll(
                    _create.UserGroup.WithAccountId(account.Id).WithName("Test1").BuildAsync(),
                    _create.UserGroup.WithAccountId(account.Id).WithName("Test2").BuildAsync())
                ).Select(x => x.Id).ToList();

            await _userGroupsClient.RestoreAsync(groupIds);

            var groups = await _userGroupsClient.GetListAsync(groupIds);

            Assert.All(groups, x => Assert.False(x.IsDeleted));
        }
    }
}