using System;
using System.ComponentModel.DataAnnotations;

namespace Crm.Apps.OAuth.Parameters
{
    public class OAuthClientChangeGetPagedListParameter
    {
        public OAuthClientChangeGetPagedListParameter(
            Guid accountId,
            Guid? changerUserId = default,
            DateTime? minCreateDate = default,
            DateTime? maxCreateDate = default,
            int offset = default,
            int limit = 10,
            string sortBy = "CreateDateTime",
            string orderBy = "desc")
        {
            AccountId = accountId;
            ChangerUserId = changerUserId;
            MinCreateDate = minCreateDate;
            MaxCreateDate = maxCreateDate;
            Offset = offset;
            Limit = limit;
            OrderBy = orderBy;
            SortBy = sortBy;
        }

        [Required] public Guid AccountId { get; set; }

        public Guid? ChangerUserId { get; set; }

        public DateTime? MinCreateDate { get; set; }

        public DateTime? MaxCreateDate { get; set; }

        public int Offset { get; set; }

        public int Limit { get; set; }

        public string SortBy { get; set; }

        public string OrderBy { get; set; }
    }
}