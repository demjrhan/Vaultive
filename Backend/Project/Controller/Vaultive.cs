using Microsoft.AspNetCore.Mvc;
using Project.Context;
using Project.Exceptions;
using Project.Repositories;
using Project.Services;

namespace Project.Controller;

[Route("[controller]")]
[ApiController]
public class Vaultive : ControllerBase
{
    private readonly MasterContext _context;
    private readonly UserService _userService;

    public Vaultive(MasterContext context, UserService userService)
    {
        _context = context;
        _userService = userService;
    }


    [HttpGet("GetUserWithSubscriptions/{userId}")]
    public async Task<IActionResult> GetUserWithSubscriptions(int userId)
    {
        try
        {
            var result = await _userService.GetUserWithSubscriptions(userId);
            return Ok(result);
        }
        catch (UserNotFoundException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (SubscriptionsNotFoundException ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpGet("GetUserWithWatchHistory/{userId}")]
    public async Task<IActionResult> GetUserWithWatchHistory(int userId)
    {
        try
        {
            var result = await _userService.GetUserWithWatchHistory(userId);
            return Ok(result);
        }
        catch (UserNotFoundException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (SubscriptionsNotFoundException ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
}