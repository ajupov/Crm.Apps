using System;
using System.Collections.Generic;
using System.Linq;
using Crm.Apps.Areas.Contacts.Models;
using Crm.Apps.Areas.Contacts.Parameters;
using Microsoft.EntityFrameworkCore;

namespace Crm.Apps.Areas.Contacts.Helpers
{
    public static class ContactsFiltersHelper
    {
        public static bool FilterByAdditional(this Contact contact, ContactGetPagedListParameter parameter)
        {
            return
                (parameter.BankAccountNumber.IsEmpty() ||
                 contact.BankAccounts.Any(x => x.BankNumber == parameter.BankAccountNumber)) &&
                (parameter.BankAccountBankNumber.IsEmpty() ||
                 contact.BankAccounts.Any(x => x.BankNumber == parameter.BankAccountBankNumber)) &&
                (parameter.BankAccountBankCorrespondentNumber.IsEmpty() || contact.BankAccounts.Any(x =>
                     x.BankCorrespondentNumber == parameter.BankAccountBankCorrespondentNumber)) &&
                (parameter.BankAccountBankName.IsEmpty() ||
                 contact.BankAccounts.Any(x => EF.Functions.Like(x.BankName, $"{parameter.BankAccountBankName}%"))) &&
                (parameter.LeadIds == null || !parameter.LeadIds.Any() ||
                 parameter.LeadIds.Any(x => LeadIdsPredicate(contact, x))) &&
                (parameter.CompanyIds == null || !parameter.CompanyIds.Any() ||
                 parameter.CompanyIds.Any(x => CompanyIdsPredicate(contact, x))) &&
                (parameter.CreateUserIds == null || !parameter.CreateUserIds.Any() ||
                 parameter.CreateUserIds.Any(x => CreateUserIdsPredicate(contact, x))) &&
                (parameter.ResponsibleUserIds == null || !parameter.ResponsibleUserIds.Any() ||
                 parameter.ResponsibleUserIds.Any(x => ResponsibleUserIdsPredicate(contact, x))) &&
                (parameter.Attributes == null || !parameter.Attributes.Any() ||
                 (parameter.AllAttributes is false
                     ? parameter.Attributes.Any(x => AttributePredicate(contact, x))
                     : parameter.Attributes.All(x => AttributePredicate(contact, x))));
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