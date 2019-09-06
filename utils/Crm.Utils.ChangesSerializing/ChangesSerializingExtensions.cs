using System;
using Newtonsoft.Json;

namespace Crm.Utils.ChangesSerializing
{
    public static class ChangesSerializingExtensions
    {
        public static (string? OldValue, string? NewValue) CreateWithAudit<TModel>(this TModel? model)
            where TModel : class
        {
            var newValue = model == null ? null : JsonConvert.SerializeObject(model);

            return (null, newValue);
        }

        public static (string? OldValue, string? NewValue) UpdateWithAudit<TModel>(this TModel? model,
            Action<TModel> action) where TModel : class
        {
            var oldValue = model == null ? null : JsonConvert.SerializeObject(model);

            action(model);

            var newValue = model == null ? null : JsonConvert.SerializeObject(model);

            return (oldValue, newValue);
        }
    }
}