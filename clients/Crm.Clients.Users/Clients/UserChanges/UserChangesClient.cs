using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Crm.Clients.Users.Models;
using Crm.Clients.Users.Settings;
using Crm.Utils.Http;
using Microsoft.Extensions.Options;

namespace Crm.Clients.Users.Clients.UserChanges
{
    public class UserChangesClient : IUserChangesClient
    {
        private readonly UsersClientSettings _settings;
        private readonly IHttpClientFactory _httpClientFactory;

        public UserChangesClient(IHttpClientFactory httpClientFactory, IOptions<UsersClientSettings> options)
        {
            _httpClientFactory = httpClientFactory;
            _settings = options.Value;
        }

        public Task<List<UserChange>> GetPagedListAsync(Guid? changerUserId = default, Guid? userId = default,
            DateTime? minCreateDate = default, DateTime? maxCreateDate = default, int offset = default,
            int limit = 10, string sortBy = default, string orderBy = default, CancellationToken ct = default)
        {
            var parameter = new
            {
                ChangerUserId = changerUserId,
                UserId = userId,
                MinCreateDate = minCreateDate,
                MaxCreateDate = maxCreateDate,
                Offset = offset,
                Limit = limit,
                SortBy = sortBy,
                OrderBy = orderBy
            };

            return _httpClientFactory.PostAsync<List<UserChange>>($"{_settings.Host}/Api/Users/Changes/GetPagedList",
                parameter, ct);
        }
    }
}