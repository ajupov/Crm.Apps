using System;
using System.Collections.Generic;
using System.Linq;
using Ajupov.Utils.All.String;
using Crm.Apps.Contacts.Models;
using Crm.Apps.Contacts.v1.Requests;
using Microsoft.EntityFrameworkCore;

namespace Crm.Apps.Contacts.Helpers
{
    public static class ContactsFiltersHelper
    {
        public static bool FilterByAdditional(this Contact contact, ContactGetPagedListRequest request)
        {
            return (request.BankAccountNumber.IsEmpty() ||
                    contact.BankAccounts.Any(x => x.Number == request.BankAccountNumber)) &&
                   (request.BankAccountBankNumber.IsEmpty() ||
                    contact.BankAccounts.Any(x => x.BankNumber == request.BankAccountBankNumber)) &&
                   (request.BankAccountBankCorrespondentNumber.IsEmpty() || contact.BankAccounts.Any(x =>
                        x.BankCorrespondentNumber == request.BankAccountBankCorrespondentNumber)) &&
                   (request.BankAccountBankName.IsEmpty() ||
                    contact.BankAccounts.Any(x =>
                        EF.Functions.ILike(x.BankName, $"{request.BankAccountBankName}%"))) &&
                   (request.LeadIds == null || !request.LeadIds.Any() ||
                    request.LeadIds.Any(x => LeadIdsPredicate(contact, x))) &&
                   (request.CompanyIds == null || !request.CompanyIds.Any() ||
                    request.CompanyIds.Any(x => CompanyIdsPredicate(contact, x))) &&
                   (request.CreateUserIds == null || !request.CreateUserIds.Any() ||
                    request.CreateUserIds.Any(x => CreateUserIdsPredicate(contact, x))) &&
                   (request.ResponsibleUserIds == null || !request.ResponsibleUserIds.Any() ||
                    request.ResponsibleUserIds.Any(x => ResponsibleUserIdsPredicate(contact, x))) &&
                   (request.Attributes == null || !request.Attributes.Any() ||
                    (request.AllAttributes is false
                        ? request.Attributes.Any(x => AttributePredicate(contact, x))
                        : request.Attributes.All(x => AttributePredicate(contact, x))));
        }

        private static bool LeadIdsPredicate(Contact contact, Guid id)
        {
            return contact.LeadId == id;
        }

        private static bool CompanyIdsPredicate(Contact contact, Guid id)
        {
            return contact.CompanyId == id;
        }

        private static bool CreateUserIdsPredicate(Contact contact, Guid id)
        {
            return contact.CreateUserId == id;
        }

        private static bool ResponsibleUserIdsPredicate(Contact contact, Guid id)
        {
            return contact.ResponsibleUserId == id;
        }

        private static bool AttributePredicate(Contact contact, KeyValuePair<Guid, string> pair)
        {
            var (key, value) = pair;

            return contact.AttributeLinks != null && contact.AttributeLinks.Any(x =>
                       x.ContactAttributeId == key && (value.IsEmpty() || x.Value == value));
        }
    }
}