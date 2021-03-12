using System;
using System.Text.Json.Serialization;

namespace Crm.Apps.Companies.Models
{
    public class CompanyBankAccount
    {
        [JsonIgnore]
        public Guid Id { get; set; }

        [JsonIgnore]
        public Guid CompanyId { get; set; }

        public string Number { get; set; }

        public string BankNumber { get; set; }

        public string BankCorrespondentNumber { get; set; }

        public string BankName { get; set; }

        public bool IsDeleted { get; set; }
    }
}
