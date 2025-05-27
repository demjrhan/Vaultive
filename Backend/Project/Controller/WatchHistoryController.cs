using Microsoft.AspNetCore.Mvc;
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
    [HttpGet("UsersWatchHistory{userId:int}")]
    public async Task<IActionResult> GetWatchHistoriesOfUserAsync(int userId)
    {
        try
        {
            var result = await _watchHistoryService.GetWatchHistoriesOfUser(userId);
            return Ok(result);
        } 
        catch (Exception ex)
        {
            return StatusCode(500, $"Unexpected error: {ex.Message}");
        }
    }
    
}