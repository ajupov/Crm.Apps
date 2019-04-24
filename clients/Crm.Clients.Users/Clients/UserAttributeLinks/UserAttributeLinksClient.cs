using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Crm.Clients.Users.Models;
using Crm.Clients.Users.Settings;
using Crm.Utils.Http;
using Microsoft.Extensions.Options;

namespace Crm.Clients.Users.Clients.UserAttributeLinks
{
    public class UserAttributeLinksClient : IUserAttributeLinksClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly UsersClientSettings _settings;

        public UserAttributeLinksClient(IOptions<UsersClientSettings> options, IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _settings = options.Value;
        }

        public Task SetAsync(Guid userId, Guid attributeId, string value = null, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync($"{_settings.Host}/Api/UserAttributeLinks/Set", new UserAttributeLink
            {
                UserId = userId,
                AttributeId = attributeId,
                Value = value
            }, ct);
        }

        public Task ResetAsync(Guid userId, Guid attributeId, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync($"{_settings.Host}/Api/UserAttributeLinks/Reset", new UserAttributeLink
            {
                UserId = userId,
                AttributeId = attributeId
            }, ct);
        }
    }
}