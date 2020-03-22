using System;
using Newtonsoft.Json;

namespace Crm.Apps.Contacts.Models
{
    public class ContactBankAccount
    {
        [JsonIgnore]
        public Guid Id { get; set; }

        [JsonIgnore]
        public Guid ContactId { get; set; }

        public string Number { get; set; }

        public string BankNumber { get; set; }

        public string BankCorrespondentNumber { get; set; }

        public string BankName { get; set; }

        public bool IsDeleted { get; set; }
    }
}