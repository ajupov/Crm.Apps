using System.ComponentModel.DataAnnotations;
using Crm.Utils.String;

namespace Identity.OAuth.Attributes.Validation
{
    public class StateValidationAttribute : ValidationAttribute
    {
        private const string TranslationsKey = "OAuth:Errors:Validation";

        private const int StateLength = 8;

        protected override ValidationResult IsValid(
            object value,
            ValidationContext validationContext)
        {
            var stateString = value?.ToString();
            if (stateString.IsEmpty())
            {
                return new ValidationResult($"{TranslationsKey}_State:IsEmpty");
            }

            if (stateString?.Length != StateLength)
            {
                return new ValidationResult($"{TranslationsKey}_StateLength:Invalid");
            }

            return ValidationResult.Success;
        }
    }
}