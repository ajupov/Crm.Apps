using System;

namespace Crm.Apps.Activities.Parameters
{
    public class ActivityChangeGetPagedListParameter
    {
        public Guid? ChangerUserId { get; set; }

        public Guid? ActivityId { get; set; }

        public DateTime? MinCreateDate { get; set; }

        public DateTime? MaxCreateDate { get; set; }

        public int Offset { get; set; }

        public int Limit { get; set; } = 10;

        public string SortBy { get; set; }

        public string OrderBy { get; set; }
    }
}