using Microsoft.AspNetCore.Mvc;
using Musikfestival.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Musikfestival.Repositories;

[ApiController]
[Route("/api/bruger")]
public class BrugerController : ControllerBase
{

    private IBruger brugerRepo;

    public BrugerController(IBruger brugerRepo)
    {
        this.brugerRepo = brugerRepo;
    }

    [HttpPut]
    public void Update(Bruger bruger)
    {
        brugerRepo.Update(bruger);
    }

    [HttpGet]
    public IEnumerable<Bruger> Get()
    {
        return brugerRepo.GetAllBrugere();
    }

    [HttpPost]
    public async Task<ActionResult<Bruger>> AuthenticateUser(Bruger bruger)
    {
        try
        {
            Bruger authenticatedUser = await brugerRepo.AuthenticateUserAsync(bruger.Username, bruger.Password);
            return Ok(authenticatedUser);
        }
        catch (Exception ex)
        {
            return BadRequest($"Error: {ex.Message}");
        }
    }
}