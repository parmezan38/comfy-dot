using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdServer.Models;
using Microsoft.AspNetCore.SignalR;

namespace IdServer.Hubs
{
    public class ChatHub: Hub
    {
        private static ConcurrentDictionary<int, RoomConnectionInfo> RoomConnections = new ConcurrentDictionary<int, RoomConnectionInfo>();
        public async Task Test()
        {
            await Clients.All.SendAsync("Test");
        }

        public async Task Join(ConnectionData data)
        {
            string connectionId = Context.ConnectionId;
            if (!RoomConnections.ContainsKey(data.Room.Id))
            {
                RoomConnections.TryAdd(data.Room.Id, new RoomConnectionInfo()
                {
                    Capacity = data.Room.Capacity,
                    ConnectionIds = new ConcurrentDictionary<string, string>()
                });
            }
            var roomConnection = RoomConnections[data.Room.Id];
            if (!roomConnection.ConnectionIds.ContainsKey(data.UserId))
            {
                roomConnection.ConnectionIds.TryAdd(data.UserId, connectionId);
                data.Room.NumberOfUsers = roomConnection.ConnectionIds.Count;
            }
            roomConnection.ConnectionIds[data.UserId] = connectionId;
            await Groups.AddToGroupAsync(connectionId, data.Room.Id.ToString());
            await Clients.All.SendAsync("UpdateRoom", data.Room);
        }

        public async Task Leave(string userId)
        {
            foreach (KeyValuePair<int, RoomConnectionInfo> entry in RoomConnections)
            {
                if (entry.Value.ConnectionIds.ContainsKey(userId))
                {
                    await Groups.RemoveFromGroupAsync(entry.Value.ConnectionIds[userId], entry.Key.ToString());
                    entry.Value.ConnectionIds.Remove(userId, out _);
                    RoomDisplayable room = new RoomDisplayable()
                    {
                        Id = entry.Key,
                        Capacity = entry.Value.Capacity,
                        NumberOfUsers = entry.Value.ConnectionIds.Count
                    };
                    await Clients.All.SendAsync("UpdateRoom", room);
                }
            }
        }

        public async Task Chat(ChatMessage msg)
        {
            await Clients.Group(msg.RoomId.ToString()).SendAsync("Chat", msg);
        }
    }
}
