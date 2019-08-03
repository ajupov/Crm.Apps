using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Crm.Utils.String;

namespace Identity.OAuth.Attributes.Validation
{
    public class RedirectUriWithStateValidationAttribute : RedirectUriValidationAttribute
    {
        private const string TranslationsKey = "OAuth:Errors:Validation";

        private const int StateLength = 8;

        protected override ValidationResult IsValid(
            object value,
            ValidationContext validationContext)
        {
            var baseValidationResult = base.IsValid(value, validationContext);

            if (baseValidationResult != ValidationResult.Success)
            {
                return baseValidationResult;
            }

            var redirectUri = value?.ToString();
            var state = new Uri(redirectUri).Query.Split('&', '?').FirstOrDefault(x => x.Contains("state"));
            if (state.IsEmpty())
            {
                return new ValidationResult($"{TranslationsKey}_State:IsEmpty");
            }

            if (state?.Length != StateLength)
            {
                return new ValidationResult($"{TranslationsKey}_StateLength:Invalid");
            }

            return ValidationResult.Success;
        }
    }
}