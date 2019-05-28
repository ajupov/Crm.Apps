using System.Linq;
using System.Threading.Tasks;
using Crm.Apps.Tests.Dsl.Creator;
using Crm.Clients.Users.Clients;
using Crm.Clients.Users.Models;
using Crm.Utils.DateTime;
using Crm.Utils.Guid;
using Crm.Utils.Json;
using Crm.Utils.String;
using Xunit;

namespace Crm.Apps.Tests.Tests.Users
{
    public class UserChangesTests
    {
        private readonly ICreate _create;
        private readonly IUsersClient _usersClient;
        private readonly IUserChangesClient _userChangesClient;

        public UserChangesTests(ICreate create, IUsersClient usersClient, IUserChangesClient userChangesClient)
        {
            _create = create;
            _usersClient = usersClient;
            _userChangesClient = userChangesClient;
        }

        [Fact]
        public async Task WhenGetPagedList_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync().ConfigureAwait(false);
            var user = await _create.User.WithAccountId(account.Id).BuildAsync().ConfigureAwait(false);
            user.IsLocked = true;
            await _usersClient.UpdateAsync(user).ConfigureAwait(false);

            var changes = await _userChangesClient
                .GetPagedListAsync(userId: user.Id, sortBy: "CreateDateTime", orderBy: "asc")
                .ConfigureAwait(false);

            Assert.NotEmpty(changes);
            Assert.True(changes.All(x => !x.ChangerUserId.IsEmpty()));
            Assert.True(changes.All(x => x.UserId == user.Id));
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