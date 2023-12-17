using MongoDB.Bson;
using MongoDB.Driver;
using Musikfestival.Repositories;
using Musikfestival.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Musikfestival.Repositories
{
    public class VagtplanRepository : IVagter // VagtPlanRepository klassen implementerer vores interface IVagter
    {
        private const string connectionString = "mongodb+srv://test:124365@musikfestivalcluster.1xtpep0.mongodb.net/"; // Vores connection string. Gemmes i variablen "connectionString"

        MongoClient dbClient; // Vi implementerer MongoDB client.
        IMongoDatabase database; // Dette repræsenterer brug af en MongoDB database
        IMongoCollection<BsonDocument> vagterKollektion; // Vi vil gerne have adgang til vores kollektion "vagterKollektion"
        IMongoCollection<BsonDocument> fordeling; // Og vi vil gerne have adgang til vores kollektion "fordeling"

        public VagtplanRepository() // En instans af VagtPlanRepository klassen
        {
            dbClient = new MongoClient(connectionString); // Forbindelsesoplysningerne gemmes i en variabel dbClient
            database = dbClient.GetDatabase("VagterDB"); // Valg af MongoDB database = GetDatabase bruger vores Database "VagterDB"
            vagterKollektion = database.GetCollection<BsonDocument>("vagter"); // Vi får fat i de respektive kollektioner.. "vagter" og "fordeling" 
            fordeling = database.GetCollection<BsonDocument>("fordeling"); // Vi gemmer dem i to variabler.. Hhv vagterKollektion ( navnet valgte vi, da det gør det nemmere at læse blandt de mange objekter med samme navn.
        }

        public void TakeVagt(Vagter vagter) // TakeVagt metoden tager parametrene Vagter vagter.. Vagter er vores modelklasse, og vagter en instans af modelklassen.
        {
            // Check om username allerede findes i "fordeling"
            var existingUsernameInFordeling = fordeling.Find(Builders<BsonDocument>.Filter.And( // I variablen existingUsernameInFordeling, søger vi igennem vores kollektion med metoden .Find, for at lave et filtreringskriterie om at
                // om et match med vagtId og username.
                Builders<BsonDocument>.Filter.Eq("vagtId", vagter.VagtId), // Hvis et dokument allerede indeholder det pågældende vagtId og username.. Hvis dokumentet findes, gemmes det i variablen existingUsernameInFordeling
                Builders<BsonDocument>.Filter.Eq("username", vagter.Username)
            )).FirstOrDefault(); // Returnerer det første dokument der opfylder kriterierne.

            if (existingUsernameInFordeling != null) // Her er der en if statement der siger at: Hvis variablen existingUsernameInFordeling ikke er null, så findes der allerede en vagt med denne bruger, og derved kan dokumentet ikke oprettes.
            {
                // Håndter tilfælde, hvor der allerede er en vagt med det samme username i "fordeling"
                // Du kan kaste en exception eller håndtere det på anden måde afhængigt af din logik
                // Eksempel:
                throw new InvalidOperationException($"Kunne ikke oprette vagt. Der er allerede en vagt med username: {vagter.Username} i fordeling");
            }

            // Check om antal er større end 0
            var vagtFull = vagterKollektion.Find(Builders<BsonDocument>.Filter.Eq("vagtId", vagter.VagtId)).FirstOrDefault();

            // Hvis vagtFull ikke er null:
            if (vagtFull != null)
            {
                // Hernæst tjekker koden om feltet indeholder "antal", hvis "antal" eksisterer hentes dets værdi ved hjælp af .AsInt32.. Hvis feltet ikke eksisterer eller ikke kan -
                // konverteres til en int, sættes værdien af antal til 0.
                int antal = vagtFull.Contains("antal") ? vagtFull["antal"].AsInt32 : 0;

                // Håndter tilfælde, hvor "antal" er 0 - Vi har på vores klient sat table rækken til at lyse rød og miste "Tag vagt" knappen, når antal når 0.
                if (antal <= 0)
                {

                    // Eksempel på at kaste en exception:
                    throw new InvalidOperationException($"Kunne ikke oprette vagt. Antal er 0 for vagt med id: {vagter.VagtId}");
                }
            }

            // Opret dokument i "vagterKollektion"
            BsonDocument vagterDocument = new BsonDocument
    {
                // Vores nøgle-værdi par.. Dette betyder at vi har vores nøgler (felter fra vores database) og hernæst værdierne som er de tilsvarende fra vagter objektet.
        { "vagtId", vagter.VagtId }, 
        { "username", vagter.Username },
        { "dato", vagter.Dato },
        { "lokation", vagter.Lokation },
        { "type", vagter.Type },
        { "start", vagter.Start },
        { "slut", vagter.Slut },
        { "beskrivelse", vagter.Beskrivelse },
        };

            // Opdater antal i "vagterKollektion"
            var filter = Builders<BsonDocument>.Filter.Eq("vagtId", vagter.VagtId); // Her oprettes et filter baseret på vagtId.
            var update = Builders<BsonDocument>.Update.Inc("antal", -1); // Inc operatøren bruges til at formindske værdien af antal. Når der oprettes et dokument vil attributten "antal" falde med 1 da der bliver én færre plads.
            var result = vagterKollektion.UpdateOne(filter, update); // Vi gemmer opdateringen af hhv. Update og Filter i variablen result vha. UpdateOne metoden.

            // Check om opdateringen var succesfuld og antal var større end 0
            if (result.IsAcknowledged && result.ModifiedCount > 0) // Hvis opdateringen i vagterKollektion er OK og antallet 
            {
                // Opret dokument i "fordeling"
                fordeling.InsertOne(vagterDocument);
            }
            else
            {
                // Håndter tilfælde, hvor antal er 0 eller opdateringen ikke lykkedes
                // Du kan kaste en exception eller håndtere det på anden måde afhængigt af din logik
                // Eksempel:
                throw new InvalidOperationException("Kunne ikke oprette vagt i fordeling. Antal er muligvis 0.");
            }
        }


        public void CreatePlan(Vagter nyvagt) // Denne metode skal sende ny data ind som et dokument i vores kollektion "vagterKollektion"
            // Metoden tager parametrene Vagter nyvagt.. De data som vi skal indsætte i vores kollektion er af attributterne fra vores "Vagter" modelklasse, vi kalder instansen for nyvagt.
        {
            BsonDocument vagtDocument = new BsonDocument // Vi laver en instans af BsonDocument, og kalder det vagtDocument.
            {
                // Egenskaberne fra modelklassen blier manipuleret og sat ind i vores JSON dokument 
                // Vores nøgle-værdi par.. Dette betyder at vi har vores nøgler (felter fra vores database) og hernæst værdierne som er de tilsvarende fra nyvagt objektet.
                { "dato", nyvagt.Dato }, 
                { "lokation", nyvagt.Lokation },
                { "rangering", nyvagt.Rangering },
                { "type", nyvagt.Type },
                { "antal", nyvagt.Antal },
                { "start", nyvagt.Start },
                { "slut", nyvagt.Slut },
                { "beskrivelse", nyvagt.Beskrivelse },
                { "vagtId", nyvagt.VagtId },
            };

            vagterKollektion.InsertOne(vagtDocument); // Her fuldføres vores forespørgsel, og vores vagtDocument bliver sat ind i vores vagterKollektion
        }

        public void UpdatePlan(Vagter vagter) // Denne metode bruger vi til at opdatere instansen vagter fra Vagter. 
        {
            var filter = Builders<BsonDocument>.Filter.Eq("vagtId", vagter.VagtId); // Dette filter sørger for at opdateringen kun gør sig gældende på det tilsvarende dokument der stemmer overens med VagtId egenskab.
            var update = Builders<BsonDocument>.Update // Variablen update tager denne opdateringsoperation af BsonDocument.. Så vi kan opdatere vores dokument ved hjælp af variablen.

                // Vi opdaterer vores nøgler med de nye værdier ud fra vagter objektet.
            .Set("dato", vagter.Dato)
            .Set("lokation", vagter.Lokation)
            .Set("rangering", vagter.Rangering)
            .Set("type", vagter.Type)
            .Set("antal", vagter.Antal)
            .Set("beskrivelse", vagter.Beskrivelse)
            .Set("start", vagter.Start)
            .Set("slut", vagter.Slut);

            var updateResult = vagterKollektion.UpdateOne(filter, update); // Vi opdaterer kollektionen med metoden UpdateOne, og giver den parametrene filter og update, så vi nu gør brug af vores filtreringskriterier fra ovenstående.
        }
        public async Task DeleteVagt(int vagtId) // Vores delete metode, tager paremeter int vagtId
        {
            // Vi kører et filter baseret på vagtId
            var filter = Builders<BsonDocument>.Filter.Eq("vagtId", vagtId); // Dette filter kræver at vagtId feltets værdi er det samme som det givne vagtId
            await vagterKollektion.DeleteOneAsync(filter); // Anvender filteret på vagtKollektion og sletter dokumentet hvis id matcher kriterierne.
        }

        public MineVagter[] GetMineVagter() // Denne metode håndterer dataudtræk for modelklassen MineVagter, heri definerer vi hvilke data vi skal hente.
        {
            var result = fordeling.Find(new BsonDocument()).ToList(); // De dokumenter som vi finder i vores "fordeling" kollektion bliver gemt i variablen result.

            List<MineVagter> minevagter = new List<MineVagter>(); // Vi opretter en instans af MineVagter som en liste vi kan ændre i.

            foreach (var doc in result) // For hvert dokument i result skal der oprettes et objekt 
            {
                MineVagter vagter = new MineVagter() // Her definerer vi objektet, det er en instans af MineVagter, vi kalder det for vagter.
                {
                    VagtId = doc.Contains("vagtId") ? doc["vagtId"].AsInt32 : 0, // doc.Contains betyder at den attribut som der skal ændres skal indeholde ("navn") som er i JSON format.
                    Username = doc.Contains("username") && doc["username"] != BsonNull.Value ? doc["username"].AsString : null, // i paranteserne skriver vi hvad vores data præcist hedder fra vores JSON dokument i MongoDB
                    Dato = doc.Contains("dato") && doc["dato"] != BsonNull.Value ? doc["dato"].AsString : null, // !=BsonNull.Value betyder at dokumentet ikke må være null.
                    Lokation = doc.Contains("lokation") && doc["lokation"] != BsonNull.Value ? doc["lokation"].AsString : null, // Vi tjekker om dokumentet indeholder egenskab "lokation", den returnerer true hvis ja.
                    Type = doc.Contains("type") && doc["type"] != BsonNull.Value ? doc["type"].AsString : null, // .AsString = vi henter værdien af feltet som typen string.
                    Start = doc.Contains("start") && doc["start"] != BsonNull.Value ? doc["start"].AsString : null, // As.String : null betyder at hvis dokumentet ikke indeholder enten korrekt navn på egenskab eller er null, så sættes værdien som null og det har vi sagt at den ikke må være !=BsonNullValue
                    Slut = doc.Contains("slut") && doc["slut"] != BsonNull.Value ? doc["slut"].AsString : null,
                    Beskrivelse = doc.Contains("beskrivelse") && doc["beskrivelse"] != BsonNull.Value ? doc["beskrivelse"].AsString : null,
                };
                minevagter.Add(vagter); // Vi tilføjer de ønskede data som er vores instans "vagter" af modelklassen MineVagter til vores array "minevagter" 

            }

            return minevagter.ToArray(); // Vi returnerer så svaret i arrayet.
        }

        public Vagter[] GetAllVagter() // Denne metode håndterer dataudtræk for modelklassen Vagter, heri definerer vi hvilke data vi skal hente.
        {
            var result = vagterKollektion.Find(new BsonDocument()).ToList();

            List<Vagter> vagtere = new List<Vagter>();

            foreach (var doc in result)
            {
                Vagter vagter = new Vagter()
                {
                    VagtId = doc.Contains("vagtId") ? doc["vagtId"].AsInt32 : 0, // Hvis feltet ikke eksisterer eller værdien af feltet ikke kan hentes, sættes "vagtId" til 0.
                    Dato = doc.Contains("dato") && doc["dato"] != BsonNull.Value ? doc["dato"].AsString : null,
                    Lokation = doc.Contains("lokation") && doc["lokation"] != BsonNull.Value ? doc["lokation"].AsString : null,
                    Rangering = doc.Contains("rangering") && doc["rangering"] != BsonNull.Value ? doc["rangering"].AsInt32 : 0,
                    Type = doc.Contains("type") && doc["type"] != BsonNull.Value ? doc["type"].AsString : null,
                    Antal = doc.Contains("antal") && doc["antal"] != BsonNull.Value ? doc["antal"].AsInt32 : 0,
                    Start = doc.Contains("start") && doc["start"] != BsonNull.Value ? doc["start"].AsString : null,
                    Slut = doc.Contains("slut") && doc["slut"] != BsonNull.Value ? doc["slut"].AsString : null,
                    Beskrivelse = doc.Contains("beskrivelse") && doc["beskrivelse"] != BsonNull.Value ? doc["beskrivelse"].AsString : null,
                };
                vagtere.Add(vagter);

            }

            return vagtere.ToArray();
        }
    }
}