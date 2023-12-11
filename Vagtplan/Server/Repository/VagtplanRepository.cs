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
        IMongoCollection<BsonDocument> vagter;

        public VagtplanRepository()
        {
            dbClient = new MongoClient(connectionString);
            database = dbClient.GetDatabase("VagterDB");
            vagter = database.GetCollection<BsonDocument>("vagter");
        }

        public void UpdatePlan(Vagter vagter)
        {
            BsonDocument vagterDocument = new BsonDocument
            {
                { "dato", vagter.Dato },
                { "lokation", vagter.Lokation },
                { "rangering", vagter.Rangering },
                { "type", vagter.Type },
                { "antal", vagter.Antal },
                { "start", vagter.Start },
                { "slut", vagter.Slut },
                { "beskrivelse", vagter.Beskrivelse },
                { "vagtId", vagter.VagtId },
            };
        }

        public Vagter[] GetAllVagter()
        {
            var result = vagter.Find(new BsonDocument()).ToList();

            List<Vagter> vagtere = new List<Vagter>();

            foreach (var doc in result)
            {
                Vagter vagter = new Vagter()
                {
                    Id = doc.Contains("_id") ? doc["_id"].AsObjectId : ObjectId.Empty,
                    Lokation = doc.Contains("lokation") && doc["lokation"] != BsonNull.Value ? doc["lokation"].AsString : null,
                    Rangering = doc.Contains("rangering") && doc["rangering"] != BsonNull.Value ? doc["rangering"].AsInt32 : 0,
                    Type = doc.Contains("type") && doc["type"] != BsonNull.Value ? doc["type"].AsString : null,
                    Antal = doc.Contains("antal") && doc["antal"] != BsonNull.Value ? doc["antal"].AsInt32 : 0,
                    Start = doc.Contains("start") && doc["start"] != BsonNull.Value ? doc["start"].AsDateTime : DateTime.MinValue,
                    Slut = doc.Contains("slut") && doc["slut"] != BsonNull.Value ? doc["slut"].AsDateTime : DateTime.MinValue,
                    Beskrivelse = doc.Contains("beskrivelse") && doc["beskrivelse"] != BsonNull.Value ? doc["beskrivelse"].AsString : null,

                };
                vagtere.Add(vagter);

            }

            return vagtere.ToArray();
        }
    }
}