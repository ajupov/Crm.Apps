using System.ComponentModel.DataAnnotations;
using Identity.OAuth.Attributes.Validation;

namespace Identity.OAuth.Models.Authorize
{
    public class PostAuthorizeRequest
    {
        [Required]
        [StringLength(256)]
        [DataType(DataType.Text)]
        public string Key { get; set; }

        [Required]
        [StringLength(256)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public bool IsRemember { get; set; }

        [Required]
        [ResponseTypeValidation]
        public string ResponseType { get; set; }

        [RedirectUriWithStateValidation]
        public string RedirectUri { get; set; }
    }
}