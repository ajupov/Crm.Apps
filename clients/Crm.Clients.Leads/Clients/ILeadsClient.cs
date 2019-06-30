using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Clients.Leads.Models;

namespace Crm.Clients.Leads.Clients
{
    public interface ILeadsClient
    {
        Task<Lead> GetAsync(Guid id, CancellationToken ct = default);

        Task<List<Lead>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct = default);

        Task<List<Lead>> GetPagedListAsync(Guid? accountId = default, string surname = default, string name = default,
            string patronymic = default, string phone = default, string email = default, string companyName = default,
            string post = default, string postcode = default, string country = default, string region = default,
            string province = default, string city = default, string street = default, string house = default,
            string apartment = default, decimal? minOpportunitySum = default, decimal? maxOpportunitySum = default,
            bool isDeleted = default, DateTime? minCreateDate = default, DateTime? maxCreateDate = default,
            bool? allAttributes = default, IDictionary<Guid, string> attributes = default,
            List<Guid> sourceIds = default, List<Guid> createUserIds = default, List<Guid> responsibleUserIds = default,
            int offset = default, int limit = 10, string sortBy = default, string orderBy = default,
            CancellationToken ct = default);

        Task<Guid> CreateAsync(Lead lead, CancellationToken ct = default);

        Task UpdateAsync(Lead lead, CancellationToken ct = default);

        Task DeleteAsync(IEnumerable<Guid> ids, CancellationToken ct = default);

        Task RestoreAsync(IEnumerable<Guid> ids, CancellationToken ct = default);
    }
}