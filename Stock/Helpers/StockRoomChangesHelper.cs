using System;
using Ajupov.Utils.All.Json;
using Crm.Apps.Stock.Models;

namespace Crm.Apps.Stock.Helpers
{
    public static class StockRoomChangesHelper
    {
        public static StockRoomChange CreateWithLog(this StockRoom room, Guid userId, Action<StockRoom> action)
        {
            action(room);

            return new StockRoomChange
            {
                StockRoomId = room.Id,
                ChangerUserId = userId,
                CreateDateTime = DateTime.UtcNow,
                OldValueJson = string.Empty,
                NewValueJson = room.ToJsonString()
            };
        }

        public static StockRoomChange UpdateWithLog(this StockRoom room, Guid userId, Action<StockRoom> action)
        {
            var oldValueJson = room.ToJsonString();

            action(room);

            return new StockRoomChange
            {
                StockRoomId = room.Id,
                ChangerUserId = userId,
                CreateDateTime = DateTime.UtcNow,
                OldValueJson = oldValueJson,
                NewValueJson = room.ToJsonString()
            };
        }
    }
}
