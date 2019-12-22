using System;
using System.Collections.Generic;
using Crm.Common.Types;

namespace Crm.Apps.Areas.Contacts.Parameters
{
    public class ContactAttributeGetPagedListParameter
    {
        public Guid AccountId { get; set; }

        public List<AttributeType> Types { get; set; }

        public string Key { get; set; }

        public bool? IsDeleted { get; set; }

        public DateTime? MinCreateDate { get; set; }

        public DateTime? MaxCreateDate { get; set; }
        
        public DateTime? MinModifyDate { get; set; }

        public DateTime? MaxModifyDate { get; set; }

        public int Offset { get; set; }

        public int Limit { get; set; } = 10;

        public string SortBy { get; set; }

        public string OrderBy { get; set; }
    }
}