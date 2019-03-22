using System.Linq;

namespace Crm.Utils.Phone
{
    public static class PhoneExtension
    {
        public static string ExtractPhone(this string value, int maxLength = 10)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            var result = new string(value.ToCharArray().Where(char.IsDigit).ToArray());

            return result.Substring(result.Length - maxLength);
        }
    }
}