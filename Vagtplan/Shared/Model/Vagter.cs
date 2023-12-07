using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Musikfestival.Shared.Models
{
    public class Vagter
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }

        public string Lokation { get; set; } = String.Empty;

        public int Rangering { get; set; } = 0;

        public string Type { get; set; } = String.Empty;

        public int Antal { get; set; } = 0;

        public DateTime Start { get; set; } = DateTime.MinValue;

        public DateTime Slut { get; set; } = DateTime.MinValue;

        public string Beskrivelse { get; set; } = String.Empty;

    }
}
