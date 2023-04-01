using AssistantApi.Models;
using Microsoft.VisualBasic;
using MongoDB.Bson;
using MongoDB.Driver;

namespace AssistantApi.services
{
    public class DBMongo
    {
        private readonly IMongoCollection<ForDB> _collection;

        public DBMongo()
        {
            var client = new MongoClient("mongodb://root:example@132.226.192.36:27017");
            var database = client.GetDatabase("users");
            _collection = database.GetCollection<ForDB>("user");
        }

        /// <summary>
        /// Метод для додавання в базу даних
        /// </summary>
        /// <param name="forDb"></param>
        public void Add(ForDB forDb)
        {
            _collection.InsertOne(forDb);
        }

        /// <summary>
        /// Перевірка чі підключений клієнта до серверу, та повернення ідентифікатора користувача для клієнта
        /// </summary>
        /// <param name="LastSeenTime"></param>
        /// <param name="desktopId"></param>
        /// <returns></returns>
        public bool ClientIsConnected(string Email, out string desktopId)
        {
            try
            {
                var user = _collection.Find(new BsonDocument("_id", Email)).First();
                desktopId = user.desktopId;
                return user.isConnected;
            }
            catch (Exception)
            {
                desktopId = null;
                return false;
            }
        }

        /// <summary>
        /// При підключенні клієнта в базі данних міняються хуйні короче
        /// </summary>
        /// <param name="id"></param>
        /// <param name="desktopId"></param>
        public bool UpdateDataIfConnected(string id, string desktopId)
        {
            try
            {
                var filter = new BsonDocument("_id", id);
                var updateSettings = new BsonDocument("$set", new BsonDocument { { "isConnected", true }, { "desktopId", desktopId } });
                _collection.UpdateOneAsync(filter, updateSettings);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// При відключенні клієнта в базі данних міняються хуйні короче
        /// </summary>
        /// <param name="id"></param>
        public void UpdateDataIfDisconnected(string desktopId)
        {
            var filter = new BsonDocument("desktopId", desktopId);
            var updateSettings = new BsonDocument("$set", new BsonDocument { { "isConnected", false }, { "desktopId", "" } });
            _collection.UpdateOneAsync(filter, updateSettings);
        }

    }
}