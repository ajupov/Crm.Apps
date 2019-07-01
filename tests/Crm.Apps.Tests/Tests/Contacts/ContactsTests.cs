using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Crm.Apps.Tests.Creator;
using Crm.Clients.Contacts.Clients;
using Crm.Clients.Contacts.Models;
using Crm.Utils.DateTime;
using Crm.Utils.Guid;
using Xunit;

namespace Crm.Apps.Tests.Tests.Contacts
{
    public class ContactTests
    {
        private readonly ICreate _create;
        private readonly IContactsClient _contactsClient;

        public ContactTests(ICreate create, IContactsClient contactsClient)
        {
            _create = create;
            _contactsClient = contactsClient;
        }

        [Fact]
        public async Task WhenGet_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync().ConfigureAwait(false);
            var leadSource = await _create.LeadSource.WithAccountId(account.Id).BuildAsync().ConfigureAwait(false);
            var lead = await _create.Lead.WithAccountId(account.Id).WithSourceId(leadSource.Id).BuildAsync()
                .ConfigureAwait(false);
            var contactId = (await _create.Contact.WithAccountId(account.Id).WithLeadId(lead.Id).BuildAsync()
                .ConfigureAwait(false)).Id;

            var contact = await _contactsClient.GetAsync(contactId).ConfigureAwait(false);

            Assert.NotNull(contact);
            Assert.Equal(contactId, contact.Id);
        }

        [Fact]
        public async Task WhenGetList_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync().ConfigureAwait(false);
            var leadSource = await _create.LeadSource.WithAccountId(account.Id).BuildAsync().ConfigureAwait(false);
            var lead1 = await _create.Lead.WithAccountId(account.Id).WithSourceId(leadSource.Id).BuildAsync()
                .ConfigureAwait(false);
            var lead2 = await _create.Lead.WithAccountId(account.Id).WithSourceId(leadSource.Id).BuildAsync()
                .ConfigureAwait(false);
            var contactIds = (await Task.WhenAll(
                    _create.Contact.WithAccountId(account.Id).WithLeadId(lead1.Id).WithTaxNumber("999999999990")
                        .BuildAsync(),
                    _create.Contact.WithAccountId(account.Id).WithLeadId(lead2.Id).WithTaxNumber("999999999991")
                        .BuildAsync())
                .ConfigureAwait(false)).Select(x => x.Id).ToList();

            var contacts = await _contactsClient.GetListAsync(contactIds).ConfigureAwait(false);

