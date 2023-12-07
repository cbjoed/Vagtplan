using Microsoft.AspNetCore.Mvc;
using Musikfestival.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Musikfestival.Repositories;

[ApiController]
[Route("/api/vagter")]
public class VagtplanController : ControllerBase
{

    private IVagter vagtRepo;

    public VagtplanController(IVagter vagtRepo)
    {
        this.vagtRepo = vagtRepo;
    }

    [HttpGet]
    public IEnumerable<Vagter> Get()
    {
        return vagtRepo.GetAllVagter();
    }
}