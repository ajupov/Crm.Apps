using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Clients.Contacts.Models;
using Crm.Apps.Clients.Contacts.Settings;
using Microsoft.Extensions.Options;

namespace Crm.Apps.Clients.Contacts.Clients
{
    public class ContactCommentsClient : IContactCommentsClient
    {
        private readonly ContactsClientSettings _settings;
        private readonly IHttpClientFactory _httpClientFactory;

        public ContactCommentsClient(IOptions<ContactsClientSettings> options, IHttpClientFactory httpClientFactory)
        {
            _url = UriBuilder.Combine(options.Value.Host, );
            _httpClientFactory = httpClientFactory;
        }

        public Task<List<ContactComment>> GetPagedListAsync(Guid? contactId = default,
            Guid? commentatorUserId = default,
            string value = default, DateTime? minCreateDate = default, DateTime? maxCreateDate = default,
            int offset = default, int limit = 10, string sortBy = default, string orderBy = default,
            CancellationToken ct = default)
        {
            var parameter = new
            {
                ContactId = contactId,
                CommentatorUserId = commentatorUserId,
                Value = value,
                MinCreateDate = minCreateDate,
                MaxCreateDate = maxCreateDate,
                Offset = offset,
                Limit = limit,
                SortBy = sortBy,
                OrderBy = orderBy
            };

            return _httpClientFactory.PostAsync<List<ContactComment>>(
                $"{_settings.Host}/Api/Contacts/Comments/GetPagedList", parameter, ct);
        }

        public Task CreateAsync(ContactComment comment, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync($"{_settings.Host}/Api/Contacts/Comments/Create", comment, ct);
        }
    }
}