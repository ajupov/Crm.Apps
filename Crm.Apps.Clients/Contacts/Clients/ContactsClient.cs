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
    public class ContactsClient : IContactsClient
    {
        private readonly ContactsClientSettings _settings;
        private readonly IHttpClientFactory _httpClientFactory;

        public ContactsClient(IOptions<ContactsClientSettings> options, IHttpClientFactory httpClientFactory)
        {
            _settings = options.Value;
            _httpClientFactory = httpClientFactory;
        }

        public Task<Contact> GetAsync(Guid id, CancellationToken ct = default)
        {
            return _httpClientFactory.GetAsync<Contact>($"{_settings.Host}/Api/Contacts/Get", new {id}, ct);
        }

        public Task<List<Contact>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync<List<Contact>>($"{_settings.Host}/Api/Contacts/GetList", ids, ct);
        }

        public Task<List<Contact>> GetPagedListAsync(Guid? accountId = default, string surname = default,
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
            int limit = 10, string sortBy = default, string orderBy = default, CancellationToken ct = default)
        {
            var parameter = new
            {
                AccountId = accountId,
                Surname = surname,
                Name = name,
                Patronymic = patronymic,
                Phone = phone,
                Email = email,
                TaxNumber = taxNumber,
                Post = post,
                Postcode = postcode,
                Country = country,
                Region = region,
                Province = province,
                City = city,
                Street = street,
                House = house,
                Apartment = apartment,
                MinBirthDate = minBirthDate,
                MaxBirthDate = maxBirthDate,
                IsDeleted = isDeleted,
                MinCreateDate = minCreateDate,
                MaxCreateDate = maxCreateDate,
                AllAttributes = allAttributes,
                Attributes = attributes,
                BankAccountNumber = bankAccountNumber,
                BankAccountBankNumber = bankAccountBankNumber,
                BankAccountBankCorrespondentNumber = bankAccountBankCorrespondentNumber,
                BankAccountBankName = bankAccountBankName,
                LeadIds = leadIds,
                CompanyIds = companyIds,
                CreateUserIds = createUserIds,
                ResponsibleUserIds = responsibleUserIds,
                Offset = offset,
                Limit = limit,
                SortBy = sortBy,
                OrderBy = orderBy
            };

            return _httpClientFactory.PostAsync<List<Contact>>($"{_settings.Host}/Api/Contacts/GetPagedList", parameter,
                ct);
        }

        public Task<Guid> CreateAsync(Contact contact, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync<Guid>($"{_settings.Host}/Api/Contacts/Create", contact, ct);
        }

        public Task UpdateAsync(Contact contact, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync($"{_settings.Host}/Api/Contacts/Update", contact, ct);
        }

        public Task DeleteAsync(IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync($"{_settings.Host}/Api/Contacts/Delete", ids, ct);
        }

        public Task RestoreAsync(IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync($"{_settings.Host}/Api/Contacts/Restore", ids, ct);
        }
    }
}