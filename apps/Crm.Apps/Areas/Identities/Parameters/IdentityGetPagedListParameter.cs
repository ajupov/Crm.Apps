using System;
using System.Collections.Generic;
using Crm.Apps.Areas.Identities.Models;

namespace Crm.Apps.Areas.Identities.Parameters
{
    public class IdentityGetPagedListParameter
    {
        public Guid? UserId { get; set; }

        public List<IdentityType> Types { get; set; }

        public bool? IsPrimary { get; set; }

        public bool? IsVerified { get; set; }

        public DateTime? MinCreateDate { get; set; }

        public DateTime? MaxCreateDate { get; set; }

        public int Offset { get; set; }

        public int Limit { get; set; } = 10;

        public string SortBy { get; set; }

        public string OrderBy { get; set; }
    }
}