using Microsoft.AspNetCore.Mvc;
using Musikfestival.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Musikfestival.Repositories;

[ApiController]
public class VagtplanController : ControllerBase
{

    private IVagter vagtRepo;

    public VagtplanController(IVagter vagtRepo)
    {
        this.vagtRepo = vagtRepo;
    }

    [HttpGet]
    [Route("/api/vagter")]
    public IEnumerable<Vagter> Get()
    {
        return vagtRepo.GetAllVagter();
    }

    [HttpPost]
    [Route("fordeling")]
    public void AddTilFordeling(Vagter vagter)
    {
        vagtRepo.AddTilFordeling(vagter);
    }
}