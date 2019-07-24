using System.ComponentModel.DataAnnotations;
using Crm.Apps.OAuth.Models;

namespace Crm.Apps.OAuth.Attributes
{
    public class ResponseTypeValidationAttribute : ValidationAttribute
    {
        private const string TranslationsKey = "OAuth:Errors:Validation";

        protected override ValidationResult IsValid(
            object value,
            ValidationContext validationContext)
        {
            var responseType = value?.ToString();
            if (responseType == AuthorizeResponseType.Code || responseType == AuthorizeResponseType.Token)
            {
                return new ValidationResult($"{TranslationsKey}_ResponseType:IsInvalid");
            }

            return ValidationResult.Success;
        }
    }
}