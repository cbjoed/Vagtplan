using MongoDB.Bson;
using MongoDB.Driver;
using Musikfestival.Repositories;
using Musikfestival.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Musikfestival.Repositories
{
    public class BrugerinfoRepository : IBruger
    {
        private const string connectionString = "mongodb+srv://test:124365@musikfestivalcluster.1xtpep0.mongodb.net/";

        MongoClient dbClient;
        IMongoDatabase database;
        IMongoCollection<BsonDocument> brugerKollektion;

        public BrugerinfoRepository()
        {
            dbClient = new MongoClient(connectionString);
            database = dbClient.GetDatabase("BrugerDB");
            brugerKollektion = database.GetCollection<BsonDocument>("bruger");
        }

        public void Update(Bruger bruger)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("username", bruger.Username);
            var update = Builders<BsonDocument>.Update

            .Set("navn", bruger.Navn)
            .Set("adresse", bruger.Adresse)
            .Set("email", bruger.Email)
            .Set("tlf", bruger.Tlf)
            .Set("beskrivelse", bruger.Beskrivelse);

            var updateResult = brugerKollektion.UpdateOne(filter, update);

            if (updateResult.ModifiedCount == 0)
            {
                Console.WriteLine("Opdatering kunne ikke gennemføres da værdien er nul.");
            }
        }

        public Bruger[] GetAllBrugere()
        {
            var result = brugerKollektion.Find(new BsonDocument()).ToList();

            List<Bruger> brugere = new List<Bruger>();

            foreach (var doc in result)
            {
                Bruger bruger = new Bruger()
                {
                    Id = doc.Contains("_id") ? doc["_id"].AsObjectId : ObjectId.Empty,
                    Username = doc.Contains("username") && doc["username"] != BsonNull.Value ? doc["username"].AsString : null,
                    Password = doc.Contains("password") && doc["password"] != BsonNull.Value ? doc["password"].AsString : null,
                    Type = doc.Contains("type") && doc["type"] != BsonNull.Value ? doc["type"].AsString : null,
                    Navn = doc.Contains("navn") && doc["navn"] != BsonNull.Value ? doc["navn"].AsString : null,
                    Adresse = doc.Contains("adresse") && doc["adresse"] != BsonNull.Value ? doc["adresse"].AsString : null,
                    Email = doc.Contains("email") && doc["email"] != BsonNull.Value ? doc["email"].AsString : null,
                    Tlf = doc.Contains("tlf") && doc["tlf"] != BsonNull.Value ? doc["tlf"].AsInt32 : 0,
                    Beskrivelse = doc.Contains("beskrivelse") && doc["beskrivelse"] != BsonNull.Value ? doc["beskrivelse"].AsString : null,

                };
                brugere.Add(bruger);

            }

            return brugere.ToArray();
        }
        public async Task<Bruger> AuthenticateUserAsync(string username, string password)
        {

            var result = await brugerKollektion.Find(
                Builders<BsonDocument>.Filter.Eq("username", username) &
                Builders<BsonDocument>.Filter.Eq("password", password))
                .FirstOrDefaultAsync();

            if (result != null)
            {
                Bruger authenticatedUser = new Bruger()
                {
                    Username = result.Contains("username") ? result["username"].AsString : null,
                    Password = result.Contains("password") ? result["password"].AsString : null,
                
                };

                return authenticatedUser;
            }

            return null; // No matching user found
        }
    }
}
