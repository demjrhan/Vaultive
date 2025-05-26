using Microsoft.AspNetCore.Mvc;
using Project.DTOs.MediaContentDTOs;
using Project.Exceptions;
using Project.Services;

namespace Project.Controller;


[Route("api/[controller]")]
[ApiController]
public class ShortFilmController : ControllerBase
{
    private readonly MediaContentService _mediaContentService;

    public ShortFilmController(MediaContentService mediaContentService)
    {
        _mediaContentService = mediaContentService;
    }
    
     [HttpGet("Get/{shortFilmId:int}")]
    public async Task<IActionResult> GetShortFilmWithGivenIdAsync(int shortFilmId)
    {
        try
        {
            var result = await _mediaContentService.GetShortFilmWithGivenIdAsync(shortFilmId);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (ShortFilmDoesNotExistsException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Unexpected error: {ex.Message}");
        }
    }
    
    [HttpGet("AllDetailed")]
    public async Task<IActionResult> GetAllShortFilmsDetailedAsync()
    {
        try
        {
            var result = await _mediaContentService.GetAllShortFilmsDetailedAsync();
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("Add")]
    public async Task<IActionResult> AddShortFilmAsync([FromBody] CreateShortFilmDTO dto)
    {
        try
        {
            await _mediaContentService.AddShortFilmAsync(dto);
            return Created(string.Empty, "Short film added successfully.");
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
    public async Task<IActionResult> UpdateShortFilmAsync(int shortFilmId, [FromBody] UpdateShortFilmDTO dto)
    {
        try
        {
            await _mediaContentService.UpdateShortFilmWithGivenIdAsync(shortFilmId, dto);
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