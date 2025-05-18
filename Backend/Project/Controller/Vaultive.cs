using Microsoft.AspNetCore.Mvc;
using Project.Context;
using Project.Services;

namespace Project.Controller;

[Route("[controller]")]
[ApiController]
public class Vaultive : ControllerBase
{
    private readonly MasterContext _context;

    public Vaultive(MasterContext context, UserService userService, MovieService movieService)
    {
        _context = context;
    }


    [HttpGet]
    public IActionResult Info()
    {
        return Ok("Vaultive API is running.");
    }


}