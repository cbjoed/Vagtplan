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
}
