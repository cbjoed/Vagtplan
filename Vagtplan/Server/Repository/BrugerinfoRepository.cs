using MongoDB.Bson;
using MongoDB.Driver;
using Musikfestival.Repositories;
using Musikfestival.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Musikfestival.Repositories
{
    public class BrugerinfoRepository : IBruger // BrugerinfoRepository klassen implementerer vores interface IBruger
    {
        private const string connectionString = "mongodb+srv://test:124365@musikfestivalcluster.1xtpep0.mongodb.net/";

        MongoClient dbClient;
        IMongoDatabase database;
        IMongoCollection<BsonDocument> brugerKollektion;

        public BrugerinfoRepository()
        {
            dbClient = new MongoClient(connectionString);
            database = dbClient.GetDatabase("BrugerDB");
            brugerKollektion = database.GetCollection<BsonDocument>("bruger");
        }

        public void Update(Bruger bruger) // Denne metode bruger vi til at opdatere instansen bruger fra Bruger. 
        {
            var filter = Builders<BsonDocument>.Filter.Eq("username", bruger.Username); // Vi har et filter på vores dokument.. Filtreringskriteriet er at feltet username skal stemme overens med egenskaben Username fra Bruger.
            var update = Builders<BsonDocument>.Update // I denne variabel "update" lagrer vi opdateringsoperationen .Update

            // vores nøgle-værd par hvor felterne ud fra vores dokument får tilskrevet en ny værdi ud fra attributterne i vores Bruger modelklasse.
            .Set("navn", bruger.Navn)
            .Set("adresse", bruger.Adresse)
            .Set("email", bruger.Email)
            .Set("tlf", bruger.Tlf)
            .Set("beskrivelse", bruger.Beskrivelse);

            var updateResult = brugerKollektion.UpdateOne(filter, update); // Vi opdaterer vores kollektion med de manipulerede data.
        }

        public Bruger[] GetAllBrugere() // Vi gør brug af GetAllBrugere(); metoden som giver os et array af Bruger objekter.
        {
            var result = brugerKollektion.Find(new BsonDocument()).ToList(); // I variablen result gemmer vi resultatet af brugerKollektion.Find og føjer det til listen.

            List<Bruger> brugere = new List<Bruger>(); // Vi opretter en instans af Bruger og kalder den brugere, dette er en array liste.

            foreach (var doc in result) //For hvert dokument ud fra hvad vi fandt frem til igennem brugerKollekton.Find som gemmes i result.
            {
                Bruger bruger = new Bruger() // Vi laver en instans af Bruger og kalder objektet for "bruger" - det skal vi bruge til bagefter at føje vores dokumenter med egenskaber fra Bruger klassen, til vores array liste.
                {
                    Username = doc.Contains("username") && doc["username"] != BsonNull.Value ? doc["username"].AsString : null,
                    Password = doc.Contains("password") && doc["password"] != BsonNull.Value ? doc["password"].AsString : null,
                    Type = doc.Contains("type") && doc["type"] != BsonNull.Value ? doc["type"].AsString : null,
                    Navn = doc.Contains("navn") && doc["navn"] != BsonNull.Value ? doc["navn"].AsString : null,
                    Adresse = doc.Contains("adresse") && doc["adresse"] != BsonNull.Value ? doc["adresse"].AsString : null,
                    Email = doc.Contains("email") && doc["email"] != BsonNull.Value ? doc["email"].AsString : null,
                    Tlf = doc.Contains("tlf") && doc["tlf"] != BsonNull.Value ? doc["tlf"].AsInt32 : 0,
                    Beskrivelse = doc.Contains("beskrivelse") && doc["beskrivelse"] != BsonNull.Value ? doc["beskrivelse"].AsString : null,

                };
                brugere.Add(bruger); // Vores instans brugere laver en .Add med argument bruger ( det tilsvarende dokument der matcher med de nøgleværdi par kriterier vi opsatte ovenover.

            }

            return brugere.ToArray(); // Returnerer data til vores array "brugere"
        }
        public async Task<Bruger> AuthenticateUserAsync(string username, string password)
        {
            // Find dokumentet i brugerKollektion, hvor både brugernavn og adgangskode matcher de angivne værdier
            var result = await brugerKollektion.Find(
                Builders<BsonDocument>.Filter.Eq("username", username) &
                Builders<BsonDocument>.Filter.Eq("password", password))
                .FirstOrDefaultAsync();

            if (result != null)
            {
                // Opret et Bruger-objekt baseret på dataene fra det fundne dokument
                Bruger authenticatedUser = new Bruger()
                {
                    Username = result.Contains("username") ? result["username"].AsString : null,
                    Password = result.Contains("password") ? result["password"].AsString : null,
                    Type = result.Contains("type") ? result["type"].AsString : null,
                };

                return authenticatedUser; // Returner det autentificerede Bruger-objekt
            }

            return null; // Ingen matchende bruger fundet
        }
    }
}