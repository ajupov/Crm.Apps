using System.ComponentModel.DataAnnotations;
using Crm.Utils.String;

namespace Crm.Apps.OAuth.Attributes
{
    public class TokenValidationAttribute : ValidationAttribute
    {
        private const string TranslationsKey = "OAuth:Errors:Validation";

        protected override ValidationResult IsValid(
            object value,
            ValidationContext validationContext)
        {
            if ((value?.ToString()).IsEmpty())
            {
                return new ValidationResult($"{TranslationsKey}_Token:IsEmpty");
            }

            return ValidationResult.Success;
        }
    }
}