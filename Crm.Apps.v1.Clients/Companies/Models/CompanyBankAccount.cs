using System;

namespace Crm.Apps.v1.Clients.Companies.Models
{
    public class CompanyBankAccount
    {
        public Guid Id { get; set; }

        public Guid CompanyId { get; set; }

        public string Number { get; set; }

        public string BankNumber { get; set; }

        public string BankCorrespondentNumber { get; set; }

        public string BankName { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreateDateTime { get; set; }

        public DateTime? ModifyDateTime { get; set; }
    }
}