namespace Project.Controller;

using Microsoft.AspNetCore.Mvc;
using DTOs.MediaContentDTOs;
using Exceptions;
using Services;

[Route("api/[controller]")]
[ApiController]
public class MovieController : ControllerBase
{
    private readonly MediaContentService _mediaContentService;

    public MovieController(MediaContentService mediaContentService)
    {
        _mediaContentService = mediaContentService;
    }

    [HttpGet("Get/{movieId:int}")]
    public async Task<IActionResult> GetMovieWithGivenIdAsync(int movieId)
    {
        try
        {
            var result = await _mediaContentService.GetMovieWithGivenIdAsync(movieId);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (MovieDoesNotExistsException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Unexpected error: {ex.Message}");
        }
    }

    [HttpGet("All")]
    public async Task<IActionResult> GetAllMoviesFrontEndAsync()
    {
        try
        {
            var result = await _mediaContentService.GetAllMoviesFrontEndAsync();
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Unexpected error: {ex.Message}");
        }
    }

    [HttpGet("AllDetailed")]
    public async Task<IActionResult> GetAllMoviesDetailedAsync()
    {
        try
        {
            var result = await _mediaContentService.GetAllMoviesDetailedAsync();
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Unexpected error: {ex.Message}");
        }
    }

    [HttpPost("Add")]
    public async Task<IActionResult> AddMovieAsync([FromBody] CreateMovieDTO movieDto)
    {
        try
        {
            await _mediaContentService.AddMovieAsync(movieDto);
            return Created(string.Empty, "Movie added successfully.");
        }
        catch (ArgumentNullException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (InvalidStateException ex)
        {
            return BadRequest($"Invalid state: {ex.Message}");
        }
        catch (InvalidGenreException ex)
        {
            return BadRequest($"Invalid genre: {ex.Message}");
        }
        catch (NoChangesDetectedException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (StreamingServiceDoesNotExistsException ex)
        {
            return NotFound(ex.Message);
        }
        catch (MediaContentTitleMustBeUniqueException ex)
        {
            return NotFound(ex.Message);
        }
        catch (AtLeastOneGenreMustExistsException ex)
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
    public async Task<IActionResult> UpdateMovieAsync(int movieId, [FromBody] UpdateMovieDTO movieDto)
    {
        try
        {
            await _mediaContentService.UpdateMovieWithGivenIdAsync(movieId, movieDto);
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
        catch (InvalidStateException ex)
        {
            return BadRequest($"Invalid state: {ex.Message}");
        }
        catch (InvalidGenreException ex)
        {
            return BadRequest($"Invalid genre: {ex.Message}");
        }
        catch (MediaContentDoesNotExistsException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (MediaContentTitleMustBeUniqueException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Unexpected error: {ex.Message}");
        }
    }
}