using System;
using System.Linq;
using System.Threading.Tasks;
using Crm.Clients.Accounts.Clients.Accounts;
using Crm.Clients.Users.Clients.Users;
using Crm.Clients.Users.Models;
using Crm.Common.UserContext;
using Crm.Utils.DateTime;
using Xunit;

namespace Crm.Apps.Tests.Users
{
    public class UsersTests
    {
        private readonly IAccountsClient _accountsClient;
        private readonly IUsersClient _usersClient;

        public UsersTests(IAccountsClient accountsClient, IUsersClient usersClient)
        {
            _usersClient = usersClient;
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
            var accountId = await _accountsClient.CreateAsync().ConfigureAwait(false);
            var id = await _usersClient.CreateAsync(CreateUser(accountId)).ConfigureAwait(false);

            var createdUser = await _usersClient.GetAsync(id).ConfigureAwait(false);

            Assert.NotNull(createdUser);
            Assert.Equal(id, createdUser.Id);
        }

        [Fact]
        public async Task WhenGetList_ThenSuccess()
        {
            var accountId = await _accountsClient.CreateAsync().ConfigureAwait(false);
            var ids = await Task.WhenAll(
                    _usersClient.CreateAsync(CreateUser(accountId)),
                    _usersClient.CreateAsync(CreateUser(accountId)))
                .ConfigureAwait(false);

            var users = await _usersClient.GetListAsync(ids).ConfigureAwait(false);

            Assert.NotEmpty(users);
            Assert.Equal(ids.Length, users.Count);
        }

        [Fact]
        public async Task WhenGetPagedList_ThenSuccess()
        {
            var accountId = await _accountsClient.CreateAsync().ConfigureAwait(false);
            await Task.WhenAll(
                    _usersClient.CreateAsync(CreateUser(accountId)),
                    _usersClient.CreateAsync(CreateUser(accountId)))
                .ConfigureAwait(false);

            var users = await _usersClient
                .GetPagedListAsync(accountId, sortBy: nameof(User.CreateDateTime), orderBy: "desc")
                .ConfigureAwait(false);

            var results = users.Skip(1)
                .Zip(users, (previous, current) => current.CreateDateTime >= previous.CreateDateTime);

            Assert.NotEmpty(users);
            Assert.All(results, Assert.True);
        }

        [Fact]
        public async Task WhenCreate_ThenSuccess()
        {
            var accountId = await _accountsClient.CreateAsync().ConfigureAwait(false);

            var user = new User
            {
                AccountId = accountId,
                Surname = "Test",
                Name = "Test",
                Patronymic = "Test",
                BirthDate = DateTime.Today.AddYears(21),
                Gender = UserGender.Male,
                AvatarUrl = "test.com",
                IsLocked = false,
                IsDeleted = false
            };

            var id = await _usersClient.CreateAsync(user).ConfigureAwait(false);
            var createdUser = await _usersClient.GetAsync(id).ConfigureAwait(false);

            Assert.NotNull(createdUser);
            Assert.Equal(id, createdUser.Id);
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
            Assert.Empty(createdUser.AttributeLinks);
            Assert.Empty(createdUser.Permissions);
            Assert.Empty(createdUser.GroupLinks);
            Assert.Empty(createdUser.Settings);
        }

