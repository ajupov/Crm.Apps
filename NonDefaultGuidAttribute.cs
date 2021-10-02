using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Crm.Apps
{
    public class NonDefaultGuidAttribute : ValidationAttribute
    {
        private const string DefaultErrorMessage = "'{0}' does not contain a valid guid";

        public NonDefaultGuidAttribute()
            : base(DefaultErrorMessage)
        {
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var input = Convert.ToString(value, CultureInfo.CurrentCulture);
            if (string.IsNullOrWhiteSpace(input))
            {
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            }

            if (!Guid.TryParse(input, out var guid))
            {
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            }

            return guid == Guid.Empty
                ? new ValidationResult(FormatErrorMessage(validationContext.DisplayName))
                : ValidationResult.Success;
        }
    }
}
