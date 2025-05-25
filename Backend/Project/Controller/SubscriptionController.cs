using Microsoft.AspNetCore.Mvc;
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
        catch (UserNotFoundException ex)
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

    [HttpDelete("Remove/{subscriptionId:int}")]
    public async Task<IActionResult> RemoveSubscriptionAsync(int subscriptionId)
    {
        try
        {
            await _subscriptionService.RemoveSubscriptionWithGivenIdAsync(subscriptionId);
            return Ok(new { message = "Subscription deleted successfully."});
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (SubscriptionsNotFoundException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Unexpected error: {ex.Message}");
        }
    }
  
}
