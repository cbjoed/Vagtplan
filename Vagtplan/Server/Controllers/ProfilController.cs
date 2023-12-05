using Microsoft.AspNetCore.Mvc;
using Musikfestival.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Musikfestival.Repositories;

[ApiController]
[Route("api/bruger")]
public class BookingsController : ControllerBase
{
    private readonly IBruger _repository;

    public BookingsController(IBruger repository)
    {
        _repository = repository;
    }

    // Hent alle Bruger
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Bruger>>> GetBruger()
    {
        try
        {
            IEnumerable<Bruger> bruger = await _repository.GetAllBruger();
            return Ok(bruger);
        }
        catch (System.Exception ex)
        {
            return StatusCode(500, $"En fejl opstod ved hentning af Bruger data. Fejl: {ex.Message}");
        }
    }


}