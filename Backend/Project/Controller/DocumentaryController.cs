using Microsoft.AspNetCore.Mvc;
using Project.DTOs.MediaContentDTOs;
using Project.Exceptions;
using Project.Services;

namespace Project.Controller;


[Route("api/[controller]")]
[ApiController]
public class DocumentaryController : ControllerBase
{
    private readonly MediaContentService _mediaContentService;

    public DocumentaryController(MediaContentService mediaContentService)
    {
        _mediaContentService = mediaContentService;
    }
    
    [HttpGet("AllDetailed")]
    public async Task<IActionResult> GetAllDocumentariesDetailedAsync()
    {
        try
        {
            var result = await _mediaContentService.GetAllDocumentariesDetailedAsync();
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("Add")]
    public async Task<IActionResult> AddDocumentaryAsync([FromBody] CreateDocumentaryDTO dto)
    {
        try
        {
            await _mediaContentService.AddDocumentaryAsync(dto);
            return Created(string.Empty, "Documentary added successfully.");
        }
        catch (ArgumentNullException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (InvalidTopicException ex)
        {
            return BadRequest($"Invalid topic: {ex.Message}");
        }
        catch (StreamingServiceDoesNotExistsException ex)
        {
            return NotFound(ex.Message);
        }
        catch (MediaContentTitleMustBeUniqueException ex)
        {
            return NotFound(ex.Message);
        }
        catch (AtLeastOneTopicMustExistsException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (AtLeastOneOptionMustExistsException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Unexpected error: {ex.Message}");
        }
    }
    
    [HttpPut("Update")]
    public async Task<IActionResult> UpdateDocumentaryAsync(int documentaryId, [FromBody] UpdateDocumentaryDTO dto)
    {
        try
        {
            await _mediaContentService.UpdateDocumentaryWithGivenIdAsync(documentaryId, dto);
            return Ok("Content update was successful.");
        }
        catch (ArgumentNullException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (AtLeastOneTopicMustExistsException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (NoChangesDetectedException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (StreamingServiceDoesNotExistsException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (MediaContentDoesNotExistsException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (MediaContentTitleMustBeUniqueException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (InvalidTopicException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Unexpected error: {ex.Message}");
        }
    }

}