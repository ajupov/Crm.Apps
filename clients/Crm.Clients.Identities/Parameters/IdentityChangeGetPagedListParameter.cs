using System;

namespace Crm.Clients.Identities.Parameters
{
    public class IdentityChangeGetPagedListParameter
    {
        public IdentityChangeGetPagedListParameter(
            Guid identityId,
            Guid? changerUserId = default,
            DateTime? minCreateDate = default,
            DateTime? maxCreateDate = default,
            int offset = default,
            int limit = 10,
            string sortBy = "CreateDateTime",
            string orderBy = "desc")
        {
            ChangerUserId = changerUserId;
            IdentityId = identityId;
            MinCreateDate = minCreateDate;
            MaxCreateDate = maxCreateDate;
            Offset = offset;
            Limit = limit;
            SortBy = sortBy;
            OrderBy = orderBy;
        }

        public Guid? ChangerUserId { get; }

        public Guid? IdentityId { get; }

        public DateTime? MinCreateDate { get; }

        public DateTime? MaxCreateDate { get; }

        public int Offset { get; }

        public int Limit { get; }

        public string SortBy { get; }

        public string OrderBy { get; }
    }
}