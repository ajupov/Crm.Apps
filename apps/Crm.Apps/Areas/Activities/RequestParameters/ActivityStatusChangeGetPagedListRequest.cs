using System;
using System.ComponentModel.DataAnnotations;

namespace Crm.Apps.Areas.Activities.RequestParameters
{
    public class ActivityStatusChangeGetPagedListRequest
    {
        [Required]
        public Guid StatusId { get; set; }

        public Guid? ChangerUserId { get; set; }

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