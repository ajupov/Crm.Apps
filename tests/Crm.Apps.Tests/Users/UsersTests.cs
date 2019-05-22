using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Crm.Apps.Tests.Dsl;
using Crm.Clients.Accounts.Clients.Accounts;
using Crm.Clients.Accounts.Models;
using Crm.Clients.Users.Clients.UserAttributes;
using Crm.Clients.Users.Clients.UserGroups;
using Crm.Clients.Users.Clients.Users;
using Crm.Clients.Users.Models;
using Crm.Common.Types;
using Crm.Common.UserContext;
using Crm.Utils.DateTime;
using Xunit;

namespace Crm.Apps.Tests.Users
{
    public class UsersTests
    {
        private readonly IAccountsClient _accountsClient;
        private readonly IUsersClient _usersClient;
        private readonly IUserAttributesClient _userAttributesClient;
        private readonly IUserGroupsClient _userGroupsClient;

        public UsersTests(IAccountsClient accountsClient, IUsersClient usersClient,
            IUserAttributesClient userAttributesClient, IUserGroupsClient userGroupsClient)
        {
            _usersClient = usersClient;
            _userAttributesClient = userAttributesClient;
            _userGroupsClient = userGroupsClient;
            _accountsClient = accountsClient;
        }

        [Fact]
        public async Task WhenGetGenders_ThenSuccess()
        {
            var genders = await _usersClient.GetGendersAsync().ConfigureAwait(false);

            Assert.NotEmpty(genders);
        }

        [Fact]
        public async Task WhenGet_ThenSuccess()
        {
            var account = Create.Account().Build();
            var createdAccountId = await _accountsClient.CreateAsync(account).ConfigureAwait(false);

            var user = Create.User(createdAccountId).Build();
            var createdUserId = await _usersClient.CreateAsync(user).ConfigureAwait(false);
            var createdUser = await _usersClient.GetAsync(createdUserId).ConfigureAwait(false);

            Assert.NotNull(createdUser);
            Assert.Equal(createdUserId, createdUser.Id);
        }

        [Fact]
        public async Task WhenGetList_ThenSuccess()
        {
            var account = Create.Account().Build();
            var createdAccountId = await _accountsClient.CreateAsync(account).ConfigureAwait(false);

            var createdUserIds = await Task.WhenAll(
                    _usersClient.CreateAsync(Create.User(createdAccountId).Build()),
                    _usersClient.CreateAsync(Create.User(createdAccountId).Build()))
                .ConfigureAwait(false);

            var createdUsers = await _usersClient.GetListAsync(createdUserIds).ConfigureAwait(false);

            Assert.NotEmpty(createdUsers);
            Assert.Equal(createdUserIds.Length, createdUsers.Count);
        }

        [Fact]
        public async Task WhenGetPagedList_ThenSuccess()
        {
            var account = Create.Account().Build();
            var createdAccountId = await _accountsClient.CreateAsync(account).ConfigureAwait(false);

            var userAttribute = Create.UserAttribute(createdAccountId).Build();
            var createdAttributeId = await _userAttributesClient.CreateAsync(userAttribute).ConfigureAwait(false);

            var user1 = Create.User(createdAccountId).WithAttributeLink(createdAttributeId, "test").Build();
            var user2 = Create.User(createdAccountId).WithAttributeLink(createdAttributeId, "test").Build();

            await Task.WhenAll(_usersClient.CreateAsync(user1), _usersClient.CreateAsync(user2)).ConfigureAwait(false);

            var filterAttributes = new Dictionary<Guid, string>
            {
                {createdAttributeId, "test"}
            };

            var anyUsers = await _usersClient.GetPagedListAsync(createdAccountId, sortBy: "CreateDateTime",
                orderBy: "desc", allAttributes: false, attributes: filterAttributes).ConfigureAwait(false);

            var results = anyUsers.Skip(1)
                .Zip(anyUsers, (previous, current) => current.CreateDateTime >= previous.CreateDateTime);

            Assert.NotEmpty(anyUsers);
            Assert.All(results, Assert.True);
        }

