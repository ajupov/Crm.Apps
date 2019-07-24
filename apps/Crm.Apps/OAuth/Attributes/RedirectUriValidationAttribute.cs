using System.ComponentModel.DataAnnotations;
using Crm.Utils.String;

namespace Crm.Apps.OAuth.Attributes
{
    public class RedirectUriValidationAttribute : ValidationAttribute
    {
        private const string TranslationsKey = "OAuth:Errors:Validation";

        private const int MaxUriLength = 2048;

        protected override ValidationResult IsValid(
            object value,
            ValidationContext validationContext)
        {
            var redirectUri = value?.ToString();
            if (redirectUri.IsEmpty())
            {
                return new ValidationResult($"{TranslationsKey}_RedirectUri:IsEmpty");
            }

            if (redirectUri?.Length > MaxUriLength)
            {
                return new ValidationResult($"{TranslationsKey}_RedirectUri:Invalid");
            }

            return ValidationResult.Success;
        }
    }
}