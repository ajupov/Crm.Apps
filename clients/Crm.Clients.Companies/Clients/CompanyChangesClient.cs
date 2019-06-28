using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Crm.Clients.Companies.Models;
using Crm.Clients.Companies.Settings;
using Crm.Utils.Http;
using Microsoft.Extensions.Options;

namespace Crm.Clients.Companies.Clients
{
    public class CompanyChangesClient : ICompanyChangesClient
    {
        private readonly CompaniesClientSettings _settings;
        private readonly IHttpClientFactory _httpClientFactory;

        public CompanyChangesClient(IOptions<CompaniesClientSettings> options, IHttpClientFactory httpClientFactory)
        {
            _settings = options.Value;
            _httpClientFactory = httpClientFactory;
        }

        public Task<List<CompanyChange>> GetPagedListAsync(Guid? changerUserId = default, Guid? companyId = default,
            DateTime? minCreateDate = default, DateTime? maxCreateDate = default, int offset = default, int limit = 10,
            string sortBy = default, string orderBy = default, CancellationToken ct = default)
        {
            var parameter = new
            {
                ChangerUserId = changerUserId,
                CompanyId = companyId,
                MinCreateDate = minCreateDate,
                MaxCreateDate = maxCreateDate,
                Offset = offset,
                Limit = limit,
                SortBy = sortBy,
                OrderBy = orderBy
            };

            return _httpClientFactory.PostAsync<List<CompanyChange>>(
                $"{_settings.Host}/Api/Companies/Changes/GetPagedList", parameter, ct);
        }
    }
}