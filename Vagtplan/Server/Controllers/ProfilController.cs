using Microsoft.AspNetCore.Mvc;
using Musikfestival.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Musikfestival.Repositories;

[ApiController]
[Route("bruger")]
public class ProfilController : ControllerBase
{

    private IBruger brugerRepo;

    public ProfilController(IBruger brugerRepo)
    {
        this.brugerRepo = brugerRepo;
    }

    /*[HttpPost]
    public void Update(Bruger bruger)
    {
        brugerRepo.Update(bruger);
    }*/

    [HttpGet]
    public IEnumerable<Bruger> Get()
    {
        return brugerRepo.GetAllBrugere();
    }
}