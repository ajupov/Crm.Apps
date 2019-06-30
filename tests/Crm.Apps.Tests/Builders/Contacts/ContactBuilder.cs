using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Crm.Clients.Contacts.Clients;
using Crm.Clients.Contacts.Models;
using Crm.Utils.Guid;

namespace Crm.Apps.Tests.Builders.Contacts
{
    public class ContactBuilder : IContactBuilder
    {
        private readonly Contact _contact;
        private readonly IContactsClient _contactsClient;

        public ContactBuilder(IContactsClient contactsClient)
        {
            _contactsClient = contactsClient;
            _contact = new Contact
            {
                AccountId = Guid.Empty,
                LeadId = Guid.Empty,
                CompanyId = Guid.Empty,
                CreateUserId = Guid.Empty,
                ResponsibleUserId = Guid.Empty,
                Surname = "Test",
                Name = "Test",
                Patronymic = "Test",
                Phone = "9999999999",
                Email = "test@test",
                TaxNumber = "999999999999",
                Post = "Test",
                Postcode = "000000",
                Country = "Test",
                Region = "Test",
                Province = "Test",
                City = "Test",
                Street = "Test",
                House = "1",
                Apartment = "1",
                BirthDate = new DateTime(1980, 1, 1),
                IsDeleted = false
            };
        }

        public ContactBuilder WithAccountId(Guid accountId)
        {
            _contact.AccountId = accountId;

            return this;
        }

        public ContactBuilder WithLeadId(Guid leadId)
        {
            _contact.LeadId = leadId;

            return this;
        }

        public ContactBuilder WithCompanyId(Guid companyId)
        {
            _contact.CompanyId = companyId;

            return this;
        }

        public ContactBuilder WithCreateUserId(Guid createUserId)
        {
            _contact.CreateUserId = createUserId;

            return this;
        }

        public ContactBuilder WithResponsibleUserId(Guid responsibleUserId)
        {
            _contact.ResponsibleUserId = responsibleUserId;

            return this;
        }

        public ContactBuilder WithSurname(string surname)
        {
            _contact.Surname = surname;

            return this;
        }

        public ContactBuilder WithName(string name)
        {
            _contact.Name = name;

            return this;
        }

        public ContactBuilder WithPatronymic(string patronymic)
        {
            _contact.Patronymic = patronymic;

            return this;
        }

        public ContactBuilder WithPhone(string phone)
        {
            _contact.Phone = phone;

            return this;
        }

        public ContactBuilder WithEmail(string email)
        {
            _contact.Email = email;

            return this;
        }

        public ContactBuilder WithTaxNumber(string taxNumber)
        {
            _contact.TaxNumber = taxNumber;

            return this;
        }

        public ContactBuilder WithPost(string post)
        {
            _contact.Post = post;

            return this;
        }

        public ContactBuilder WithPostcode(string postcode)
        {
            _contact.Postcode = postcode;

            return this;
        }

        public ContactBuilder WithCountry(string country)
        {
            _contact.Country = country;

            return this;
        }

        public ContactBuilder WithRegion(string region)
        {
            _contact.Region = region;

            return this;
        }

        public ContactBuilder WithProvince(string province)
        {
            _contact.Province = province;

            return this;
        }

        public ContactBuilder WithCity(string city)
        {
            _contact.City = city;

            return this;
        }

        public ContactBuilder WithStreet(string street)
        {
            _contact.Street = street;

            return this;
        }

        public ContactBuilder WithHouse(string house)
        {
            _contact.House = house;

            return this;
        }

        public ContactBuilder WithApartment(string apartment)
        {
            _contact.Apartment = apartment;

            return this;
        }

        public ContactBuilder WithBirthDate(DateTime birthDate)
        {
            _contact.BirthDate = birthDate;

            return this;
        }

        public ContactBuilder AsDeleted()
        {
            _contact.IsDeleted = true;

            return this;
        }

        public ContactBuilder WithAttributeLink(Guid attributeId, string value)
        {
            if (_contact.AttributeLinks == null)
            {
                _contact.AttributeLinks = new List<ContactAttributeLink>();
            }

            _contact.AttributeLinks.Add(new ContactAttributeLink
            {
                ContactAttributeId = attributeId,
                Value = value
            });

            return this;
        }

        public ContactBuilder WithBankAccount(string number, string bankNumber, string bankName,
            string bankCorrespondentNumber)
        {
            if (_contact.BankAccounts == null)
            {
                _contact.BankAccounts = new List<ContactBankAccount>();
            }

            _contact.BankAccounts.Add(new ContactBankAccount
            {
                Number = number,
                BankNumber = bankNumber,
                BankName = bankName,
                BankCorrespondentNumber = bankCorrespondentNumber
            });

            return this;
        }

        public async Task<Contact> BuildAsync()
        {
            if (_contact.AccountId.IsEmpty())
            {
                throw new InvalidOperationException(nameof(_contact.AccountId));
            }

            if (_contact.LeadId.IsEmpty())
            {
                throw new InvalidOperationException(nameof(_contact.LeadId));
            }

            var createdId = await _contactsClient.CreateAsync(_contact).ConfigureAwait(false);

            return await _contactsClient.GetAsync(createdId).ConfigureAwait(false);
        }
    }
}