using System;

namespace Crm.Apps.Areas.Contacts.Models
{
    public class ContactBankAccount
    {
        public Guid Id { get; set; }

        public Guid ContactId { get; set; }

        public string Number { get; set; }

        public string BankNumber { get; set; }

        public string BankCorrespondentNumber { get; set; }

        public string BankName { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreateDateTime { get; set; }
    }
}