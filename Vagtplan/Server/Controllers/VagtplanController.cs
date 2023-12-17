using Microsoft.AspNetCore.Mvc;
using Musikfestival.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Musikfestival.Repositories;

[ApiController]
public class VagtplanController : ControllerBase
{

    private IVagter vagtRepo; // Vi opretter et privat felt "vagtRepo" af typen IVagter i controllerklassen. Det bruges til at gemme implemeneteringer.

    public VagtplanController(IVagter vagtRepo) // En offentlig konstruktør for VagtplanController klassen.. Den tager en parameter IBruger ved navn vagtRepo.
    {
        this.vagtRepo = vagtRepo; // Vi udfører afhængighedsindsprøjtning ved at tildele vagtRepo-feltet en implementering af IVagter.
        // Dette giver VagtplanController mulighed for at samarbejde med vagtrelaterede operationer uden at være bundet til en specifik implementering.
    }

    [HttpGet]
    [Route("/api/vagter")]
    public IEnumerable<Vagter> Get() // Vi kalder metoden Get fra vagtRepo, som er en instans af IVagter.
    {
        return vagtRepo.GetAllVagter(); // Dette metodekald vil initiere GetAllVagter(); metoden fra vores klasse som implementerer IVagter, altså vores VagtplanRepository.
    }

    [HttpGet]
    [Route("fordeling")]
    public IEnumerable<MineVagter> GetMineVagter() // Ligeledes denne Get metode vil initiere GetMineVagter fra vores repository.
    {
        return vagtRepo.GetMineVagter();
    }

    [HttpPut]
    [Route("/api/vagter")]
    // Kalder UpdatePlan-metoden på vagtRepo
    // og giver den Vagter-objektet (vagter) som parameter.
    public void UpdatePlan(Vagter vagter) // Metoden forventer en instans af Vagter, vagter.
    {
       vagtRepo.UpdatePlan(vagter);
    }

    [HttpPost]
    [Route("fordeling")]
    public void TakeVagt(Vagter vagter)
    {
        vagtRepo.TakeVagt(vagter);
    }

    // HTTP POST-metode til at oprette en ny vagtplan
    // Kommentar: Metoden kalder CreatePlan-metoden på vagtRepo med det medsendte Vagter-objekt.
    [HttpPost]
    [Route("/api/vagter")]
    public void CreatePlan(Vagter nyvagt)
    {
        vagtRepo.CreatePlan(nyvagt);
    }

    // HTTP DELETE-metode til at slette en vagt
    // Kommentar: Metoden kalder DeleteVagt-metoden på vagtRepo med det medsendte vagtId.
    [HttpDelete]
    [Route("/api/vagter/{vagtId}")]
    public void DeleteVagt(int vagtId)
    {
        vagtRepo.DeleteVagt(vagtId);
    }
}