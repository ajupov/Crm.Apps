using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Clients.Companies.Models;
using Crm.Apps.Clients.Companies.Settings;
using Microsoft.Extensions.Options;

namespace Crm.Apps.Clients.Companies.Clients
{
    public class CompanyCommentsClient : ICompanyCommentsClient
    {
        private readonly CompaniesClientSettings _settings;
        private readonly IHttpClientFactory _httpClientFactory;

        public CompanyCommentsClient(IOptions<CompaniesClientSettings> options, IHttpClientFactory httpClientFactory)
        {
            _settings = options.Value;
            _httpClientFactory = httpClientFactory;
        }

        public Task<List<CompanyComment>> GetPagedListAsync(Guid? companyId = default, Guid? commentatorUserId = default,
            string value = default, DateTime? minCreateDate = default, DateTime? maxCreateDate = default,
            int offset = default, int limit = 10, string sortBy = default, string orderBy = default,
            CancellationToken ct = default)
        {
            var parameter = new
            {
                CompanyId = companyId,
                CommentatorUserId = commentatorUserId,
                Value = value,
                MinCreateDate = minCreateDate,
                MaxCreateDate = maxCreateDate,
                Offset = offset,
                Limit = limit,
                SortBy = sortBy,
                OrderBy = orderBy
            };

            return _httpClientFactory.PostAsync<List<CompanyComment>>(
                $"{_settings.Host}/Api/Companies/Comments/GetPagedList", parameter, ct);
        }

        public Task CreateAsync(CompanyComment comment, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync($"{_settings.Host}/Api/Companies/Comments/Create", comment, ct);
        }
    }
}