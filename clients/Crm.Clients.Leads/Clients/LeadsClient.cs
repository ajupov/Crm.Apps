using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Crm.Clients.Leads.Models;
using Crm.Clients.Leads.Settings;
using Crm.Utils.Http;
using Microsoft.Extensions.Options;

namespace Crm.Clients.Leads.Clients
{
    public class LeadsClient : ILeadsClient
    {
        private readonly LeadsClientSettings _settings;
        private readonly IHttpClientFactory _httpClientFactory;

        public LeadsClient(IOptions<LeadsClientSettings> options, IHttpClientFactory httpClientFactory)
        {
            _settings = options.Value;
            _httpClientFactory = httpClientFactory;
        }

        public Task<Lead> GetAsync(Guid id, CancellationToken ct = default)
        {
            return _httpClientFactory.GetAsync<Lead>($"{_settings.Host}/Api/Leads/Get", new {id}, ct);
        }

        public Task<List<Lead>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync<List<Lead>>($"{_settings.Host}/Api/Leads/GetList", ids, ct);
        }

        public Task<List<Lead>> GetPagedListAsync(Guid? accountId = default, string surname = default,
            string name = default, string patronymic = default, string phone = default, string email = default,
            string companyName = default, string post = default, string postCode = default, string country = default,
            string region = default, string province = default, string city = default, string street = default,
            string house = default, string apartment = default, decimal? minOpportunitySum = default,
            decimal? maxOpportunitySum = default, bool isDeleted = default, DateTime? minCreateDate = default,
            DateTime? maxCreateDate = default, bool? allAttributes = default,
            IDictionary<Guid, string> attributes = default, List<Guid> sourceIds = default,
            List<Guid> createUserIds = default, List<Guid> responsibleUserIds = default, int? offset = default,
            int? limit = 10, string sortBy = default, string orderBy = default, CancellationToken ct = default)
        {
            var parameter = new
            {
                AccountId = accountId,
                Surname = surname,
                Name = name,
                Patronymic = patronymic,
                Phone = phone,
                Email = email,
                CompanyName = companyName,
                Post = post,
                PostCode = postCode,
                Country = country,
                Region = region,
                Province = province,
                City = city,
                Street = street,
                House = house,
                Apartment = apartment,
                MinOpportunitySum = minOpportunitySum,
                MaxOpportunitySum = maxOpportunitySum,
                IsDeleted = isDeleted,
                MinCreateDate = minCreateDate,
                MaxCreateDate = maxCreateDate,
                AllAttributes = allAttributes,
                Attributes = attributes,
                SourceIds = sourceIds,
                CreateUserIds = createUserIds,
                ResponsibleUserIds = responsibleUserIds,
                Offset = offset,
                Limit = limit,
                SortBy = sortBy,
                OrderBy = orderBy
            };

            return _httpClientFactory.PostAsync<List<Lead>>($"{_settings.Host}/Api/Leads/GetPagedList", parameter, ct);
        }

        public Task<Guid> CreateAsync(Lead lead, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync<Guid>($"{_settings.Host}/Api/Leads/Create", lead, ct);
        }

        public Task UpdateAsync(Lead lead, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync($"{_settings.Host}/Api/Leads/Update", lead, ct);
        }

        public Task DeleteAsync(IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync($"{_settings.Host}/Api/Leads/Delete", ids, ct);
        }

        public Task RestoreAsync(IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync($"{_settings.Host}/Api/Leads/Restore", ids, ct);
        }
    }
}