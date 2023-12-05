using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Musikfestival.Shared.Models
{
    public class Bruger
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; } = "";

        public string username { get; set; } = string.Empty;

        public string password { get; set; } = string.Empty;

        public string type { get; set; } = string.Empty;

        public string navn { get; set; } = string.Empty;

        public string adresse { get; set; } = string.Empty;

        public string email { get; set; } = string.Empty;

        public int tlf { get; set; } = 0;

        public string beskrivelse { get; set; } = string.Empty;

    }
}
