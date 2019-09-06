using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Crm.Apps.Activities.Models;

namespace Crm.Apps.Activities.RequestParameters
{
    public class ActivityGetPagedListRequest
    {
        [Required]
        public Guid AccountId { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public string? Result { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? MinStartDateTime { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? MaxStartDateTime { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? MinEndDateTime { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? MaxEndDateTime { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? MinDeadLineDateTime { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? MaxDeadLineDateTime { get; set; }

        public bool? IsDeleted { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? MinCreateDate { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? MaxCreateDate { get; set; }

        public List<Guid>? TypeIds { get; set; }

        public List<Guid>? StatusIds { get; set; }

        public List<Guid>? LeadIds { get; set; }

        public List<Guid>? CompanyIds { get; set; }

        public List<Guid>? ContactIds { get; set; }

        public List<Guid>? DealIds { get; set; }

        public List<Guid>? CreateUserIds { get; set; }

        public List<Guid>? ResponsibleUserIds { get; set; }

        public List<ActivityPriority>? Priorities { get; set; }

        public bool? AllAttributes { get; set; }

        public IDictionary<Guid, string>? Attributes { get; set; }

        public int Offset { get; set; }

        public int Limit { get; set; }

        public string? SortBy { get; set; }

        public string? OrderBy { get; set; }
    }
}