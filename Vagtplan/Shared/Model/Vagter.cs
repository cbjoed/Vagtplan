using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Musikfestival.Shared.Models
{
    public class Vagter
    {
        [BsonId]
        [BsonRepresentationAttribute(BsonType.ObjectId)]
        public ObjectId Id { get; set; }
        public string Lokation { get; set; } = String.Empty;
        public int Rangering { get; set; } = 0;
        public string Type { get; set; } = String.Empty;
        public int Antal { get; set; } = 0;
        public DateTime Start { get; set; } = DateTime.Now;
        public DateTime Slut { get; set; } = DateTime.Now;
        public string Beskrivelse { get; set; } = String.Empty;
    }
}


