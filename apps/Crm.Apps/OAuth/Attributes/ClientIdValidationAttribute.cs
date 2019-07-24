using System.ComponentModel.DataAnnotations;
using Crm.Utils.String;

namespace Crm.Apps.OAuth.Attributes
{
    public class ClientIdValidationAttribute : ValidationAttribute
    {
        private const string TranslationsKey = "OAuth:Errors:Validation";

        protected override ValidationResult IsValid(
            object value,
            ValidationContext validationContext)
        {
            var clientIdString = value?.ToString();
            if (clientIdString.IsEmpty() || int.TryParse(clientIdString, out var clientId) || clientId <= 0)
            {
                return new ValidationResult($"{TranslationsKey}_ClientId:IsEmpty");
            }

            return ValidationResult.Success;
        }
    }
}