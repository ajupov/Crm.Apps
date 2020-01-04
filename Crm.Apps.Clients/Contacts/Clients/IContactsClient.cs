using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Clients.Contacts.Models;

namespace Crm.Apps.Clients.Contacts.Clients
{
    public interface IContactsClient
    {
        Task<Contact> GetAsync(Guid id, CancellationToken ct = default);

        Task<List<Contact>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct = default);

        Task<List<Contact>> GetPagedListAsync(Guid? accountId = default, string surname = default,
            string name = default, string patronymic = default, string phone = default, string email = default,
            string taxNumber = default, string post = default, string postcode = default, string country = default,
            string region = default, string province = default, string city = default, string street = default,
            string house = default, string apartment = default, DateTime? minBirthDate = default,
            DateTime? maxBirthDate = default, bool isDeleted = default, DateTime? minCreateDate = default,
            DateTime? maxCreateDate = default, bool? allAttributes = default,
            IDictionary<Guid, string> attributes = default, string bankAccountNumber = default,
            string bankAccountBankNumber = default, string bankAccountBankCorrespondentNumber = default,
            string bankAccountBankName = default, List<Guid> leadIds = default, List<Guid> companyIds = default,
            List<Guid> createUserIds = default, List<Guid> responsibleUserIds = default, int offset = default,
            int limit = 10, string sortBy = default, string orderBy = default, CancellationToken ct = default);

        Task<Guid> CreateAsync(Contact contact, CancellationToken ct = default);

        Task UpdateAsync(Contact contact, CancellationToken ct = default);

        Task DeleteAsync(IEnumerable<Guid> ids, CancellationToken ct = default);

        Task RestoreAsync(IEnumerable<Guid> ids, CancellationToken ct = default);
    }
}