using System;

namespace Crm.Infrastructure.HotStorage.HotStorage
{
    public interface IHotStorage
    {
        void SetValue<T>(
            string key,
            T value,
            TimeSpan timeSpan);

        void IsExist(
            string key);

        T GetValue<T>(
            string key);
    }
}