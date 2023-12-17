using Microsoft.AspNetCore.Mvc;
using Musikfestival.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Musikfestival.Repositories;

[ApiController]
[Route("/api/bruger")] // Ruten til vores kollektion
public class BrugerController : ControllerBase // Arv fra ControllerBase klassen, som er en del af ASP.NET Core MVC framework.
{

    private IBruger brugerRepo; // VI opretter et privat felt "brugerRepo" af typen IBruger i controllerklassen. Det bruges til at gemme implemeneteringer.

    public BrugerController(IBruger brugerRepo) // En offentlig konstruktør for BrugerCOntroller klassen.. Den tager en parameter IBruger ved navn brugerRepo.
    {
        this.brugerRepo = brugerRepo; // Vi udfører afhængighedsindsprøjtning ved at tildele brugerRepo-feltet en implementering af IBruger.
        // Dette giver BrugerController mulighed for at samarbejde med brugerrelaterede operationer uden at være bundet til en specifik implementering.
    }

    [HttpPut]
    public void Update(Bruger bruger) // Denne metode laver vores RESTful HttpPut, metoden forventer en instans "bruger".
    {
        brugerRepo.Update(bruger); // Den pågældende metode fra vores interface IBruger vil udføre en handling til at opdatere argumentet (bruger)
    }
    [HttpGet]
    public IEnumerable<Bruger> Get()
    {
        // Kalder metoden i brugerRepo, der henter alle brugere
        return brugerRepo.GetAllBrugere();
    }

    // HTTP POST-metode til at autentificere en bruger
    [HttpPost]
    public async Task<ActionResult<Bruger>> AuthenticateUser(Bruger bruger)
    {
        try
        {
            // Kalder metoden i brugerRepo til at autentificere brugeren
            Bruger authenticatedUser = await brugerRepo.AuthenticateUserAsync(bruger.Username, bruger.Password);

            // Sender det autentificerede brugerobjekt som OK-respons til klienten
            return Ok(authenticatedUser);
        }
        catch (Exception ex)
        {
            // Sender fejlmeddelelse som BadRequest-respons til klienten
            return BadRequest($"Error: {ex.Message}");
        }
    }
}