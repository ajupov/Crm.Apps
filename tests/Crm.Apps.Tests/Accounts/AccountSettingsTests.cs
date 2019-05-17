using System.Threading.Tasks;
using Crm.Clients.Accounts.Clients.AccountSettings;
using Xunit;

namespace Crm.Apps.Tests.Accounts
{
    public class AccountSettingsTests
    {
        private readonly IAccountSettingsClient _accountSettingsClient;

        public AccountSettingsTests(IAccountSettingsClient accountSettingsClient)
        {
            _accountSettingsClient = accountSettingsClient;
        }

        [Fact]
        public async Task WhenGetTypes_ThenSuccess()
        {
            var types = await _accountSettingsClient.GetTypesAsync().ConfigureAwait(false);

            Assert.NotEmpty(types);
        }
    }
}