        [Fact]
        public async Task WhenUpdate_ThenSuccess()
        {
            var accountId = await _accountsClient.CreateAsync().ConfigureAwait(false);
            var id = await _usersClient.CreateAsync(CreateUser(accountId)).ConfigureAwait(false);
            var createdUser = await _usersClient.GetAsync(id).ConfigureAwait(false);

            createdUser.Surname = "Test2";
            createdUser.Name = "Test2";
            createdUser.Patronymic = "Test2";
            createdUser.BirthDate = DateTime.Today.AddYears(22);
            createdUser.Gender = UserGender.Female;
            createdUser.AvatarUrl = "test2.com";
            createdUser.IsLocked = true;
            createdUser.IsDeleted = true;
            createdUser.Settings.Add(new UserSetting {Type = UserSettingType.None, Value = "Test"});
            var createdUserSettings = createdUser.Settings.Select(x => new {x.Type, x.Value});
            createdUser.Permissions.Add(new UserPermission{Permission = Permission.Administration});
            var createdUserPermissions = createdUser.Permissions.Select(x => x.Permission);

            await _usersClient.UpdateAsync(createdUser).ConfigureAwait(false);

            var updatedUser = await _usersClient.GetAsync(id).ConfigureAwait(false);
            var updatedUserSettings = updatedUser.Settings.Select(x => new {x.Type, x.Value});
            var updatedUserPermissions = updatedUser.Permissions.Select(x => x.Permission);

            Assert.Equal(createdUser.Surname, updatedUser.Surname);
            Assert.Equal(createdUser.Name, updatedUser.Name);
            Assert.Equal(createdUser.Patronymic, updatedUser.Patronymic);
            Assert.Equal(createdUser.BirthDate, updatedUser.BirthDate);
            Assert.Equal(createdUser.Gender, updatedUser.Gender);
            Assert.Equal(createdUser.AvatarUrl, updatedUser.AvatarUrl);
            Assert.Equal(createdUser.IsLocked, updatedUser.IsLocked);
            Assert.Equal(createdUser.IsDeleted, updatedUser.IsDeleted);
            Assert.Equal(createdUserSettings, updatedUserSettings);
            Assert.Equal(createdUserPermissions, updatedUserPermissions);
        }

        [Fact]
        public async Task WhenLock_ThenSuccess()
        {
            var accountId = await _accountsClient.CreateAsync().ConfigureAwait(false);

            var ids = await Task.WhenAll(
                    _usersClient.CreateAsync(CreateUser(accountId)),
                    _usersClient.CreateAsync(CreateUser(accountId)))
                .ConfigureAwait(false);

            await _usersClient.LockAsync(ids).ConfigureAwait(false);

            var lockedUsers = await _usersClient.GetListAsync(ids).ConfigureAwait(false);

            Assert.True(lockedUsers.All(x => x.IsLocked));
        }

        [Fact]
        public async Task WhenUnlock_ThenSuccess()
        {
            var accountId = await _accountsClient.CreateAsync().ConfigureAwait(false);

            var ids = await Task.WhenAll(
                    _usersClient.CreateAsync(CreateUser(accountId)),
                    _usersClient.CreateAsync(CreateUser(accountId)))
                .ConfigureAwait(false);

            await _usersClient.UnlockAsync(ids).ConfigureAwait(false);

            var unLockedUsers = await _usersClient.GetListAsync(ids).ConfigureAwait(false);

            Assert.False(unLockedUsers.All(x => x.IsLocked));
        }

        [Fact]
        public async Task WhenDelete_ThenSuccess()
        {
            var accountId = await _accountsClient.CreateAsync().ConfigureAwait(false);

            var ids = await Task.WhenAll(
                    _usersClient.CreateAsync(CreateUser(accountId)),
                    _usersClient.CreateAsync(CreateUser(accountId)))
                .ConfigureAwait(false);

            await _usersClient.DeleteAsync(ids).ConfigureAwait(false);

            var deletedUsers = await _usersClient.GetListAsync(ids).ConfigureAwait(false);

            Assert.True(deletedUsers.All(x => x.IsDeleted));
        }

        [Fact]
        public async Task WhenRestore_ThenSuccess()
        {
            var accountId = await _accountsClient.CreateAsync().ConfigureAwait(false);

            var ids = await Task.WhenAll(
                    _usersClient.CreateAsync(CreateUser(accountId)),
                    _usersClient.CreateAsync(CreateUser(accountId)))
                .ConfigureAwait(false);

            await _usersClient.RestoreAsync(ids).ConfigureAwait(false);

            var restoredUsers = await _usersClient.GetListAsync(ids).ConfigureAwait(false);

            Assert.False(restoredUsers.All(x => x.IsDeleted));
        }

        private static User CreateUser(Guid accountId)
        {
            return new User
            {
                AccountId = accountId,
                Surname = "Test",
                Name = "Test",
                Patronymic = "Test",
                BirthDate = DateTime.Today.AddYears(21),
                Gender = UserGender.Male,
                AvatarUrl = ""
            };
        }
    }
}