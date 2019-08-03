using System;
using Identity.Identities.Models;

namespace Identity.Identities.Parameters
{
    public class IdentityGetPagedListParameter
    {
        public IdentityGetPagedListParameter(
            Guid userId,
            IdentityType[] types = default,
            bool? isPrimary = default,
            bool? isVerified = true,
            DateTime? minCreateDateTime = default,
            DateTime? maxCreateDateTime = default,
            int offset = default,
            int limit = 10,
            string sortBy = "CreateDateTime",
            string orderBy = "desc")
        {
            UserId = userId;
            Types = types;
            IsPrimary = isPrimary;
            IsVerified = isVerified;
            MinCreateDateTime = minCreateDateTime;
            MaxCreateDateTime = maxCreateDateTime;
            Offset = offset;
            Limit = limit;
            SortBy = sortBy;
            OrderBy = orderBy;
        }

        public Guid? UserId { get; set; }

        public IdentityType[] Types { get; set; }

        public bool? IsPrimary { get; set; }

        public bool? IsVerified { get; set; }

        public DateTime? MinCreateDateTime { get; set; }

        public DateTime? MaxCreateDateTime { get; set; }

        public int Offset { get; set; }

        public int Limit { get; set; }

        public string SortBy { get; set; }

        public string OrderBy { get; set; }
    }
}