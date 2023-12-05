using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Musikfestival.Shared.Models
{
    public class Vagter
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; } = "";

        public string lokation { get; set; } = string.Empty;

        public int rangering { get; set; } = 0;

        public string type { get; set; } = string.Empty;

        public int antal { get; set; } = 0;

        public DateTime start { get; set; } = DateTime.MinValue;

        public DateTime slut { get; set; } = DateTime.MinValue;

        public string beskrivelse { get; set; } = string.Empty;

    }
}
