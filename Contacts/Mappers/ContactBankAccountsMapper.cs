using System;
using System.Collections.Generic;
using System.Linq;
using Crm.Apps.Contacts.Models;

namespace Crm.Apps.Contacts.Mappers
{
    public static class ContactBankAccountsMapper
    {
        public static List<ContactBankAccount> Map(this List<ContactBankAccount> accounts, Guid contactId)
        {
            return accounts?
                .Select(l => Map(l, contactId))
                .ToList();
        }

        public static ContactBankAccount Map(this ContactBankAccount account, Guid contactId)
        {
            return new ContactBankAccount
            {
                Id = account.Id,
                ContactId = contactId,
                Number = account.Number,
                BankNumber = account.BankNumber,
                BankCorrespondentNumber = account.BankCorrespondentNumber,
                BankName = account.BankName,
                IsDeleted = account.IsDeleted
            };
        }
    }
}