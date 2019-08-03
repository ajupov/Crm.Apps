using System.ComponentModel.DataAnnotations;

namespace Identity.OAuth.Models.Registration
{
    public class RegisterRequestModel
    {
        [Required, DataType(DataType.EmailAddress), StringLength(256), EmailAddress]
        public string Email { get; set; }

        [Required, DataType(DataType.Text), StringLength(256)]
        public string Name { get; set; }

        [Required, StringLength(256), DataType(DataType.Password)]
        public string Password { get; set; }

        [Required, StringLength(256), DataType(DataType.Password), Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}