using System.ComponentModel.DataAnnotations;
using Crm.Utils.String;

namespace Identity.OAuth.Attributes.Validation
{
    public class ClientIdValidationAttribute : ValidationAttribute
    {
        private const string TranslationsKey = "OAuth:Errors:Validation";

        protected override ValidationResult IsValid(
            object value,
            ValidationContext validationContext)
        {
            var clientIdString = value?.ToString();
            if (clientIdString.IsEmpty())
            {
                return new ValidationResult($"{TranslationsKey}_ClientId:IsEmpty");
            }

            return ValidationResult.Success;
        }
    }
}