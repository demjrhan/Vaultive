using Microsoft.AspNetCore.Mvc;
using Project.Models.Enumerations;
using Project.Services;

namespace Project.Controller;

[Route("api/[controller]")]
[ApiController]
public class MovieController : ControllerBase
{
    private readonly MovieService _movieService;

    public MovieController(MovieService movieService)
    {
        _movieService = movieService;
    }

    [HttpGet("All")]
    public async Task<IActionResult> GetAllMovies()
    {
        try
        {
            var result = await _movieService.GetAllMovies();
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("ByGenre/{genre}")]
    public async Task<IActionResult> GetMoviesWithGivenGenre(string genre)
    {
        try
        {
            if (!Enum.TryParse<Genre>(genre, true, out var parsedGenre))
                return BadRequest($"Invalid genre: {genre}");

            var result = await _movieService.GetMoviesWithGivenGenre(parsedGenre);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}