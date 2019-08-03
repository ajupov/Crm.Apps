using System.ComponentModel.DataAnnotations;
using Crm.Utils.String;

namespace Identity.OAuth.Attributes.Validation
{
    public class ClientSecretValidationAttribute : ValidationAttribute
    {
        private const string TranslationsKey = "OAuth:Errors:Validation";

        protected override ValidationResult IsValid(
            object value,
            ValidationContext validationContext)
        {
            if ((value?.ToString()).IsEmpty())
            {
                return new ValidationResult($"{TranslationsKey}_ClientSecret:IsEmpty");
            }

            return ValidationResult.Success;
        }
    }
}