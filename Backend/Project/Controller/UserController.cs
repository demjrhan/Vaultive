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
        catch (UserDoesNotExistsException ex)
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
            await _userService.ReviewAsync(addReviewDto);
            return Ok($"User {addReviewDto.UserId} added review successfully.");
        }

        catch (DuplicateReviewException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (WatchHistoryDoesNotExistsException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (MediaContentDoesNotExistsException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (UserDoesNotExistsException ex)
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
    public async Task<IActionResult> SubscribeAsync([FromBody] AddOrRenewSubscriptionDTO addOrRenewSubscriptionDto)
    {
        try
        {
            await _userService.SubscribeAsync(addOrRenewSubscriptionDto);
            return Ok(
                $"User with id {addOrRenewSubscriptionDto.UserId} subscribed to streaming service with id {addOrRenewSubscriptionDto.StreamingServiceId} successfully.");
        }

        catch (PaymentFailedException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (StreamingServiceDoesNotExistsException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (UserDoesNotExistsException ex)
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
    [HttpPut("UpdateReview/{userId:int}")]
    public async Task<IActionResult> UpdateReviewAsync(int userId, [FromBody] UpdateReviewDTO dto)
    {
        try
        {
            await _userService.UpdateReviewAsync(userId, dto);
            return Ok(new { message = "Review updated successfully" });
        }
        catch (AccessDeniedException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (ReviewDoesNotExistsException ex)
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
        catch (UserDoesNotExistsException ex)
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

    [HttpDelete("Delete/{userId:int}")]
    public async Task<IActionResult> DeleteUserAsync(int userId)
    {
        try
        {
            await _userService.DeleteUserWithGivenIdAsync(userId);
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
    [HttpDelete("CancelSubscription/{userId:int}/{subscriptionId:int}")]
    public async Task<IActionResult>  CancelSubscriptionAsync(int userId, int subscriptionId)
    {
        try
        {
            await _userService.CancelSubscriptionWithGivenIdAsync(userId, subscriptionId);
            return Ok(new { message = "Subscription deleted successfully." });
        }
        catch (AccessDeniedException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (SubscriptionsDoesNotExistsException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Unexpected error: {ex.Message}");
        }
    }
    [HttpDelete("DeleteReview/{userId:int}/{reviewId:int}")]
    public async Task<IActionResult> DeleteReviewAsync(int userId, int reviewId)
    {
        try
        {
            await _userService.DeleteReviewWithGivenIdAsync(userId, reviewId);
            return Ok(new { message = "Review deleted successfully" });
        }
        catch (AccessDeniedException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (ReviewDoesNotExistsException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Unexpected error: {ex.Message}");
        }
    }
    
}