using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Clients.Contacts.Models;
using Crm.Apps.Clients.Contacts.Settings;
using Crm.Common.All.Types.AttributeType;
using Microsoft.Extensions.Options;

namespace Crm.Apps.Clients.Contacts.Clients
{
    public class ContactAttributesClient : IContactAttributesClient
    {
        private readonly ContactsClientSettings _settings;
        private readonly IHttpClientFactory _httpClientFactory;

        public ContactAttributesClient(IOptions<ContactsClientSettings> options, IHttpClientFactory httpClientFactory)
        {
            _url = UriBuilder.Combine(options.Value.Host, );
            _httpClientFactory = httpClientFactory;
        }

        public Task<List<AttributeType>> GetTypesAsync(CancellationToken ct = default)
        {
            return _httpClientFactory.GetAsync<List<AttributeType>>(
                $"{_settings.Host}/Api/Contacts/Attributes/GetTypes", ct: ct);
        }

        public Task<ContactAttribute> GetAsync(Guid id, CancellationToken ct = default)
        {
            return _httpClientFactory.GetAsync<ContactAttribute>($"{_settings.Host}/Api/Contacts/Attributes/Get",
                new {id}, ct);
        }

        public Task<List<ContactAttribute>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync<List<ContactAttribute>>(
                $"{_settings.Host}/Api/Contacts/Attributes/GetList", ids, ct);
        }

        public Task<List<ContactAttribute>> GetPagedListAsync(Guid? accountId = default,
            List<AttributeType> types = default, string key = default, bool? isDeleted = default,
            DateTime? minCreateDate = default, DateTime? maxCreateDate = default, int offset = default, int limit = 10,
            string sortBy = default, string orderBy = default, CancellationToken ct = default)
        {
            var parameter = new
            {
                AccountId = accountId,
                Types = types,
                Key = key,
                IsDeleted = isDeleted,
                MinCreateDate = minCreateDate,
                MaxCreateDate = maxCreateDate,
                Offset = offset,
                Limit = limit,
                SortBy = sortBy,
                OrderBy = orderBy
            };

            return _httpClientFactory.PostAsync<List<ContactAttribute>>(
                $"{_settings.Host}/Api/Contacts/Attributes/GetPagedList", parameter, ct);
        }

        public Task<Guid> CreateAsync(ContactAttribute attribute, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync<Guid>($"{_settings.Host}/Api/Contacts/Attributes/Create", attribute,
                ct);
        }

        public Task UpdateAsync(ContactAttribute attribute, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync($"{_settings.Host}/Api/Contacts/Attributes/Update", attribute, ct);
        }

        public Task DeleteAsync(IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync($"{_settings.Host}/Api/Contacts/Attributes/Delete", ids, ct);
        }

        public Task RestoreAsync(IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync($"{_settings.Host}/Api/Contacts/Attributes/Restore", ids, ct);
        }
    }
}