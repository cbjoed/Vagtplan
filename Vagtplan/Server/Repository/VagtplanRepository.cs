using MongoDB.Bson;
using MongoDB.Driver;
using Musikfestival.Repositories;
using Musikfestival.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Musikfestival.Repositories
{
    public class VagtplanRepository : IVagter
    {
        private const string connectionString = "mongodb+srv://test:124365@musikfestivalcluster.1xtpep0.mongodb.net/";

        MongoClient dbClient;
        IMongoDatabase database;
        IMongoCollection<BsonDocument> vagterKollektion;
        IMongoCollection<BsonDocument> fordeling;

        public VagtplanRepository()
        {
            dbClient = new MongoClient(connectionString);
            database = dbClient.GetDatabase("VagterDB");
            vagterKollektion = database.GetCollection<BsonDocument>("vagter");
            fordeling = database.GetCollection<BsonDocument>("fordeling");
        }

        public void AddVagt(Vagter vagter)
        {
            var existingUsername = fordeling.Find(Builders<BsonDocument>.Filter.Eq("username", vagter.Username)).FirstOrDefault();

            if (existingUsername != null)
            {
                // Håndter tilfælde, hvor der allerede er en vagt med det samme username
                // Du kan kaste en exception eller håndtere det på anden måde afhængigt af din logik
                // Eksempel:
                throw new InvalidOperationException($"Kunne ikke oprette vagt. Der er allerede en vagt med username: {vagter.Username}");
            }

            var vagtFull = vagterKollektion.Find(Builders<BsonDocument>.Filter.Eq("vagtId", vagter.VagtId)).FirstOrDefault();

            if (vagtFull != null)
            {
                int antal = vagtFull.Contains("antal") ? vagtFull["antal"].AsInt32 : 0;

                if (antal <= 0)
                {
                    // Håndter tilfælde, hvor "antal" er 0
                    // Du kan kaste en exception eller håndtere det på anden måde afhængigt af din logik
                    // Eksempel:
                    throw new InvalidOperationException($"Kunne ikke oprette vagt. Antal er 0 for vagt med id: {vagter.VagtId}");
                }
            }

            BsonDocument vagterDocument = new BsonDocument
            {
                { "vagtId", vagter.VagtId },
                { "username", vagter.Username },
            };

            var filter = Builders<BsonDocument>.Filter.Eq("vagtId", vagter.VagtId);
            var update = Builders<BsonDocument>.Update.Inc("antal", -1);

            var result = vagterKollektion.UpdateOne(filter, update);

            fordeling.InsertOne(vagterDocument);
        }

        public void CreatePlan (Vagter nyvagt)
        {
            BsonDocument vagterDocument = new BsonDocument
            {
                { "dato", nyvagt.Dato },
                { "lokation", nyvagt.Lokation },
                { "rangering", nyvagt.Rangering },
                { "type", nyvagt.Type },
                { "antal", nyvagt.Antal },
                { "start", nyvagt.Start },
                { "slut", nyvagt.Slut },
                { "beskrivelse", nyvagt.Beskrivelse },
                { "vagtId", nyvagt.VagtId },
            };

            vagterKollektion.InsertOne(vagterDocument);
        }

        public void UpdatePlan(Vagter vagter)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("vagtId", vagter.VagtId);
            var update = Builders<BsonDocument>.Update

            .Set("dato", vagter.Dato)
            .Set("lokation", vagter.Lokation)
            .Set("rangering", vagter.Rangering)
            .Set("type", vagter.Type)
            .Set("antal", vagter.Antal)
            .Set("beskrivelse", vagter.Beskrivelse)
            .Set("start", vagter.Start)
            .Set("slut", vagter.Slut);

            var updateResult = vagterKollektion.UpdateOne(filter, update);

            if (updateResult.ModifiedCount == 0)
            {
                Console.WriteLine("Opdatering kunne ikke gennemføres da værdien er nul.");
            }
        }
        public async Task DeleteVagt(int vagtId)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("vagtId", vagtId);
            await vagterKollektion.DeleteOneAsync(filter);
        }

        public Vagter[] GetAllVagter()
        {
            var result = vagterKollektion.Find(new BsonDocument()).ToList();

            List<Vagter> vagtere = new List<Vagter>();

            foreach (var doc in result)
            {
                Vagter vagter = new Vagter()
                {
                    VagtId = doc.Contains("vagtId") ? doc["vagtId"].AsInt32 : 0,
                    Dato = doc.Contains("dato") && doc["dato"] != BsonNull.Value ? doc["dato"].AsString : null,
                    Lokation = doc.Contains("lokation") && doc["lokation"] != BsonNull.Value ? doc["lokation"].AsString : null,
                    Rangering = doc.Contains("rangering") && doc["rangering"] != BsonNull.Value ? doc["rangering"].AsInt32 : 0,
                    Type = doc.Contains("type") && doc["type"] != BsonNull.Value ? doc["type"].AsString : null,
                    Antal = doc.Contains("antal") && doc["antal"] != BsonNull.Value ? doc["antal"].AsInt32 : 0,
                    Start = doc.Contains("start") && doc["start"] != BsonNull.Value ? doc["start"].AsString : null,
                    Slut = doc.Contains("slut") && doc["slut"] != BsonNull.Value ? doc["slut"].AsString : null,
                    Beskrivelse = doc.Contains("beskrivelse") && doc["beskrivelse"] != BsonNull.Value ? doc["beskrivelse"].AsString : null,
                };
                vagtere.Add(vagter);

            }

            return vagtere.ToArray();
        }
    }
}