using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Crm.Clients.Companies.Models;
using Crm.Clients.Companies.Settings;
using Crm.Common.Types;
using Crm.Utils.Http;
using Microsoft.Extensions.Options;

namespace Crm.Clients.Companies.Clients
{
    public class CompanyAttributesClient : ICompanyAttributesClient
    {
        private readonly CompaniesClientSettings _settings;
        private readonly IHttpClientFactory _httpClientFactory;

        public CompanyAttributesClient(IOptions<CompaniesClientSettings> options, IHttpClientFactory httpClientFactory)
        {
            _settings = options.Value;
            _httpClientFactory = httpClientFactory;
        }

        public Task<List<AttributeType>> GetTypesAsync(CancellationToken ct = default)
        {
            return _httpClientFactory.GetAsync<List<AttributeType>>(
                $"{_settings.Host}/Api/Companies/Attributes/GetTypes", ct: ct);
        }

        public Task<CompanyAttribute> GetAsync(Guid id, CancellationToken ct = default)
        {
            return _httpClientFactory.GetAsync<CompanyAttribute>($"{_settings.Host}/Api/Companies/Attributes/Get",
                new {id}, ct);
        }

        public Task<List<CompanyAttribute>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync<List<CompanyAttribute>>(
                $"{_settings.Host}/Api/Companies/Attributes/GetList", ids, ct);
        }

        public Task<List<CompanyAttribute>> GetPagedListAsync(Guid? accountId = default,
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

            return _httpClientFactory.PostAsync<List<CompanyAttribute>>(
                $"{_settings.Host}/Api/Companies/Attributes/GetPagedList", parameter, ct);
        }

        public Task<Guid> CreateAsync(CompanyAttribute attribute, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync<Guid>($"{_settings.Host}/Api/Companies/Attributes/Create", attribute,
                ct);
        }

        public Task UpdateAsync(CompanyAttribute attribute, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync($"{_settings.Host}/Api/Companies/Attributes/Update", attribute, ct);
        }

        public Task DeleteAsync(IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync($"{_settings.Host}/Api/Companies/Attributes/Delete", ids, ct);
        }

        public Task RestoreAsync(IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync($"{_settings.Host}/Api/Companies/Attributes/Restore", ids, ct);
        }
    }
}