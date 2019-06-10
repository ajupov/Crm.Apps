using System;

namespace Crm.Apps.Identities.Parameters
{
    public class IdentityChangeGetPagedListParameter
    {
        public Guid? ChangerUserId { get; set; }

        public Guid? IdentityId { get; set; }

        public DateTime? MinCreateDate { get; set; }

        public DateTime? MaxCreateDate { get; set; }

        public int Offset { get; set; }

        public int Limit { get; set; } = 10;

        public string SortBy { get; set; }

        public string OrderBy { get; set; }
    }
}