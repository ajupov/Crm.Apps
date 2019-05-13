using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Crm.Clients.Accounts.Clients.Accounts;
using Crm.Clients.Users.Clients.UserAttributes;
using Crm.Clients.Users.Clients.UserGroups;
using Crm.Clients.Users.Clients.Users;
using Crm.Clients.Users.Clients.UsersDefault;
using Crm.Clients.Users.Clients.UserSettings;
using Crm.Clients.Users.Models;
using Crm.Utils.Guid;
using Xunit;

namespace Crm.Apps.Tests.Users
{
    public class UsersTests
    {
        private readonly IAccountsClient _accountsClient;
        private readonly IUsersDefaultClient _usersDefaultClient;
        private readonly IUsersClient _usersClient;
        private readonly IUserAttributesClient _userAttributesClient;
        private readonly IUserGroupsClient _userGroupsClient;
        private readonly IUsersSettingsClient _usersSettingsClient;

        public UsersTests(IAccountsClient accountsClient, IUsersDefaultClient usersDefaultClient,
            IUsersClient usersClient, IUserAttributesClient userAttributesClient, IUserGroupsClient userGroupsClient,
            IUsersSettingsClient usersSettingsClient)
        {
            _usersDefaultClient = usersDefaultClient;
            _usersClient = usersClient;
            _userAttributesClient = userAttributesClient;
            _userGroupsClient = userGroupsClient;
            _usersSettingsClient = usersSettingsClient;
            _accountsClient = accountsClient;
        }

        [Fact]
        public Task Status()
        {
            return _usersDefaultClient.StatusAsync();
        }

        [Fact]
        public async Task GetUserSettingsTypes()
        {
            var types = await _usersSettingsClient.GetTypesAsync().ConfigureAwait(false);

            Assert.NotEmpty(types);
        }

        [Fact]
        public async Task GetGenders()
        {
            var genders = await _usersClient.GetGendersAsync().ConfigureAwait(false);

            Assert.NotEmpty(genders);
        }

        [Fact]
        public async Task GetUser()
        {
            var accountId = await _accountsClient.CreateAsync().ConfigureAwait(false);

            var user = new User
            {
                AccountId = accountId,
                Surname = "Test",
                Name = "Test",
                Patronymic = "Test",
                BirthDate = DateTime.Today.AddYears(21),
                Gender = UserGender.Female,
                AvatarUrl = ""
            };

            var id = await _usersClient.CreateAsync(user).ConfigureAwait(false);

            var createdUser = await _usersClient.GetAsync(id).ConfigureAwait(false);

            Assert.NotNull(createdUser);
            Assert.Equal(id, user.Id);
        }

        [Fact]
        public async Task GetUsersList()
        {
            var ids = await Task.WhenAll(_usersClient.CreateAsync(new User()), _usersClient.CreateAsync(new User()))
                .ConfigureAwait(false);

            var users = await _usersClient.GetListAsync(ids).ConfigureAwait(false);

            Assert.NotEmpty(users);
            Assert.Equal(ids.Length, users.Count);
        }

        [Fact]
        public async Task GetUsersPagedList()
        {
            await Task.WhenAll(_usersClient.CreateAsync(new User()), _usersClient.CreateAsync(new User()))
                .ConfigureAwait(false);

            var users = await _usersClient
                .GetPagedListAsync(sortBy: nameof(User.CreateDateTime), orderBy: "desc")
                .ConfigureAwait(false);

            var results = users.Skip(1).Zip(users,
                (previous, current) => current.CreateDateTime >= previous.CreateDateTime);

            Assert.NotEmpty(users);
            Assert.All(results, Assert.True);
        }

        [Fact]
        public async Task CreateUser()
        {
            var id = await _usersClient.CreateAsync(new User()).ConfigureAwait(false);

            Assert.True(!id.IsEmpty());
        }

        [Fact]
        public async Task UpdateUser()
        {
            var id = await _usersClient.CreateAsync(new User()).ConfigureAwait(false);
            var user = await _usersClient.GetAsync(id).ConfigureAwait(false);

            user.IsLocked = true;
            user.IsDeleted = true;
            user.Settings = new List<UserSetting>
            {
                new UserSetting
                {
                    Type = UserSettingType.None,
                    Value = "Test"
                }
            };

            await _usersClient.UpdateAsync(user).ConfigureAwait(false);

            var updatedUser = await _usersClient.GetAsync(id).ConfigureAwait(false);

            Assert.Equal(user.IsLocked, updatedUser.IsLocked);
            Assert.Equal(user.IsDeleted, updatedUser.IsDeleted);
            Assert.Equal(user.Settings.Select(x => new {x.Type, x.Value}),
                updatedUser.Settings.Select(x => new {x.Type, x.Value}));
        }

        [Fact]
        public async Task LockUser()
        {
            var id = await _usersClient.CreateAsync(new User()).ConfigureAwait(false);

            await _usersClient.LockAsync(new List<Guid> {id}).ConfigureAwait(false);

            var lockedUser = await _usersClient.GetAsync(id).ConfigureAwait(false);

            Assert.True(lockedUser.IsLocked);
        }

        [Fact]
        public async Task UnlockUser()
        {
            var id = await _usersClient.CreateAsync(new User()).ConfigureAwait(false);

            await _usersClient.UnlockAsync(new List<Guid> {id}).ConfigureAwait(false);

            var unlockedUser = await _usersClient.GetAsync(id).ConfigureAwait(false);

            Assert.False(unlockedUser.IsLocked);
        }

        [Fact]
        public async Task DeleteUser()
        {
            var id = await _usersClient.CreateAsync(new User()).ConfigureAwait(false);

            await _usersClient.DeleteAsync(new List<Guid> {id}).ConfigureAwait(false);

            var deletedUser = await _usersClient.GetAsync(id).ConfigureAwait(false);

            Assert.True(deletedUser.IsDeleted);
        }

        [Fact]
        public async Task RestoreUser()
        {
            var id = await _usersClient.CreateAsync(new User()).ConfigureAwait(false);

            await _usersClient.RestoreAsync(new List<Guid> {id}).ConfigureAwait(false);

            var restoredUser = await _usersClient.GetAsync(id).ConfigureAwait(false);

            Assert.False(restoredUser.IsDeleted);
        }
    }
}