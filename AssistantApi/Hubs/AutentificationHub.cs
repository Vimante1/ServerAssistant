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
            if (!dBMongo.UpdateDataIfConnected(id, Context.ConnectionId))
            {
                Console.WriteLine("Хуйня короче при підключенні"); //TODO: Відправляти юзеру щось, щоб він охуїв і червоним підсвітилось
            }
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            dBMongo.UpdateDataIfDisconnected(Context.ConnectionId);
        }
    }
}