            Assert.NotEmpty(contacts);
            Assert.Equal(contactIds.Count, contacts.Count);
        }

        [Fact]
        public async Task WhenGetPagedList_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync().ConfigureAwait(false);
            var leadSource = await _create.LeadSource.WithAccountId(account.Id).BuildAsync().ConfigureAwait(false);
            var lead1 = await _create.Lead.WithAccountId(account.Id).WithSourceId(leadSource.Id).BuildAsync()
                .ConfigureAwait(false);
            var lead2 = await _create.Lead.WithAccountId(account.Id).WithSourceId(leadSource.Id).BuildAsync()
                .ConfigureAwait(false);
            var attribute = await _create.ContactAttribute.WithAccountId(account.Id).BuildAsync().ConfigureAwait(false);
            await Task.WhenAll(
                    _create.Contact.WithAccountId(account.Id).WithLeadId(lead1.Id).WithTaxNumber("999999999990")
                        .WithAttributeLink(attribute.Id, "Test").BuildAsync(),
                    _create.Contact.WithAccountId(account.Id).WithLeadId(lead2.Id).WithTaxNumber("999999999991")
                        .WithAttributeLink(attribute.Id, "Test").BuildAsync())
                .ConfigureAwait(false);
            var filterAttributes = new Dictionary<Guid, string> {{attribute.Id, "Test"}};

            var contacts = await _contactsClient.GetPagedListAsync(account.Id, sortBy: "CreateDateTime",
                orderBy: "desc", allAttributes: false, attributes: filterAttributes).ConfigureAwait(false);

            var results = contacts.Skip(1)
                .Zip(contacts, (previous, current) => current.CreateDateTime >= previous.CreateDateTime);

            Assert.NotEmpty(contacts);
            Assert.All(results, Assert.True);
        }

        [Fact]
        public async Task WhenCreate_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync().ConfigureAwait(false);
            var leadSource = await _create.LeadSource.WithAccountId(account.Id).BuildAsync().ConfigureAwait(false);
            var lead = await _create.Lead.WithAccountId(account.Id).WithSourceId(leadSource.Id).BuildAsync()
                .ConfigureAwait(false);
            var attribute = await _create.ContactAttribute.WithAccountId(account.Id).BuildAsync().ConfigureAwait(false);

            var contact = new Contact
            {
                AccountId = account.Id,
                LeadId = lead.Id,
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
                IsDeleted = false,
                AttributeLinks = new List<ContactAttributeLink>
                {
                    new ContactAttributeLink
                    {
                        ContactAttributeId = attribute.Id,
                        Value = "Test"
                    }
                },
                BankAccounts = new List<ContactBankAccount>
                {
                    new ContactBankAccount
                    {
                        Number = "9999999999999999999999999",
                        BankNumber = "9999999999",
                        BankCorrespondentNumber = "9999999999999999999999999",
                        BankName = "Test"
                    }
                }
            };

            var createdContactId = await _contactsClient.CreateAsync(contact).ConfigureAwait(false);

            var createdContact = await _contactsClient.GetAsync(createdContactId).ConfigureAwait(false);

            Assert.NotNull(createdContact);
            Assert.Equal(createdContactId, createdContact.Id);
            Assert.Equal(contact.AccountId, createdContact.AccountId);
            Assert.Equal(contact.LeadId, createdContact.LeadId);
            Assert.Equal(contact.CompanyId, createdContact.CompanyId);
            Assert.True(!createdContact.CreateUserId.IsEmpty());
            Assert.Equal(contact.ResponsibleUserId, createdContact.ResponsibleUserId);
            Assert.Equal(contact.Surname, createdContact.Surname);
            Assert.Equal(contact.Name, createdContact.Name);
            Assert.Equal(contact.Patronymic, createdContact.Patronymic);
            Assert.Equal(contact.Phone, createdContact.Phone);
            Assert.Equal(contact.Email, createdContact.Email);
            Assert.Equal(contact.TaxNumber, createdContact.TaxNumber);
            Assert.Equal(contact.Postcode, createdContact.Postcode);
            Assert.Equal(contact.Country, createdContact.Country);
            Assert.Equal(contact.Region, createdContact.Region);
            Assert.Equal(contact.Province, createdContact.Province);
            Assert.Equal(contact.City, createdContact.City);
            Assert.Equal(contact.Street, createdContact.Street);
            Assert.Equal(contact.House, createdContact.House);
            Assert.Equal(contact.Apartment, createdContact.Apartment);
            Assert.Equal(contact.BirthDate, createdContact.BirthDate);
            Assert.Equal(contact.IsDeleted, createdContact.IsDeleted);
            Assert.True(createdContact.CreateDateTime.IsMoreThanMinValue());
            Assert.NotEmpty(createdContact.AttributeLinks);
            Assert.NotEmpty(createdContact.BankAccounts);
        }

        [Fact]
        public async Task WhenUpdate_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync().ConfigureAwait(false);
            var leadSource = await _create.LeadSource.WithAccountId(account.Id).BuildAsync().ConfigureAwait(false);
            var lead = await _create.Lead.WithAccountId(account.Id).WithSourceId(leadSource.Id).BuildAsync()
                .ConfigureAwait(false);
            var attribute = await _create.ContactAttribute.WithAccountId(account.Id).BuildAsync().ConfigureAwait(false);
            var contact = await _create.Contact.WithAccountId(account.Id).WithLeadId(lead.Id).BuildAsync()
                .ConfigureAwait(false);

            contact.ResponsibleUserId = Guid.Empty;
            contact.Surname = "Test";
            contact.Name = "Test";
            contact.Patronymic = "Test";
            contact.Phone = "9999999999";
            contact.Email = "test@test";
            contact.TaxNumber = "999999999999";
            contact.Post = "Test";
            contact.Postcode = "000000";
            contact.Country = "Test";
            contact.Region = "Test";
            contact.Province = "Test";
            contact.City = "Test";
            contact.Street = "Test";
            contact.House = "1";
            contact.Apartment = "1";
            contact.BirthDate = new DateTime(1990, 1, 1);
            contact.IsDeleted = true;
            contact.AttributeLinks.Add(new ContactAttributeLink {ContactAttributeId = attribute.Id, Value = "Test"});
            contact.BankAccounts.Add(new ContactBankAccount
            {
                Number = "9999999999999999999999999",
                BankNumber = "9999999999",
                BankCorrespondentNumber = "9999999999999999999999999",
                BankName = "Test"
            });
            await _contactsClient.UpdateAsync(contact).ConfigureAwait(false);

            var updatedContact = await _contactsClient.GetAsync(contact.Id).ConfigureAwait(false);

            Assert.Equal(contact.AccountId, updatedContact.AccountId);
            Assert.Equal(contact.LeadId, updatedContact.LeadId);
            Assert.Equal(contact.CreateUserId, updatedContact.CreateUserId);
            Assert.Equal(contact.ResponsibleUserId, updatedContact.ResponsibleUserId);
            Assert.Equal(contact.Surname, updatedContact.Surname);
            Assert.Equal(contact.Name, updatedContact.Name);
            Assert.Equal(contact.Patronymic, updatedContact.Patronymic);
            Assert.Equal(contact.Phone, updatedContact.Phone);
            Assert.Equal(contact.Email, updatedContact.Email);
            Assert.Equal(contact.TaxNumber, updatedContact.TaxNumber);
            Assert.Equal(contact.Post, updatedContact.Post);
            Assert.Equal(contact.Postcode, updatedContact.Postcode);
            Assert.Equal(contact.Country, updatedContact.Country);
            Assert.Equal(contact.Region, updatedContact.Region);
            Assert.Equal(contact.Province, updatedContact.Province);
            Assert.Equal(contact.City, updatedContact.City);
            Assert.Equal(contact.Street, updatedContact.Street);
            Assert.Equal(contact.House, updatedContact.House);
            Assert.Equal(contact.Apartment, updatedContact.Apartment);
            Assert.Equal(contact.BirthDate, updatedContact.BirthDate);
            Assert.Equal(contact.IsDeleted, updatedContact.IsDeleted);
            Assert.Equal(contact.AttributeLinks.Single().ContactAttributeId,
                updatedContact.AttributeLinks.Single().ContactAttributeId);
            Assert.Equal(contact.AttributeLinks.Single().Value, updatedContact.AttributeLinks.Single().Value);
            Assert.Equal(contact.BankAccounts.Single().Number, updatedContact.BankAccounts.Single().Number);
            Assert.Equal(contact.BankAccounts.Single().BankNumber, updatedContact.BankAccounts.Single().BankNumber);
        }

        [Fact]
        public async Task WhenDelete_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync().ConfigureAwait(false);
            var leadSource = await _create.LeadSource.WithAccountId(account.Id).BuildAsync().ConfigureAwait(false);
            var lead1 = await _create.Lead.WithAccountId(account.Id).WithSourceId(leadSource.Id).BuildAsync()
                .ConfigureAwait(false);
            var lead2 = await _create.Lead.WithAccountId(account.Id).WithSourceId(leadSource.Id).BuildAsync()
                .ConfigureAwait(false);
            var contactIds = (await Task.WhenAll(
                    _create.Contact.WithAccountId(account.Id).WithLeadId(lead1.Id).WithTaxNumber("999999999990")
                        .BuildAsync(),
                    _create.Contact.WithAccountId(account.Id).WithLeadId(lead2.Id).WithTaxNumber("999999999991")
                        .BuildAsync())
                .ConfigureAwait(false)).Select(x => x.Id).ToList();

            await _contactsClient.DeleteAsync(contactIds).ConfigureAwait(false);

            var contacts = await _contactsClient.GetListAsync(contactIds).ConfigureAwait(false);

            Assert.All(contacts, x => Assert.True(x.IsDeleted));
        }

        [Fact]
        public async Task WhenRestore_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync().ConfigureAwait(false);
            var leadSource = await _create.LeadSource.WithAccountId(account.Id).BuildAsync().ConfigureAwait(false);
            var lead1 = await _create.Lead.WithAccountId(account.Id).WithSourceId(leadSource.Id).BuildAsync()
                .ConfigureAwait(false);
            var lead2 = await _create.Lead.WithAccountId(account.Id).WithSourceId(leadSource.Id).BuildAsync()
                .ConfigureAwait(false);
            var contactIds = (await Task.WhenAll(
                    _create.Contact.WithAccountId(account.Id).WithLeadId(lead1.Id).WithTaxNumber("999999999990")
                        .BuildAsync(),
                    _create.Contact.WithAccountId(account.Id).WithLeadId(lead2.Id).WithTaxNumber("999999999991")
                        .BuildAsync())
                .ConfigureAwait(false)).Select(x => x.Id).ToList();

            await _contactsClient.RestoreAsync(contactIds).ConfigureAwait(false);

            var contacts = await _contactsClient.GetListAsync(contactIds).ConfigureAwait(false);

            Assert.All(contacts, x => Assert.False(x.IsDeleted));
        }
    }
}