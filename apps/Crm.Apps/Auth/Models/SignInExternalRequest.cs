using System.ComponentModel.DataAnnotations;

namespace Crm.Apps.Auth.Models
{
    public class SignInExternalRequest
    {
        [Required, DataType(DataType.Text), StringLength(256)]
        public string Provider { get; set; }

        [DataType(DataType.Url), StringLength(2048)]
        public string RedirectUrl { get; set; }
    }
}