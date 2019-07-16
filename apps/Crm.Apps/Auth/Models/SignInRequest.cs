using System.ComponentModel.DataAnnotations;

namespace Crm.Apps.Auth.Models
{
    public class SignInRequestModel
    {
        [Required, DataType(DataType.Text), StringLength(256)]
        public string Key { get; set; }

        [Required, DataType(DataType.Password), StringLength(256)]
        public string Password { get; set; }

        public bool IsRemember { get; set; }

        [DataType(DataType.Url), StringLength(2048)]
        public string RedirectUrl { get; set; }
    }
}