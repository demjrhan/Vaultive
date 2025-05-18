using Microsoft.AspNetCore.Mvc;
using Project.DTOs;
using Project.Exceptions;
using Project.Services;

namespace Project.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly UserService _userService;

    public UserController(UserService userService)
    {
        _userService = userService;
    }

    [HttpGet("WithSubscriptions/{userId}")]
    public async Task<IActionResult> GetUserWithSubscriptions(int userId)
    {
        try
        {
            var result = await _userService.GetUserWithSubscriptions(userId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("WithWatchHistory/{userId}")]
    public async Task<IActionResult> GetUserWithWatchHistory(int userId)
    {
        try
        {
            var result = await _userService.GetUserWithWatchHistory(userId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("Add")]
    public async Task<IActionResult> AddUser([FromBody] CreateUserDTO user)
    {
        try
        {
            await _userService.AddUserAsync(user);
            return Ok("User added successfully.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("Update")]
    public async Task<IActionResult> UpdateUser([FromBody] UpdateUserDTO updatedUser)
    {
        try
        {
            await _userService.UpdateUserAsync(updatedUser);
            return Ok($"User with ID {updatedUser.Id} updated successfully.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("Delete/{userId}")]
    public async Task<IActionResult> DeleteUser(int userId)
    {
        try
        {
            await _userService.DeleteUserAsync(userId);
            return Ok($"User with ID {userId} deleted.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
