using System.Threading.Tasks;
using Crm.Clients.Users.Clients;
using Xunit;

namespace Crm.Apps.Tests.Tests.Users
{
    public class UserSettingsTests
    {
        private readonly IUserSettingsClient _userSettingsClient;

        public UserSettingsTests(IUserSettingsClient userSettingsClient)
        {
            _userSettingsClient = userSettingsClient;
        }

        [Fact]
        public async Task WhenGetTypes_ThenSuccess()
        {
            var types = await _userSettingsClient.GetTypesAsync().ConfigureAwait(false);

            Assert.NotEmpty(types);
        }
    }
}