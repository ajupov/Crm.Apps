using System;
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
    public class UsersTests
    {
        private readonly ICreate _create;
        private readonly IUsersClient _usersClient;

        public UsersTests(ICreate create, IUsersClient usersClient)
        {
            _create = create;
            _usersClient = usersClient;
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
            var account = await _create.Account.BuildAsync().ConfigureAwait(false);
            var userId = (await _create.User.WithAccountId(account.Id).BuildAsync().ConfigureAwait(false)).Id;

            var user = await _usersClient.GetAsync(userId).ConfigureAwait(false);

            Assert.NotNull(user);
            Assert.Equal(userId, user.Id);
        }

        [Fact]
        public async Task WhenGetList_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync().ConfigureAwait(false);
            var userIds = (await Task.WhenAll(_create.User.WithAccountId(account.Id).BuildAsync(),
                _create.User.WithAccountId(account.Id).BuildAsync()).ConfigureAwait(false)).Select(x => x.Id).ToList();

            var users = await _usersClient.GetListAsync(userIds).ConfigureAwait(false);

            Assert.NotEmpty(users);
            Assert.Equal(userIds.Count, users.Count);
        }

        [Fact]
        public async Task WhenGetPagedList_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync().ConfigureAwait(false);
            var attribute = await _create.UserAttribute.WithAccountId(account.Id).BuildAsync().ConfigureAwait(false);
            var group = await _create.UserGroup.WithAccountId(account.Id).WithName("Test").BuildAsync()
                .ConfigureAwait(false);
            await Task.WhenAll(
                    _create.User.WithAccountId(account.Id).WithAttributeLink(attribute.Id, "Test").BuildAsync(),
                    _create.User.WithAccountId(account.Id).WithAttributeLink(attribute.Id, "Test").BuildAsync())
                .ConfigureAwait(false);
            var filterAttributes = new Dictionary<Guid, string> {{attribute.Id, "Test"}};
            var filterGroupIds = new List<Guid> {group.Id};

            var users = await _usersClient.GetPagedListAsync(account.Id, sortBy: "CreateDateTime", orderBy: "desc",
                    allAttributes: false, attributes: filterAttributes, allGroupIds: true, groupIds: filterGroupIds)
                .ConfigureAwait(false);

            var results = users.Skip(1)
                .Zip(users, (previous, current) => current.CreateDateTime >= previous.CreateDateTime);

            Assert.NotEmpty(users);
            Assert.All(results, Assert.True);
        }

        [Fact]
        public async Task WhenCreate_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync().ConfigureAwait(false);
            var attribute = await _create.UserAttribute.WithAccountId(account.Id).BuildAsync().ConfigureAwait(false);
            var group = await _create.UserGroup.WithAccountId(account.Id).BuildAsync().ConfigureAwait(false);
            var user = new User
            {
                AccountId = account.Id,
                Surname = "Test",
                Name = "Test",
                Patronymic = "Test",
                BirthDate = DateTime.Today.AddYears(21),
                Gender = UserGender.Male,
                AvatarUrl = "test.com",
                IsLocked = true,
                IsDeleted = true,
                AttributeLinks = new List<UserAttributeLink>
                {
                    new UserAttributeLink
                    {
                        UserAttributeId = attribute.Id,
                        Value = "Test"
                    }
                },
                Permissions = new List<UserPermission>
                {
                    new UserPermission
                    {
                        Permission = Permission.Administration
                    }
                },
                GroupLinks = new List<UserGroupLink>
                {
                    new UserGroupLink
                    {
                        UserGroupId = group.Id
                    }
                },
                Settings = new List<UserSetting>
                {
                    new UserSetting
                    {
                        Type = UserSettingType.None,
                        Value = "Test"
                    }
                }
            };

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
            var account = await _create.Account.BuildAsync().ConfigureAwait(false);
            var user = await _create.User.WithAccountId(account.Id).BuildAsync().ConfigureAwait(false);
            var attribute = await _create.UserAttribute.WithAccountId(account.Id).BuildAsync().ConfigureAwait(false);
            var group = await _create.UserGroup.WithAccountId(account.Id).WithPermission(Permission.TechnicalSupport)
                .BuildAsync().ConfigureAwait(false);

            user.Surname = "Test2";
            user.Name = "Test2";
            user.Patronymic = "Test2";
            user.BirthDate = DateTime.Today.AddYears(22);
            user.Gender = UserGender.Female;
            user.AvatarUrl = "test2.com";
            user.IsLocked = true;
            user.IsDeleted = true;
            user.Settings.Add(new UserSetting {Type = UserSettingType.None, Value = "Test"});
            user.Permissions.Add(new UserPermission {Permission = Permission.Administration});
            user.AttributeLinks.Add(new UserAttributeLink {UserAttributeId = attribute.Id, Value = "Test"});
            user.GroupLinks.Add(new UserGroupLink {UserGroupId = group.Id});
            await _usersClient.UpdateAsync(user).ConfigureAwait(false);

            var updatedUser = await _usersClient.GetAsync(user.Id).ConfigureAwait(false);

            Assert.Equal(user.Surname, updatedUser.Surname);
            Assert.Equal(user.Name, updatedUser.Name);
            Assert.Equal(user.Patronymic, updatedUser.Patronymic);
            Assert.Equal(user.BirthDate, updatedUser.BirthDate);
            Assert.Equal(user.Gender, updatedUser.Gender);
            Assert.Equal(user.AvatarUrl, updatedUser.AvatarUrl);
            Assert.Equal(user.IsLocked, updatedUser.IsLocked);
            Assert.Equal(user.IsDeleted, updatedUser.IsDeleted);
            Assert.Equal(user.Settings.Single().Type, updatedUser.Settings.Single().Type);
            Assert.Equal(user.Settings.Single().Value, updatedUser.Settings.Single().Value);
            Assert.Equal(user.Permissions.Single().Permission, updatedUser.Permissions.Single().Permission);
            Assert.Equal(user.AttributeLinks.Single().UserAttributeId, updatedUser.AttributeLinks.Single().UserAttributeId);
            Assert.Equal(user.AttributeLinks.Single().Value, updatedUser.AttributeLinks.Single().Value);
            Assert.Equal(user.GroupLinks.Single().UserGroupId, updatedUser.GroupLinks.Single().UserGroupId);
        }

        [Fact]
        public async Task WhenLock_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync().ConfigureAwait(false);
            var userIds = (await Task.WhenAll(_create.User.WithAccountId(account.Id).BuildAsync(),
                _create.User.WithAccountId(account.Id).BuildAsync()).ConfigureAwait(false)).Select(x => x.Id).ToList();

            await _usersClient.LockAsync(userIds).ConfigureAwait(false);

            var users = await _usersClient.GetListAsync(userIds).ConfigureAwait(false);

            Assert.All(users, x => Assert.True(x.IsLocked));
        }

        [Fact]
        public async Task WhenUnlock_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync().ConfigureAwait(false);
            var userIds = (await Task.WhenAll(_create.User.WithAccountId(account.Id).BuildAsync(),
                _create.User.WithAccountId(account.Id).BuildAsync()).ConfigureAwait(false)).Select(x => x.Id).ToList();

            await _usersClient.UnlockAsync(userIds).ConfigureAwait(false);

            var users = await _usersClient.GetListAsync(userIds).ConfigureAwait(false);

            Assert.All(users, x => Assert.False(x.IsLocked));
        }

        [Fact]
        public async Task WhenDelete_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync().ConfigureAwait(false);
            var userIds = (await Task.WhenAll(_create.User.WithAccountId(account.Id).BuildAsync(),
                _create.User.WithAccountId(account.Id).BuildAsync()).ConfigureAwait(false)).Select(x => x.Id).ToList();

            await _usersClient.DeleteAsync(userIds).ConfigureAwait(false);

            var users = await _usersClient.GetListAsync(userIds).ConfigureAwait(false);

            Assert.All(users, x => Assert.True(x.IsDeleted));
        }

        [Fact]
        public async Task WhenRestore_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync().ConfigureAwait(false);
            var userIds = (await Task.WhenAll(_create.User.WithAccountId(account.Id).BuildAsync(),
                _create.User.WithAccountId(account.Id).BuildAsync()).ConfigureAwait(false)).Select(x => x.Id).ToList();

            await _usersClient.LockAsync(userIds).ConfigureAwait(false);

            var users = await _usersClient.GetListAsync(userIds).ConfigureAwait(false);

            Assert.All(users, x => Assert.False(x.IsDeleted));
        }
    }
}