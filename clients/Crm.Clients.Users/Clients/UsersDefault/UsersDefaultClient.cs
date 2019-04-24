using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Crm.Clients.Users.Settings;
using Crm.Utils.Http;
using Microsoft.Extensions.Options;

namespace Crm.Clients.Users.Clients.UsersDefault
{
    public class UsersDefaultClient : IUsersDefaultClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly UsersClientSettings _settings;

        public UsersDefaultClient(IOptions<UsersClientSettings> options, IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _settings = options.Value;
        }


        public Task StatusAsync(CancellationToken ct = default)
        {
            return _httpClientFactory.GetAsync(_settings.Host, ct: ct);
        }
    }
}