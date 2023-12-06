using MongoDB.Driver;
using Musikfestival.Repositories;
using Musikfestival.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Musikfestival.Repositories
{
    public class BrugerinfoRepository : IBruger
    {
        private readonly IMongoCollection<Bruger> _Bruger;

        public BrugerinfoRepository()
        {
            MongoClient client = new MongoClient(
                @"mongodb+srv://test:124365@musikfestivalcluster.1xtpep0.mongodb.net/");
            IMongoDatabase database = client.GetDatabase("BrugerDB");
            _Bruger = database.GetCollection<Bruger>("bruger");
        }

        public async Task<IEnumerable<Bruger>> GetAllBruger()
        {
            return await _Bruger.Find(_ => true).ToListAsync();
        }
    }
}
