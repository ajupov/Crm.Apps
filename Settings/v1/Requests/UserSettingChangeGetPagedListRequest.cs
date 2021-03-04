using System.ComponentModel.DataAnnotations;
using Crm.Apps.Settings.Models;

namespace Crm.Apps.Settings.V1.Requests
{
    public class UserSettingChangeGetPagedListRequest
    {
        [Required]
        public UserSettingType Type { get; set; }

        public int Offset { get; set; }

        public int Limit { get; set; } = 10;

        public string SortBy { get; set; } = "CreateDateTime";

        public string OrderBy { get; set; } = "desc";
    }
}
