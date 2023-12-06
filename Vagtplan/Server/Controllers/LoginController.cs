using Microsoft.AspNetCore.Mvc;
using Musikfestival.Repositories;
using Musikfestival.Shared.Models;


[ApiController]
[Route("/bruger")]
public class LoginController : ControllerBase
{
    private readonly BrugerinfoRepository _repository;

    public LoginController(BrugerinfoRepository repository)
    {
        _repository = repository;
    }

    [HttpPost]
    public async Task<ActionResult<Bruger>> AuthenticateUser(Login login)
    {
        try
        {
            Bruger authenticatedUser = await _repository.AuthenticateUserAsync(login.Username, login.Password);
            return Ok(authenticatedUser);
        }
        catch (Exception ex)
        {
            return BadRequest($"Error: {ex.Message}");
        }
    }
}