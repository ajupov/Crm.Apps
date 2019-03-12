using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace Crm.Utils.Http
{
    public static class HttpExtension
    {
        public static string ToQueryParams(this object parameters)
        {
            var result = TypeDescriptor.GetProperties(parameters)
                .Cast<PropertyDescriptor>()
                .Select(p => $"{p.Name}={p.GetValue(parameters)}")
                .ToList();

            return result.Any() ? $"?{string.Join("&", result)}" : string.Empty;
        }

        public static StringContent ToStringContent(this object model)
        {
            return new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
        }
    }
}