﻿using Microsoft.AspNetCore.Mvc;
using Project.DTOs.StreamingServiceDTOs;
using Project.Exceptions;
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

    
    [HttpGet("Get/{streamingServiceId:int}")]
    public async Task<IActionResult> GetStreamingServiceWithGivenIdAsync(int streamingServiceId)
    {
        try
        {
            var result = await _streamingServiceService.GetAllStreamingServicesWithGivenIdAsync(streamingServiceId);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (StreamingServiceDoesNotExistsException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Unexpected error: {ex.Message}");
        }
    }
    
    [HttpGet("Get/{mediaTitle}")]
    public async Task<IActionResult> GetStreamingServiceWithGivenIdAsync(string mediaTitle)
    {
        try
        {
            var result = await _streamingServiceService.GetSupportedStreamingServicesOfMediaContent(mediaTitle);
            return Ok(result);
        }
        catch (ArgumentNullException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Unexpected error: {ex.Message}");
        }
    }
    
    [HttpGet("All")]
    public async Task<IActionResult> GetAllStreamingServicesAsync()
    {
        try
        {
            var result = await _streamingServiceService.GetAllStreamingServicesAsync();
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    [HttpGet("AllDetailed")]
    public async Task<IActionResult> GetAllStreamingServicesDetailedAsync()
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
    [HttpGet("GetMostPopularStreamingService")]
    public async Task<IActionResult> GetMostPopularStreamingServiceAsync()
    {
        try
        {
            var result = await _streamingServiceService.GetMostPopularStreamingServiceAsync();
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (NoStreamingServiceExistsException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Unexpected error: {ex.Message}");
        }
    }
    [HttpPost("Add")]
    public async Task<IActionResult> AddStreamingServiceAsync([FromBody] AddStreamingServiceDTO streamingServiceDto)
    {
        try
        {
            await _streamingServiceService.AddStreamingServiceAsync(streamingServiceDto);
            return Created(string.Empty, "Streaming Service added successfully.");
        }
        catch (ArgumentNullException ex)
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
    
    [HttpPut("Update")]
    public async Task<IActionResult> UpdateStreamingServicesAsync(int streamingServiceId, [FromBody] UpdateStreamingServiceDTO streamingServiceDto)
    {
        try
        {
            await _streamingServiceService.UpdateStreamingServiceWithGivenIdAsync(streamingServiceId, streamingServiceDto);
            return Ok("Streaming Service update was successful.");
        }
        catch (ArgumentNullException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (StreamingServiceDoesNotExistsException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (StreamingServiceNameMustBeUniqueException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Unexpected error: {ex.Message}");
        }
    }
    
    [HttpDelete("Delete/{streamingServiceId:int}")]
    public async Task<IActionResult> DeleteStreamingServiceAsync(int streamingServiceId)
    {
        try
        {
            await _streamingServiceService.DeleteStreamingServiceWithGivenIdAsync(streamingServiceId);
            return Ok("Streaming service deleted successfully.");
        }
        catch (StreamingServiceDoesNotExistsException ex)
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
}