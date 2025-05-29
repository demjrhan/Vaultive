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
    
    [HttpGet("Search")]
    public async Task<IActionResult> SearchMediaContentAsync(string text)
    {
        try
        {
            var result = await _mediaContentService.GetMediaContentWithGivenTextAsync(text);
            return Ok(result);
        }
        catch (ArgumentNullException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (NoMediaContentFoundException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Unexpected error: {ex.Message}");
        }
    }
    
    [HttpDelete("Delete/{mediaId:int}")]
    public async Task<IActionResult> DeleteMediaContentAsync(int mediaId)
    {
        try
        {
            await _mediaContentService.DeleteMediaContentWithGivenIdAsync(mediaId);
            return Ok("Media Content deleted successfully.");
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
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

    [HttpPut("ChangeState/{mediaId:int}")]
    public async Task<IActionResult> ChangeStateOfMediaContentAsync(int mediaId, string state)
    {
        try
        {
            await _mediaContentService.ChangeStateOfMediaContentAsync(mediaId,state);
            return Ok($"Media Content's state changed to {state} successfully.");
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (InvalidStateException ex)
        {
            return BadRequest($"Invalid state: {ex.Message}");
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
    
    [HttpPut("Archive/{mediaId:int}")]
    public async Task<IActionResult> ArchiveMediaContentAsync(int mediaId)
    {
        try
        {
            await _mediaContentService.ArchiveMediaContentAsync(mediaId);
            return Ok($"Media Content's state changed to Archived successfully.");
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (InvalidStateException ex)
        {
            return BadRequest($"Invalid state: {ex.Message}");
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
    [HttpPut("Publish/{mediaId:int}")]
    public async Task<IActionResult> PublishMediaContentAsync(int mediaId)
    {
        try
        {
            await _mediaContentService.PublishMediaContentAsync(mediaId);
            return Ok($"Media Content's state changed to Published successfully.");
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (InvalidStateException ex)
        {
            return BadRequest($"Invalid state: {ex.Message}");
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