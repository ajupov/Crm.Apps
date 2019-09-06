using System;
using System.ComponentModel.DataAnnotations;

namespace Crm.Apps.Activities.RequestParameters
{
    public class ActivityStatusGetPagedListRequest
    {
        [Required]
        public Guid AccountId { get; set; }

        public string? Name { get; set; }

        public bool? IsFinish { get; set; }

        public bool? IsDeleted { get; set; }
        
        [DataType(DataType.DateTime)]
        public DateTime? MinCreateDate { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? MaxCreateDate { get; set; }

        public int Offset { get; set; }

        public int Limit { get; set; }

        public string? SortBy { get; set; }

        public string? OrderBy { get; set; }
    }
}