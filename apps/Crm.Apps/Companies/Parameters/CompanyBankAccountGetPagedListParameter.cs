using System;

namespace Crm.Apps.Companies.Parameters
{
    public class CompanyBankAccountGetPagedListParameter
    {
        public Guid CompanyId { get; set; }

        public string Number { get; set; }

        public string BankNumber { get; set; }

        public string BankCorrespondentNumber { get; set; }

        public string BankName { get; set; }

        public DateTime? MinCreateDate { get; set; }

        public DateTime? MaxCreateDate { get; set; }

        public int Offset { get; set; }

        public int Limit { get; set; } = 10;

        public string SortBy { get; set; }

        public string OrderBy { get; set; }
    }
}