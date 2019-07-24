using System.ComponentModel.DataAnnotations;
using Crm.Apps.OAuth.Models;

namespace Crm.Apps.OAuth.Attributes
{
    public class TokenTypeValidationAttribute : ValidationAttribute
    {
        private const string TranslationsKey = "OAuth:Errors:Validation";

        protected override ValidationResult IsValid(
            object value,
            ValidationContext validationContext)
        {
            var tokenType = value?.ToString();
            if (tokenType == AuthorizeTokenType.Bearer)
            {
                return new ValidationResult($"{TranslationsKey}_TokenType:IsInvalid");
            }

            return ValidationResult.Success;
        }
    }
}