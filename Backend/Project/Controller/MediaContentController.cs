namespace Project.Controller;

using Microsoft.AspNetCore.Mvc;
using Exceptions;
using Services;

[Route("api/[controller]")]
[ApiController]
public class MediaContentController : ControllerBase
{
    private readonly MediaContentService _mediaContentService;

    public MediaContentController(MediaContentService mediaContentService)
    {
        _mediaContentService = mediaContentService;
    }

    [HttpDelete("Remove/{mediaId:int}")]
    public async Task<IActionResult> RemoveMediaContentAsync(int mediaId)
    {
        try
        {
            await _mediaContentService.RemoveMediaContentWithGivenIdAsync(mediaId);
            return Ok("Media Content deleted successfully.");
        }
        catch (MediaContentDoesNotExistsException ex)
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

   

    [HttpGet("Get/{mediaId:int}")]
    public async Task<IActionResult> GetMediaContentWithGivenIdAsync(int mediaId)
    {
        try
        {
            var result = await _mediaContentService.GetMediaContentWithGivenIdAsync(mediaId);
            return Ok(result);
        }
        catch (MediaContentDoesNotExistsException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Unexpected error: {ex.Message}");
        }
    }
}