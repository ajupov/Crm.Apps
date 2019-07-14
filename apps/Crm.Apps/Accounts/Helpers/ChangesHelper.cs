using System;
using Crm.Utils.Json;

namespace Crm.Apps.Accounts.Helpers
{
    public static class ChangesHelper
    {
        public static (string OldValue, string NewValue) CreateWithAudit<TModel>(
            this TModel model)
        {
            var newValue = model == null ? null : model.ToJsonString();

            return (null, newValue);
        }
        
        public static (string OldValue, string NewValue) UpdateWithAudit<TModel>(
            this TModel model,
            Action<TModel> action)
        {
            var oldValue = model == null ? null : model.ToJsonString();

            action(model);

            var newValue = model == null ? null : model.ToJsonString();

            return (oldValue, newValue);
        }
    }
}