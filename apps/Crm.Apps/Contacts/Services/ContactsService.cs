using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Contacts.Helpers;
using Crm.Apps.Contacts.Models;
using Crm.Apps.Contacts.Parameters;
using Crm.Apps.Contacts.Storages;
using Crm.Utils.Guid;
using Crm.Utils.String;
using Microsoft.EntityFrameworkCore;

namespace Crm.Apps.Contacts.Services
{
    public class ContactsService : IContactsService
    {
        private readonly ContactsStorage _storage;

        public ContactsService(ContactsStorage storage)
        {
            _storage = storage;
        }

        public Task<Contact> GetAsync(Guid id, CancellationToken ct)
        {
            return _storage.Contacts.Include(x => x.BankAccounts).Include(x => x.AttributeLinks)
                .FirstOrDefaultAsync(x => x.Id == id, ct);
        }

        public Task<List<Contact>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct)
        {
            return _storage.Contacts.Where(x => ids.Contains(x.Id)).ToListAsync(ct);
        }

        public async Task<List<Contact>> GetPagedListAsync(ContactGetPagedListParameter parameter, CancellationToken ct)
        {
            var temp = await _storage.Contacts.Include(x => x.BankAccounts).Include(x => x.AttributeLinks).Where(x =>
                    (parameter.AccountId.IsEmpty() || x.AccountId == parameter.AccountId) &&
                    (parameter.Surname.IsEmpty() || EF.Functions.Like(x.Surname, $"{parameter.Surname}%")) &&
                    (parameter.Name.IsEmpty() || EF.Functions.Like(x.Name, $"{parameter.Name}%")) &&
                    (parameter.Patronymic.IsEmpty() || EF.Functions.Like(x.Patronymic, $"{parameter.Patronymic}%")) &&
                    (parameter.Phone.IsEmpty() || x.Phone == parameter.Phone) &&
                    (parameter.Email.IsEmpty() || x.Email == parameter.Email) &&
                    (parameter.TaxNumber.IsEmpty() || x.TaxNumber == parameter.TaxNumber) &&
                    (parameter.Post.IsEmpty() || EF.Functions.Like(x.Post, $"{parameter.Post}%")) &&
                    (parameter.Postcode.IsEmpty() || x.Postcode == parameter.Postcode) &&
                    (parameter.Country.IsEmpty() || EF.Functions.Like(x.Country, $"{parameter.Country}%")) &&
                    (parameter.Region.IsEmpty() || EF.Functions.Like(x.Region, $"{parameter.Region}%")) &&
                    (parameter.Province.IsEmpty() || EF.Functions.Like(x.Province, $"{parameter.Province}%")) &&
                    (parameter.City.IsEmpty() || EF.Functions.Like(x.City, $"{parameter.City}%")) &&
                    (parameter.Street.IsEmpty() || EF.Functions.Like(x.Street, $"{parameter.Street}%")) &&
                    (parameter.House.IsEmpty() || EF.Functions.Like(x.House, $"{parameter.House}%")) &&
                    (parameter.Apartment.IsEmpty() || x.Apartment == parameter.Apartment) &&
                    (parameter.MinBirthDate == null || x.BirthDate >= parameter.MinBirthDate.Value) &&
                    (parameter.MaxBirthDate == null || x.BirthDate <= parameter.MaxBirthDate) &&
                    (!parameter.IsDeleted.HasValue || x.IsDeleted == parameter.IsDeleted) &&
                    (!parameter.MinCreateDate.HasValue || x.CreateDateTime >= parameter.MinCreateDate) &&
                    (!parameter.MaxCreateDate.HasValue || x.CreateDateTime <= parameter.MaxCreateDate))
                .Sort(parameter.SortBy, parameter.OrderBy)
                .ToListAsync(ct).ConfigureAwait(false);

            return temp.Where(x => x.FilterByAdditional(parameter)).Skip(parameter.Offset).Take(parameter.Limit)
                .ToList();
        }

