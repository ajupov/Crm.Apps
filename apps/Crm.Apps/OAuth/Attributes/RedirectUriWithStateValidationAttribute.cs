using System;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using Crm.Utils.String;

namespace Crm.Apps.OAuth.Attributes
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
            var state = new Uri(redirectUri).ParseQueryString().Get("state");
            if (state.IsEmpty())
            {
                return new ValidationResult($"{TranslationsKey}_State:IsEmpty");
            }

            if (state.Length != StateLength)
            {
                return new ValidationResult($"{TranslationsKey}_StateLength:Invalid");
            }

            return ValidationResult.Success;
        }
    }
}