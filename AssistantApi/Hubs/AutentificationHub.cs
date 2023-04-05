using AssistantApi.Models;
using AssistantApi.services;
using Microsoft.AspNetCore.SignalR;
using MongoDB.Driver;

namespace AssistantApi.Hubs
{
    public class AutentificationHub : Hub
    {
        DBMongo dBMongo = new DBMongo();

        public void CreateUser(string id)
        {
            dBMongo.UpdateDataIfConnected(id, Context.ConnectionId);
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            dBMongo.UpdateDataIfDisconnected(Context.ConnectionId);
        }
    }
}
