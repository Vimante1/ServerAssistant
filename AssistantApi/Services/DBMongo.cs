using AssistantApi.Models;
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
        // Метод для додавання об'єкту в базу даних
        public void Add(ForDB forDb)
        {
            _collection.InsertOne(forDb);
        }
        public bool CheckForParametrDesktopId(string forDb)
        {
            var filter = Builders<ForDB>.Filter.Eq("DesktopId", forDb);
            var result = _collection.Find(filter).Any();
            return result ? true : false;
        }


        public string GetDesktopIdByLastSeenTime (string lastSeenTime, string Time)
        {
            // Створення фільтра для пошуку документів за другим параметром
            var filter = Builders<ForDB>.Filter.Eq("lastSeenTime", lastSeenTime);

            // Отримання документа з колекції за фільтром
            var document = _collection.Find(filter).FirstOrDefault();

            // Перевірка наявності документа та отримання значення параметра
            if (document != null)
            {

                _collection.ReplaceOne(filter, new ForDB(Time, document.desktopId));
                return document.desktopId;
            }
            else
            {
                return null; // Повернення null, якщо документ не знайдено
            }

            
        }

        public bool replace (ForDB forDb )
        {


            return true;
        }
        
        public async Task DeleteForDesktopId(string forDB)
        {
            var filter = Builders<ForDB>.Filter.Eq("desktopId", forDB);
            _collection.DeleteMany(filter);
        }



    }


}
