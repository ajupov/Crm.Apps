using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Crm.Clients.Users.Clients.UserAttributeLinks;
using Crm.Clients.Users.Clients.UserAttributes;
using Crm.Clients.Users.Clients.UserGroupLinks;
using Crm.Clients.Users.Clients.UserGroups;
using Crm.Clients.Users.Clients.UserPostLinks;
using Crm.Clients.Users.Clients.UserPosts;
using Crm.Clients.Users.Clients.Users;
using Crm.Clients.Users.Clients.UsersDefault;
using Crm.Clients.Users.Clients.UserSettings;
using Crm.Clients.Users.Models;
using Xunit;

namespace Crm.Apps.Tests.Users
{
    public class UsersTests
    {
        private readonly IUsersDefaultClient _usersDefaultClient;
        private readonly IUsersClient _usersClient;
        private readonly IUserAttributesClient _userAttributesClient;
        private readonly IUserAttributeLinksClient _userAttributeLinksClient;
        private readonly IUserGroupsClient _userGroupsClient;
        private readonly IUserGroupLinksClient _userGroupLinksClient;
        private readonly IUserPostsClient _userPostsClient;
        private readonly IUserPostLinksClient _userPostLinksClient;
        private readonly IUsersSettingsClient _usersSettingsClient;

        public UsersTests(IUsersDefaultClient usersDefaultClient, IUsersClient usersClient,
            IUserAttributesClient userAttributesClient, IUserAttributeLinksClient userAttributeLinksClient,
            IUserGroupsClient userGroupsClient, IUserGroupLinksClient userGroupLinksClient,
            IUserPostsClient userPostsClient, IUserPostLinksClient userPostLinksClient,
            IUsersSettingsClient usersSettingsClient)
        {
            _usersDefaultClient = usersDefaultClient;
            _usersClient = usersClient;
            _userAttributesClient = userAttributesClient;
            _userAttributeLinksClient = userAttributeLinksClient;
            _userGroupsClient = userGroupsClient;
            _userGroupLinksClient = userGroupLinksClient;
            _userPostsClient = userPostsClient;
            _userPostLinksClient = userPostLinksClient;
            _usersSettingsClient = usersSettingsClient;
        }

        [Fact]
        public Task Status()
        {
            return _usersDefaultClient.StatusAsync();
        }
        
        [Fact]
        public async Task GetGenders()
        {
            var genders = await _usersClient.GetGendersAsync().ConfigureAwait(false);

            Assert.NotEmpty(genders);
        }

        [Fact]
        public async Task GetUserSettingsTypes()
        {
            var types = await _usersSettingsClient.GetTypesAsync().ConfigureAwait(false);

            Assert.NotEmpty(types);
        }

        [Fact]
        public async Task GetUser()
        {
            var id = await _usersClient.CreateAsync(new User()).ConfigureAwait(false);

            var user = await _usersClient.GetAsync(id).ConfigureAwait(false);

            Assert.NotNull(user);
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

            Assert.NotEqual(id, Guid.Empty);
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