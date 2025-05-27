using Microsoft.AspNetCore.Mvc;
using Project.Exceptions;
using Project.Services;

namespace Project.Controller;

[Route("api/[controller]")]
[ApiController]
public class WatchHistoryController : ControllerBase
{
    private readonly WatchHistoryService _watchHistoryService;
    public WatchHistoryController(WatchHistoryService watchHistoryService)
    {
        _watchHistoryService = watchHistoryService;
    }


    [HttpGet("All")]
    public async Task<IActionResult> GetAllWatchHistoriesAsync()
    {
        try
        {
            var result = await _watchHistoryService.GetAllWatchHistoriesAsync();
            return Ok(result);
        } 
        catch (Exception ex)
        {
            return StatusCode(500, $"Unexpected error: {ex.Message}");
        }
    }
    
    
    [HttpGet("AllDetailed")]
    public async Task<IActionResult> GetAllWatchHistoriesDetailedAsync()
    {
        try
        {
            var result = await _watchHistoryService.GetAllWatchHistoriesDetailedAsync();
            return Ok(result);
        } 
        catch (Exception ex)
        {
            return StatusCode(500, $"Unexpected error: {ex.Message}");
        }
    }
    [HttpGet("UsersWatchHistories/{userId:int}")]
    public async Task<IActionResult> GetWatchHistoriesOfUserAsync(int userId)
    {
        try
        {
            var result = await _watchHistoryService.GetWatchHistoriesOfUserAsync(userId);
            return Ok(result);
        } 
        catch (Exception ex)
        {
            return StatusCode(500, $"Unexpected error: {ex.Message}");
        }
    }
    [HttpGet("CanUserWriteReview/{userId:int}/{mediaId:int}")]
    public async Task<IActionResult> CanUserWriteReviewAsync(int userId, int mediaId)
    {
        try
        {
            var result = await _watchHistoryService.CanUserWriteReviewAsync(userId, mediaId);
            return Ok(result);
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
        catch (Exception ex)
        {
            return StatusCode(500, $"Unexpected error: {ex.Message}");
        }
    }
}