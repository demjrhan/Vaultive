using Microsoft.AspNetCore.Mvc;
using Project.DTOs.SubscriptionDTOs;
using Project.Exceptions;
using Project.Models;
using Project.Services;

namespace Project.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SubscriptionController : ControllerBase
{
    private readonly SubscriptionService _subscriptionService;

    public SubscriptionController(SubscriptionService subscriptionService)
    {
        _subscriptionService = subscriptionService;
    }

    [HttpGet("All")]
    public async Task<IActionResult> GetAllSubscriptions()
    {
        try
        {
            var result = await _subscriptionService.GetAllSubscriptionsAsync();
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var result = await _subscriptionService.GetSubscriptionByIdAsync(id);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet("ActiveOfUser/{userId}")]
    public async Task<IActionResult> GetActiveSubscriptionsOfUser(int userId)
    {
        try
        {
            var result = await _subscriptionService.GetActiveSubscriptionsOfUserIdAsync(userId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet("LatestConfirmation")]
    public async Task<IActionResult> GetLatestConfirmationForUser(int userId, int subscriptionId)
    {
        try
        {
            var result = await _subscriptionService.GetLatestConfirmationForUserAsync(userId, subscriptionId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost("Add")]
    public async Task<IActionResult> Add([FromBody] AddSubscriptionDTO subscription)
    {
        try
        {
            await _subscriptionService.AddSubscriptionAsync(subscription);
            return Ok("Subscription added.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }


    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _subscriptionService.DeleteSubscriptionAsync(id);
            return Ok("Subscription deleted.");
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
}
