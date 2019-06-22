using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using Crm.Utils.Json;

namespace Crm.Utils.Http
{
    public static class HttpExtensions
    {
        public static string ToQueryParams(this object parameters)
        {
            var properties = TypeDescriptor.GetProperties(parameters);
            var result = new List<string>();

            foreach (PropertyDescriptor property in properties)
            {
                var value = property.GetValue(parameters);
                if (value is IEnumerable enumerable && !(value is string))
                {
                    var items = enumerable.Cast<object>().Select(x => x.ToString());

                    result.AddRange(items.Select(x => $"{property.Name}={x}"));
                }
                else
                {
                    result.Add($"{property.Name}={value}");
                }
            }

            return result.Any() ? $"?{string.Join("&", result)}" : string.Empty;
        }

        public static StringContent ToJsonStringContent(this object model)
        {
            return new StringContent(model.ToJsonString(), Encoding.UTF8, "application/json");
        }

        public static bool IsCollectionType(Type type)
        {
            return type.GetInterface(nameof(IEnumerable)) != null;
        }
    }
}