using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace Musikfestival.Shared.Models
{
    public class MineVagter
    {
        [BsonId]
        [BsonRepresentationAttribute(BsonType.ObjectId)]
        public int VagtId { get; set; }
        public string Username { get; set; } = String.Empty;
        public string Dato { get; set; } = String.Empty;
        public string Lokation { get; set; } = String.Empty;
        public string Type { get; set; } = String.Empty;
        public string Start { get; set; } = String.Empty;
        public string Slut { get; set; } = String.Empty;
        public string Beskrivelse { get; set; } = String.Empty;
    }
}


