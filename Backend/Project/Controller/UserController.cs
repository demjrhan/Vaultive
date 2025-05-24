using Microsoft.AspNetCore.Mvc;
using Project.DTOs.ReviewDTOs;
using Project.DTOs.SubscriptionDTOs;
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
            return Created(string.Empty, $"User {userDto.Firstname} added successfully.");
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
            return Ok($"User {userId} deleted successfully.");
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Unexpected error: {ex.Message}");
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
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (UserNotFoundException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Unexpected error: {ex.Message}");
        }
    }

    [HttpGet("Get/{userId:int}")]
    public async Task<IActionResult> GetUserWithGivenIdAsync(int userId)
    {
        try
        {
            var result = await _userService.GetUserWithGivenIdAsync(userId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Unexpected error: {ex.Message}");
        }
    }

    [HttpPut("Watch")]
    public async Task<IActionResult> WatchMediaContentAsync(int userId, int mediaId)
    {
        try
        {
            await _userService.WatchMediaContentAsync(userId, mediaId);
            return Ok($"User {userId} successfully watched media content {mediaId}.");
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (MediaContentDoesNotExistsException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (UserNotFoundException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (MediaContentAlreadyWatchedException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (UserHasNoActiveSubscriptionException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Unexpected error: {ex.Message}");
        }
    }

    [HttpPut("Update")]
    public async Task<IActionResult> UpdateUserAsync([FromBody] UpdateUserDTO userDto)
    {
        try
        {
            await _userService.UpdateUserWithGivenIdAsync(userDto);
            return Ok($"User {userDto.Id} update was successful.");
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
        catch (Exception ex)
        {
            return StatusCode(500, $"Unexpected error: {ex.Message}");
        }
    }
    
    [HttpPost("Review")]
    public async Task<IActionResult> AddReviewAsync([FromBody] AddReviewDTO addReviewDto)
    {
        try
        {
            await _userService.AddReviewToMediaContentAsync(addReviewDto);
            return Ok($"User {addReviewDto.UserId} added review successfully.");
        }

        catch (DuplicateReviewException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (WatchHistoryNotFoundException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (MediaContentDoesNotExistsException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (UserNotFoundException ex)
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
        catch (Exception ex)
        {
            return StatusCode(500, $"Unexpected error: {ex.Message}");
        }
    }
    
    [HttpPost("Subscribe")]
    public async Task<IActionResult> SubscribeAsync([FromBody] AddSubscriptionDTO addSubscriptionDto)
    {
        try
        {
            await _userService.SubscribeAsync(addSubscriptionDto);
            return Ok($"User with id {addSubscriptionDto.UserId} subscribed to streaming service with id {addSubscriptionDto.StreamingServiceId} successfully.");
        }

        catch (StreamingServiceNotFoundException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (UserNotFoundException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (SubscriptionAlreadyExistsException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Unexpected error: {ex.Message}");
        }
    }

    
}