        [Fact]
        public async Task WhenCreate_ThenSuccess()
        {
            var account = Create.Account().Build();
            var createdAccountId = await _accountsClient.CreateAsync(account).ConfigureAwait(false);

            var userAttribute = Create.UserAttribute(createdAccountId).Build();
            var createdAttributeId = await _userAttributesClient.CreateAsync(userAttribute).ConfigureAwait(false);

            var userGroup = Create.UserGroup(createdAccountId).WithPermission(Permission.TechnicalSupport).Build();
            var createdGroupId = await _userGroupsClient.CreateAsync(userGroup).ConfigureAwait(false);

            var user = Create.User(createdAccountId).WithSurname("Test").WithName("Test").WithPatronymic("Test")
                .WithBirthDate(DateTime.Today.AddYears(21)).WithGender(UserGender.Male).WithAvatarUrl("test.com")
                .AsLocked().AsDeleted().WithAttributeLink(createdAttributeId, "test")
                .WithPermission(Permission.Administration).WithGroupLink(createdGroupId).WithSetting("test").Build();

            var createdUserId = await _usersClient.CreateAsync(user).ConfigureAwait(false);
            var createdUser = await _usersClient.GetAsync(createdUserId).ConfigureAwait(false);

            Assert.NotNull(createdUser);
            Assert.Equal(createdUserId, createdUser.Id);
            Assert.Equal(user.AccountId, createdUser.AccountId);
            Assert.Equal(user.Surname, createdUser.Surname);
            Assert.Equal(user.Name, createdUser.Name);
            Assert.Equal(user.Patronymic, createdUser.Patronymic);
            Assert.Equal(user.BirthDate, createdUser.BirthDate);
            Assert.Equal(user.Gender, createdUser.Gender);
            Assert.Equal(user.AvatarUrl, createdUser.AvatarUrl);
            Assert.Equal(user.IsLocked, createdUser.IsLocked);
            Assert.Equal(user.IsDeleted, createdUser.IsDeleted);
            Assert.True(createdUser.CreateDateTime.IsMoreThanMinValue());
            Assert.NotEmpty(createdUser.AttributeLinks);
            Assert.NotEmpty(createdUser.Permissions);
            Assert.NotEmpty(createdUser.GroupLinks);
            Assert.NotEmpty(createdUser.Settings);
        }

        [Fact]
        public async Task WhenUpdate_ThenSuccess()
        {
            var account = Create.Account().Build();
            var createdAccountId = await _accountsClient.CreateAsync(account).ConfigureAwait(false);

            var user = Create.User(createdAccountId).Build();
            var createdUserId = await _usersClient.CreateAsync(user).ConfigureAwait(false);
            var createdUser = await _usersClient.GetAsync(createdUserId).ConfigureAwait(false);

            var userAttribute = Create.UserAttribute(createdAccountId).Build();
            var createdAttributeId = await _userAttributesClient.CreateAsync(userAttribute).ConfigureAwait(false);

            var userGroup = Create.UserGroup(createdAccountId).WithPermission(Permission.TechnicalSupport).Build();
            var createdGroupId = await _userGroupsClient.CreateAsync(userGroup).ConfigureAwait(false);

            createdUser.Surname = "Test2";
            createdUser.Name = "Test2";
            createdUser.Patronymic = "Test2";
            createdUser.BirthDate = DateTime.Today.AddYears(22);
            createdUser.Gender = UserGender.Female;
            createdUser.AvatarUrl = "test2.com";
            createdUser.IsLocked = true;
            createdUser.IsDeleted = true;
            createdUser.Settings.Add(new UserSetting {Type = UserSettingType.None, Value = "Test"});
            createdUser.Permissions.Add(new UserPermission {Permission = Permission.Administration});
            createdUser.AttributeLinks.Add(new UserAttributeLink {AttributeId = createdAttributeId, Value = "test"});
            createdUser.GroupLinks.Add(new UserGroupLink {GroupId = createdGroupId});

            await _usersClient.UpdateAsync(createdUser).ConfigureAwait(false);
            var updatedUser = await _usersClient.GetAsync(createdUserId).ConfigureAwait(false);

            Assert.Equal(createdUser.Surname, updatedUser.Surname);
            Assert.Equal(createdUser.Name, updatedUser.Name);
            Assert.Equal(createdUser.Patronymic, updatedUser.Patronymic);
            Assert.Equal(createdUser.BirthDate, updatedUser.BirthDate);
            Assert.Equal(createdUser.Gender, updatedUser.Gender);
            Assert.Equal(createdUser.AvatarUrl, updatedUser.AvatarUrl);
            Assert.Equal(createdUser.IsLocked, updatedUser.IsLocked);
            Assert.Equal(createdUser.IsDeleted, updatedUser.IsDeleted);
            Assert.Equal(createdUser.Settings.Single().Type, updatedUser.Settings.Single().Type);
            Assert.Equal(createdUser.Settings.Single().Value, updatedUser.Settings.Single().Value);
            Assert.Equal(createdUser.Permissions.Single().Permission, updatedUser.Permissions.Single().Permission);
            Assert.Equal(createdUser.AttributeLinks.Single().AttributeId,
                updatedUser.AttributeLinks.Single().AttributeId);
            Assert.Equal(createdUser.AttributeLinks.Single().Value, updatedUser.AttributeLinks.Single().Value);
            Assert.Equal(createdUser.GroupLinks.Single().GroupId, updatedUser.GroupLinks.Single().GroupId);
        }

