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
        private readonly IMongoCollection<Vagter> _Vagter;

        public VagtplanRepository()
        {
            MongoClient client = new MongoClient(
                @"mongodb+srv://test:124365@musikfestivalcluster.1xtpep0.mongodb.net/");
            IMongoDatabase database = client.GetDatabase("VagterDB");
            _Vagter = database.GetCollection<Vagter>("vagter");
        }

        public async Task<IEnumerable<Vagter>> GetAllVagter()
        {
            return await _Vagter.Find(_ => true).ToListAsync();
        }

    }
    public Vagter[] GetAllVagt()
    {
        var result = vagt.Find(new BsonDocument()).ToList();

        List<Vagter> brugere = new List<Vagter>();

        foreach (var doc in result)
        {
            Vagter bruger = new Vagter()
            {
                _Id = doc.Contains("_id") ? doc["_id"].AsObjectId : ObjectId.Empty,
                Type = doc.Contains("type") && doc["type"] != BsonNull.Value ? doc["type"].AsString : null,
                Lokation = doc.Contains("lokation") && doc["lokation"] != BsonNull.Value ? doc["lokation"].AsString : null,
                Rangering = doc.Contains("rangering") && doc["rangering"] != BsonNull.Value ? doc["rangering"].AsInt : null,
                Antal = doc.Contains("antal") && doc["antal"] != BsonNull.Value ? doc["antal"].AsInt : null,
                Start = doc.Contains("start") && doc["start"] != BsonNull.Value ? doc["start"].AsDateTime : null,
                Slut = doc.Contains("slut") && doc["slut"] != BsonNull.Value ? doc["slut"].AsDateTime : null,
                Beskrivelse = doc.Contains("beskrivelse") && doc["beskrivelse"] != BsonNull.Value ? doc["beskrivelse"].AsString : null,

            };
            Vagter.Add(vagt);

        }

        return vagter.ToArray();
    }
}
