using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Crm.Common.Types;

namespace Crm.Apps.Areas.Activities.RequestParameters
{
    public class ActivityAttributeGetPagedListRequest
    {
        [Required]
        public Guid AccountId { get; set; }

        public List<AttributeType>? Types { get; set; }

        public string? Key { get; set; }

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