        [Fact]
        public async Task WhenLock_ThenSuccess()
        {
            var account = Create.Account().Build();
            var createdAccountId = await _accountsClient.CreateAsync(account).ConfigureAwait(false);

            var createdUserIds = await Task.WhenAll(
                    _usersClient.CreateAsync(Create.User(createdAccountId).Build()),
                    _usersClient.CreateAsync(Create.User(createdAccountId).Build()))
                .ConfigureAwait(false);

            await _usersClient.LockAsync(createdUserIds).ConfigureAwait(false);
            var lockedUsers = await _usersClient.GetListAsync(createdUserIds).ConfigureAwait(false);

            Assert.All(lockedUsers, x => Assert.True(x.IsLocked));
        }

        [Fact]
        public async Task WhenUnlock_ThenSuccess()
        {
            var account = Create.Account().Build();
            var createdAccountId = await _accountsClient.CreateAsync(account).ConfigureAwait(false);

            var createdUserIds = await Task.WhenAll(
                    _usersClient.CreateAsync(Create.User(createdAccountId).Build()),
                    _usersClient.CreateAsync(Create.User(createdAccountId).Build()))
                .ConfigureAwait(false);

            await _usersClient.UnlockAsync(createdUserIds).ConfigureAwait(false);
            var unLockedUsers = await _usersClient.GetListAsync(createdUserIds).ConfigureAwait(false);

            Assert.All(unLockedUsers, x => Assert.False(x.IsLocked));
        }

        [Fact]
        public async Task WhenDelete_ThenSuccess()
        {
            var account = Create.Account().Build();
            var createdAccountId = await _accountsClient.CreateAsync(account).ConfigureAwait(false);

            var createdUserIds = await Task.WhenAll(
                    _usersClient.CreateAsync(Create.User(createdAccountId).Build()),
                    _usersClient.CreateAsync(Create.User(createdAccountId).Build()))
                .ConfigureAwait(false);

            await _usersClient.DeleteAsync(createdUserIds).ConfigureAwait(false);
            var deletedUsers = await _usersClient.GetListAsync(createdUserIds).ConfigureAwait(false);

            Assert.All(deletedUsers, x => Assert.True(x.IsDeleted));
        }

        [Fact]
        public async Task WhenRestore_ThenSuccess()
        {
            var account = Create.Account().Build();
            var createdAccountId = await _accountsClient.CreateAsync(account).ConfigureAwait(false);

            var createdUserIds = await Task.WhenAll(
                    _usersClient.CreateAsync(Create.User(createdAccountId).Build()),
                    _usersClient.CreateAsync(Create.User(createdAccountId).Build()))
                .ConfigureAwait(false);

            await _usersClient.LockAsync(createdUserIds).ConfigureAwait(false);
            var restoredUsers = await _usersClient.GetListAsync(createdUserIds).ConfigureAwait(false);

            Assert.All(restoredUsers, x => Assert.False(x.IsDeleted));
        }
    }
}