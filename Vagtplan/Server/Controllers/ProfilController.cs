using Microsoft.AspNetCore.Mvc;
using Musikfestival.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Musikfestival.Repositories;

[ApiController]
[Route("/api/bruger")]
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