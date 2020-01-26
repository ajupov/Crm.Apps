using System;
using System.Collections.Generic;
using System.Linq;
using Ajupov.Utils.All.Guid;
using Crm.Apps.Contacts.v1.Models;

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
            var isNew = account.Id.IsEmpty();

            return new ContactBankAccount
            {
                Id = isNew ? Guid.NewGuid() : account.Id,
                ContactId = contactId,
                Number = account.Number,
                BankNumber = account.BankNumber,
                BankCorrespondentNumber = account.BankCorrespondentNumber,
                BankName = account.BankName,
                IsDeleted = account.IsDeleted,
                CreateDateTime = isNew ? DateTime.UtcNow : account.CreateDateTime,
                ModifyDateTime = isNew ? (DateTime?) null : DateTime.UtcNow
            };
        }
    }
}