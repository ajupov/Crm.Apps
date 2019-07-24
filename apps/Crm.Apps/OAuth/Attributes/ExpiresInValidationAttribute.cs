using System.ComponentModel.DataAnnotations;
using Crm.Utils.String;

namespace Crm.Apps.OAuth.Attributes
{
    public class ExpiresInValidationAttribute : ValidationAttribute
    {
        private const string TranslationsKey = "OAuth:Errors:Validation";

        protected override ValidationResult IsValid(
            object value,
            ValidationContext validationContext)
        {
            var expiresInString = value?.ToString();
            if (expiresInString.IsEmpty() || int.TryParse(expiresInString, out var expiresIn) || expiresIn <= 0)
            {
                return new ValidationResult($"{TranslationsKey}_ExpiresIn:IsEmpty");
            }

            return ValidationResult.Success;
        }
    }
}