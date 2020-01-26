using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Utils.All.Guid;
using Ajupov.Utils.All.Sorting;
using Ajupov.Utils.All.String;
using Crm.Apps.Contacts.Helpers;
using Crm.Apps.Contacts.Mappers;
using Crm.Apps.Contacts.Storages;
using Crm.Apps.Contacts.v1.Models;
using Crm.Apps.Contacts.v1.RequestParameters;
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
            return _storage.Contacts
                .Include(x => x.BankAccounts)
                .Include(x => x.AttributeLinks)
                .FirstOrDefaultAsync(x => x.Id == id, ct);
        }

        public Task<List<Contact>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct)
        {
            return _storage.Contacts
                .Where(x => ids.Contains(x.Id))
                .ToListAsync(ct);
        }

        public async Task<List<Contact>> GetPagedListAsync(
            ContactGetPagedListRequestParameter request,
            CancellationToken ct)
        {
            var temp = await _storage.Contacts
                .Include(x => x.BankAccounts)
                .Include(x => x.AttributeLinks)
                .Where(x =>
                    (request.AccountId.IsEmpty() || x.AccountId == request.AccountId) &&
                    (request.Surname.IsEmpty() || EF.Functions.Like(x.Surname, $"{request.Surname}%")) &&
                    (request.Name.IsEmpty() || EF.Functions.Like(x.Name, $"{request.Name}%")) &&
                    (request.Patronymic.IsEmpty() || EF.Functions.Like(x.Patronymic, $"{request.Patronymic}%")) &&
                    (request.Phone.IsEmpty() || x.Phone == request.Phone) &&
                    (request.Email.IsEmpty() || x.Email == request.Email) &&
                    (request.TaxNumber.IsEmpty() || x.TaxNumber == request.TaxNumber) &&
                    (request.Post.IsEmpty() || EF.Functions.Like(x.Post, $"{request.Post}%")) &&
                    (request.Postcode.IsEmpty() || x.Postcode == request.Postcode) &&
                    (request.Country.IsEmpty() || EF.Functions.Like(x.Country, $"{request.Country}%")) &&
                    (request.Region.IsEmpty() || EF.Functions.Like(x.Region, $"{request.Region}%")) &&
                    (request.Province.IsEmpty() || EF.Functions.Like(x.Province, $"{request.Province}%")) &&
                    (request.City.IsEmpty() || EF.Functions.Like(x.City, $"{request.City}%")) &&
                    (request.Street.IsEmpty() || EF.Functions.Like(x.Street, $"{request.Street}%")) &&
                    (request.House.IsEmpty() || EF.Functions.Like(x.House, $"{request.House}%")) &&
                    (request.Apartment.IsEmpty() || x.Apartment == request.Apartment) &&
                    (request.MinBirthDate == null || x.BirthDate >= request.MinBirthDate.Value) &&
                    (request.MaxBirthDate == null || x.BirthDate <= request.MaxBirthDate) &&
                    (!request.IsDeleted.HasValue || x.IsDeleted == request.IsDeleted) &&
                    (!request.MinCreateDate.HasValue || x.CreateDateTime >= request.MinCreateDate) &&
                    (!request.MaxCreateDate.HasValue || x.CreateDateTime <= request.MaxCreateDate) &&
                    (!request.MinModifyDate.HasValue || x.ModifyDateTime >= request.MinModifyDate) &&
                    (!request.MaxModifyDate.HasValue || x.ModifyDateTime <= request.MaxModifyDate))
                .SortBy(request.SortBy, request.OrderBy)
                .ToListAsync(ct);

            return temp
                .Where(x => x.FilterByAdditional(request))
                .Skip(request.Offset)
                .Take(request.Limit)
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
                x.BankAccounts = contact.BankAccounts.Map(x.Id);
                x.AttributeLinks = contact.AttributeLinks.Map(x.Id);
            });

            var entry = await _storage.AddAsync(newContact, ct);
            await _storage.AddAsync(change, ct);
            await _storage.SaveChangesAsync(ct);

            return entry.Entity.Id;
        }

        public async Task UpdateAsync(Guid userId, Contact oldContact, Contact newContact, CancellationToken ct)
        {
            var change = oldContact.UpdateWithLog(userId, x =>
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
                x.ModifyDateTime = DateTime.UtcNow;
                x.BankAccounts = newContact.BankAccounts.Map(x.Id);
                x.AttributeLinks = newContact.AttributeLinks.Map(x.Id);
            });

            _storage.Update(oldContact);
            await _storage.AddAsync(change, ct);
            await _storage.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            var changes = new List<ContactChange>();

            await _storage.Contacts
                .Where(x => ids.Contains(x.Id))
                .ForEachAsync(x => changes.Add(x.UpdateWithLog(userId, c =>
                {
                    c.IsDeleted = true;
                    c.ModifyDateTime = DateTime.UtcNow;
                })), ct);

            await _storage.AddRangeAsync(changes, ct);
            await _storage.SaveChangesAsync(ct);
        }

        public async Task RestoreAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            var changes = new List<ContactChange>();

            await _storage.Contacts
                .Where(x => ids.Contains(x.Id))
                .ForEachAsync(x => changes.Add(x.UpdateWithLog(userId, c =>
                {
                    c.IsDeleted = false;
                    c.ModifyDateTime = DateTime.UtcNow;
                })), ct);

            await _storage.AddRangeAsync(changes, ct);
            await _storage.SaveChangesAsync(ct);
        }
    }
}