        public async Task<Guid> CreateAsync(Guid userId, Contact contact, CancellationToken ct)
        {
            var newContact = new Contact();
            var change = newContact.CreateWithLog(userId, x =>
            {
                x.Id = Guid.NewGuid();
                x.AccountId = contact.AccountId;
                x.LeadId = contact.LeadId;
                x.CompanyId = contact.CompanyId;
                x.CreateUserId = userId;
                x.ResponsibleUserId = contact.ResponsibleUserId;
                x.Surname = contact.Surname;
                x.Name = contact.Name;
                x.Patronymic = contact.Patronymic;
                x.Phone = contact.Phone;
                x.Email = contact.Email;
                x.TaxNumber = contact.TaxNumber;
                x.Post = contact.Post;
                x.Postcode = contact.Postcode;
                x.Country = contact.Country;
                x.Region = contact.Region;
                x.Province = contact.Province;
                x.City = contact.City;
                x.Street = contact.Street;
                x.House = contact.House;
                x.Apartment = contact.Apartment;
                x.BirthDate = contact.BirthDate;
                x.Photo = contact.Photo;
                x.IsDeleted = contact.IsDeleted;
                x.CreateDateTime = DateTime.UtcNow;
                x.BankAccounts = contact.BankAccounts;
                x.AttributeLinks = contact.AttributeLinks;
            });

            var entry = await _storage.AddAsync(newContact, ct).ConfigureAwait(false);
            await _storage.AddAsync(change, ct).ConfigureAwait(false);
            await _storage.SaveChangesAsync(ct).ConfigureAwait(false);

            return entry.Entity.Id;
        }

        public async Task UpdateAsync(Guid contactId, Contact oldContact, Contact newContact, CancellationToken ct)
        {
            var change = oldContact.UpdateWithLog(contactId, x =>
            {
                x.AccountId = newContact.AccountId;
                x.LeadId = newContact.LeadId;
                x.CompanyId = newContact.CompanyId;
                x.ResponsibleUserId = newContact.ResponsibleUserId;
                x.Surname = newContact.Surname;
                x.Name = newContact.Name;
                x.Patronymic = newContact.Patronymic;
                x.Phone = newContact.Phone;
                x.Email = newContact.Email;
                x.TaxNumber = newContact.TaxNumber;
                x.Post = newContact.Post;
                x.Postcode = newContact.Postcode;
                x.Country = newContact.Country;
                x.Region = newContact.Region;
                x.Province = newContact.Province;
                x.City = newContact.City;
                x.Street = newContact.Street;
                x.House = newContact.House;
                x.Apartment = newContact.Apartment;
                x.BirthDate = newContact.BirthDate;
                x.Photo = newContact.Photo;
                x.IsDeleted = newContact.IsDeleted;
                x.BankAccounts = newContact.BankAccounts;
                x.AttributeLinks = newContact.AttributeLinks;
            });

            _storage.Update(oldContact);
            await _storage.AddAsync(change, ct).ConfigureAwait(false);
            await _storage.SaveChangesAsync(ct).ConfigureAwait(false);
        }

        public async Task DeleteAsync(Guid contactId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            var changes = new List<ContactChange>();

            await _storage.Contacts.Where(x => ids.Contains(x.Id))
                .ForEachAsync(u => changes.Add(u.UpdateWithLog(contactId, x => x.IsDeleted = true)), ct)
                .ConfigureAwait(false);

            await _storage.AddRangeAsync(changes, ct).ConfigureAwait(false);
            await _storage.SaveChangesAsync(ct).ConfigureAwait(false);
        }

        public async Task RestoreAsync(Guid contactId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            var changes = new List<ContactChange>();

            await _storage.Contacts.Where(x => ids.Contains(x.Id))
                .ForEachAsync(u => changes.Add(u.UpdateWithLog(contactId, x => x.IsDeleted = false)), ct)
                .ConfigureAwait(false);

            await _storage.AddRangeAsync(changes, ct).ConfigureAwait(false);
            await _storage.SaveChangesAsync(ct).ConfigureAwait(false);
        }
    }
}