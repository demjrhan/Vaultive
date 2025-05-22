using Microsoft.AspNetCore.Mvc;
using Project.DTOs.UserDTOs;
using Project.Exceptions;
using Project.Services;

namespace Project.Controller;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly UserService _userService;

    public UserController(UserService userService)
    {
        _userService = userService;
    }

    [HttpPost("Add")]
    public async Task<IActionResult> AddMovieAsync([FromBody] CreateUserDTO userDto)
    {
        try
        {
            await _userService.AddUserAsync(userDto);
            return Created(string.Empty, "User added successfully.");
        }
        catch (InvalidUserStatusException ex)
        {
            return BadRequest($"Invalid status: {ex.Message}");
        }
        catch (NicknameAlreadyExistsException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (EmailAlreadyExistsException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (ArgumentNullException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (AddDataFailedException ex)
        {
            return StatusCode(500, $"Failed to add movie. Please try again. {ex.Message}");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Unexpected error: {ex.Message}");
        }
    }

    [HttpDelete("Remove/{userId:int}")]
    public async Task<IActionResult> RemoveUserAsync(int userId)
    {
        try
        {
            await _userService.RemoveUserWithGivenIdAsync(userId);
            return Ok("User deleted successfully.");
        }
        catch (RemoveDataFailedException ex)
        {
            return StatusCode(500, $"Failed to remove user. Please try again. {ex.Message}");
        }
       
    }
    
    [HttpGet("AllDetailed")]
    public async Task<IActionResult> GetAllUsersDetailedAsync()
    {
        try
        {
            var result = await _userService.GetAllUsersDetailedAsync();
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
