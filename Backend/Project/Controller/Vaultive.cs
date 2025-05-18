using Microsoft.AspNetCore.Mvc;
using Project.Context;
using Project.DTOs;
using Project.Exceptions;
using Project.Models;
using Project.Models.Enumerations;
using Project.Repositories;
using Project.Services;

namespace Project.Controller;

[Route("[controller]")]
[ApiController]
public class Vaultive : ControllerBase
{
    private readonly MasterContext _context;
    private readonly UserService _userService;
    private readonly MovieService _movieService;

    public Vaultive(MasterContext context, UserService userService, MovieService movieService)
    {
        _context = context;
        _userService = userService;
        _movieService = movieService;
    }


    [HttpGet("GetUserWithSubscriptions/{userId}")]
    public async Task<IActionResult> GetUserWithSubscriptions(int userId)
    {
        try
        {
            var result = await _userService.GetUserWithSubscriptions(userId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
       
    }
    [HttpGet("GetUserWithWatchHistory/{userId}")]
    public async Task<IActionResult> GetUserWithWatchHistory(int userId)
    {
        try
        {
            var result = await _userService.GetUserWithWatchHistory(userId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
        
    }
    
    [HttpGet("GetMoviesWithGivenGenre/{genre}")]

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
    
    [HttpGet("GetAllMovies/")]
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


    [HttpPost("AddUser")]
    public async Task<IActionResult> AddUser([FromBody] CreateUserDTO user)
    {
        try
        {
            await _userService.AddUserAsync(user);
            return Ok("User added successfully.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("DeleteUser/{userId}")]
    public async Task<IActionResult> DeleteUser(int userId)
    {
        try
        {
            await _userService.DeleteUserAsync(userId);
            return Ok($"User with ID {userId} deleted.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpPut("UpdateUser")]
    public async Task<IActionResult> UpdateUser([FromBody] UpdateUserDTO updatedUser)
    {
        try
        {
            await _userService.UpdateUserAsync(updatedUser);
            return Ok($"User with ID {updatedUser.Id} updated successfully.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

}