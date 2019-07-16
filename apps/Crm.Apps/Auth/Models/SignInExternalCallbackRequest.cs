using System.ComponentModel.DataAnnotations;

namespace Crm.Apps.Auth.Models
{
    public class SignInExternalCallbackRequest
    {
        [Required, DataType(DataType.Text), StringLength(8)]
        public string State { get; set; }

        [Required, DataType(DataType.Text), StringLength(20)]
        public string Nonce { get; set; }

        [DataType(DataType.Url), StringLength(2048)]
        public string RedirectUrl { get; set; }
    }
}