using Microsoft.AspNetCore.Mvc;
using Project.DTOs.SubscriptionDTOs;
using Project.Exceptions;
using Project.Services;

namespace Project.Controller;

[ApiController]
[Route("api/[controller]")]
public class SubscriptionController : ControllerBase
{
    private readonly SubscriptionService _subscriptionService;

    public SubscriptionController(SubscriptionService subscriptionService)
    {
        _subscriptionService = subscriptionService;
    }
    
    [HttpPost("Add")]
    public async Task<IActionResult> AddSubscriptionAsync([FromBody] AddOrRenewSubscriptionDTO addOrRenewSubscriptionDto)
    {
        try
        {
            await _subscriptionService.AddSubscriptionAsync(addOrRenewSubscriptionDto);
            return Ok(
                $"User with id {addOrRenewSubscriptionDto.UserId} subscribed to streaming service with id {addOrRenewSubscriptionDto.StreamingServiceId} successfully.");
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
    [HttpGet("ActiveSubscriptionsOfUser/{userId:int}")]
    public async Task<IActionResult> GetActiveSubscriptionsOfUserIdAsync(int userId)
    {
        try
        {
            var result = await _subscriptionService.GetActiveSubscriptionsOfUserIdAsync(userId);
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
    
    
    [HttpGet("InactiveSubscriptionsOfUser/{userId:int}")]
    public async Task<IActionResult> GetInactiveSubscriptionsOfUserIdAsync(int userId)
    {
        try
        {
            var result = await _subscriptionService.GetInactiveSubscriptionsOfUserIdAsync(userId);
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

    [HttpGet("SubscriptionsWithConfirmations")]
    public async Task<IActionResult> GetAllSubscriptionsWithConfirmations()
    {
        try
        {
            var result = await _subscriptionService.GetAllSubscriptionsWithConfirmations();
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Unexpected error: {ex.Message}");
        }
    }

    [HttpDelete("Delete/{subscriptionId:int}")]
    public async Task<IActionResult> CancelSubscriptionAsync(int subscriptionId)
    {
        try
        {
            await _subscriptionService.CancelSubscriptionWithGivenIdAsync(subscriptionId);
            return Ok(new { message = "Subscription cancelled successfully."});
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
    
}
