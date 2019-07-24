using System.ComponentModel.DataAnnotations;
using Crm.Apps.OAuth.Models;

namespace Crm.Apps.OAuth.Attributes
{
    public class GrandTypeValidationAttribute : ValidationAttribute
    {
        private const string TranslationsKey = "OAuth:Errors:Validation";

        protected override ValidationResult IsValid(
            object value,
            ValidationContext validationContext)
        {
            var grandType = value?.ToString();
            if (grandType == AuthorizeGrandType.AuthorizationCode || 
                grandType == AuthorizeGrandType.Password ||
                grandType == AuthorizeGrandType.RefreshToken)
            {
                return new ValidationResult($"{TranslationsKey}_GrandType:IsInvalid");
            }

            return ValidationResult.Success;
        }
    }
}