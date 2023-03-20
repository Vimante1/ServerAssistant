using AssistantApi.Models;
using AssistantApi.services;
using Microsoft.AspNetCore.SignalR;
using MongoDB.Driver;

namespace AssistantApi.Hubs
{
    public class AutentificationHub : Hub
    {

        public void SendCommands(string message)
        {
            Console.WriteLine($"{Context.ConnectionId}");
            Clients.All.SendAsync("Send", message);
        }

        public void CreateUser(string Time)
        {
            var Mongo = new DBMongo();
            ForDB forDB = new ForDB(Time, Context.ConnectionId);
            Mongo.Add(forDB);
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var Mongo = new DBMongo();
            await Mongo.DeleteForDesktopId(Context.ConnectionId);
        }


    }
}
