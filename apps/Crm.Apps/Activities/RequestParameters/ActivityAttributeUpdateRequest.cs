using System;
using System.ComponentModel.DataAnnotations;
using Crm.Common.Types;

namespace Crm.Apps.Activities.RequestParameters
{
    public class ActivityAttributeUpdateRequest
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public Guid AccountId { get; set; }

        [Required]
        public AttributeType Type { get; set; }

        [Required]
        public string Key { get; set; }

        [Required]
        public bool IsDeleted { get; set; }
    }
}