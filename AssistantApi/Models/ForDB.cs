using MongoDB.Bson.Serialization.Attributes;
using System;

namespace AssistantApi.Models
{
    public class ForDB
    {
        [BsonId]
        public string id{ get;} //this is user mail
        public string desktopId{ get; set; } = "";
        public bool isConnected { get; set; } = false;

        public ForDB(string id)
        {
            this.id = id;
        }
    }
}
