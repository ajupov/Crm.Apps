using System;
using System.ComponentModel.DataAnnotations;

namespace Crm.Apps.Accounts.RequestParameters
{
    public class AccountChangeGetPagedListRequest
    {
        [Required]
        public Guid AccountId { get; set; }

        public Guid? ChangerUserId { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? MinCreateDate { get; set; } = DateTime.UtcNow.AddDays(-1);

        [DataType(DataType.DateTime)]
        public DateTime? MaxCreateDate { get; set; } = DateTime.UtcNow;

        public int Offset { get; set; } = 0;

        public int Limit { get; set; } = 10;

        public string? SortBy { get; set; } = "CreateDateTime";

        public string? OrderBy { get; set; } = "desc";
    }
}