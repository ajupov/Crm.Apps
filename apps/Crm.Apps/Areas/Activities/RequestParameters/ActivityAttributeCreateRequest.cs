using System;
using System.ComponentModel.DataAnnotations;
using Crm.Common.Types;

namespace Crm.Apps.Areas.Activities.RequestParameters
{
    public class ActivityAttributeCreateRequest
    {
        [Required]
        public Guid AccountId { get; set; }

        [Required]
        public AttributeType Type { get; set; }

        [Required]
        public string Key { get; set; }

        public bool IsDeleted { get; set; }
    }
}