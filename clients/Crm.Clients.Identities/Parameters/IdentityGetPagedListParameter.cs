using System;
using Crm.Clients.Identities.Models;

namespace Crm.Clients.Identities.Parameters
{
    public class IdentityGetPagedListParameter
    {
        public IdentityGetPagedListParameter(
            Guid? userId = default,
            IdentityType[] types = default,
            bool? isPrimary = default,
            bool? isVerified = default,
            DateTime? minCreateDate = default,
            DateTime? maxCreateDate = default,
            int offset = default,
            int limit = 10,
            string sortBy = "CreateDateTime",
            string orderBy = "desc")
        {
            UserId = userId;
            Types = types;
            IsPrimary = isPrimary;
            IsVerified = isVerified;
            MinCreateDate = minCreateDate;
            MaxCreateDate = maxCreateDate;
            Offset = offset;
            Limit = limit;
            SortBy = sortBy;
            OrderBy = orderBy;
        }

        public Guid? UserId { get; }

        public IdentityType[] Types { get; }

        public bool? IsPrimary { get; }

        public bool? IsVerified { get; }

        public DateTime? MinCreateDate { get; }

        public DateTime? MaxCreateDate { get; }

        public int Offset { get; }

        public int Limit { get; }

        public string SortBy { get; }

        public string OrderBy { get; }
    }
}