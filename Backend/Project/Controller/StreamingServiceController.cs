using Microsoft.AspNetCore.Mvc;
using Project.Services;

namespace Project.Controller;


[Route("api/[controller]")]
[ApiController]
public class StreamingServiceController : ControllerBase
{
    
    private readonly StreamingServiceService _streamingServiceService;

    public StreamingServiceController(StreamingServiceService streamingServiceService)
    {
        _streamingServiceService = streamingServiceService;
    }
    [HttpGet("AllDetailed")]
    public async Task<IActionResult> GetAllStreamingServicesAsync()
    {
        try
        {
            var result = await _streamingServiceService.GetAllStreamingServicesDetailedAsync();
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}