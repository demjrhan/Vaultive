using Microsoft.AspNetCore.Mvc;
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
    [HttpGet("GetActiveSubscriptionsOfUser/{userId:int}")]
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
        catch (Exception ex)
        {
            return StatusCode(500, $"Unexpected error: {ex.Message}");
        }
    }
  
}
