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

    [HttpPut]
    [Route("/api/vagter")]
    public void UpdatePlan(Vagter vagter)
    {
       vagtRepo.UpdatePlan(vagter);
    }

    [HttpPost]
    [Route("fordeling")]
    public void AddVagt(Vagter vagter)
    {
        vagtRepo.AddVagt(vagter);
    }

    [HttpPost]
    [Route("/api/vagter")]
    public void CreatePlan(Vagter nyvagt)
    {
        vagtRepo.CreatePlan(nyvagt);
    }

    [HttpDelete]
    [Route("/api/vagter/{vagtId}")]
    public void DeleteVagt(int vagtId)
    {
        vagtRepo.DeleteVagt(vagtId);
    }

}