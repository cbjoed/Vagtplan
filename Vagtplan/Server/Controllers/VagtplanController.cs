using Microsoft.AspNetCore.Mvc;
using Musikfestival.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Musikfestival.Repositories;

[ApiController]
[Route("api/Vagtplan")]
public class VagtplanController : ControllerBase
{
    private readonly IVagter _repository;

    public VagtplanController(IVagter repository)
    {
        _repository = repository;
    }

    // Hent alle Vagter
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Vagter>>> GetVagter()
    {
        try
        {
            IEnumerable<Vagter> vagter = await _repository.GetAllVagter();
            return Ok(vagter);
        }
        catch (System.Exception ex)
        {
            return StatusCode(500, $"En fejl opstod ved hentning af Bruger data. Fejl: {ex.Message}");
        }
    }


}