using Microsoft.AspNetCore.Mvc;
using Project.DTOs.MediaContentDTOs;
using Project.Exceptions;
using Project.Models.Enumerations;
using Project.Services;

namespace Project.Controller;

[Route("api/[controller]")]
[ApiController]
public class MovieController : ControllerBase
{
    private readonly MediaContentService _mediaContentService;

    public MovieController(MediaContentService mediaContentService)
    {
        _mediaContentService = mediaContentService;
    }

    [HttpGet("All")]
    public async Task<IActionResult> GetAllMoviesFrontEnd()
    {
        try
        {
            var result = await _mediaContentService.GetAllMoviesFrontEnd();
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("AllDetailed")]
    public async Task<IActionResult> GetAllMoviesDetailed()
    {
        try
        {
            var result = await _mediaContentService.GetAllMoviesDetailed();
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("Add")]
    public async Task<IActionResult> AddMovie([FromBody] CreateMovieDTO movieDto)
    {
        try
        {
            await _mediaContentService.AddMovie(movieDto);
            return Created(string.Empty, "Movie added successfully.");
        }
        catch (InvalidGenreException ex)
        {
            return BadRequest($"Invalid genre: {ex.Message}");
        }
        catch (StreamingServiceNotFoundException ex)
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
        catch (AddDataFailedException ex)
        {
            return StatusCode(500, "Failed to add movie. Please try again.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Unexpected error: {ex.Message}");
        }
    }

}