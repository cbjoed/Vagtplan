using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Musikfestival.Shared.Models
{
    public class Bruger
    {
        [BsonId]
        [BsonRepresentationAttribute(BsonType.ObjectId)]
        public ObjectId Id { get; set; }
        public string Username { get; set; } = String.Empty;
        public string Password { get; set; } = String.Empty;
        public string Type { get; set; } = String.Empty;
        public string Navn { get; set; } = String.Empty;
        public string Adresse { get; set; } = String.Empty;
        public string Email { get; set; } = String.Empty;
        public int Tlf { get; set; } = 0;
        public string Beskrivelse { get; set; } = String.Empty;
    }
}


