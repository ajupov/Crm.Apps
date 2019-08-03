using System;

namespace Identity.Clients.Parameters
{
    public class ClientGetPagedListParameter
    {
        public ClientGetPagedListParameter(
            bool? isLocked = false,
            bool? isDeleted = false,
            DateTime? minCreateDate = default,
            DateTime? maxCreateDate = default,
            int offset = default,
            int limit = 10,
            string sortBy = "CreateDateTime",
            string orderBy = "desc")
        {
            IsLocked = isLocked;
            IsDeleted = isDeleted;
            MinCreateDate = minCreateDate;
            MaxCreateDate = maxCreateDate;
            Offset = offset;
            Limit = limit;
            OrderBy = orderBy;
            SortBy = sortBy;
        }

        public bool? IsLocked { get; }

        public bool? IsDeleted { get; }

        public DateTime? MinCreateDate { get; }

        public DateTime? MaxCreateDate { get; }

        public int Offset { get; }

        public int Limit { get; }

        public string SortBy { get; }

        public string OrderBy { get; }
    }
}