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
    
    [HttpDelete("Remove")]
    public async Task<IActionResult> RemoveMediaContent(int mediaId)
    {
        try
        {
            await _mediaContentService.RemoveMediaContentWithGivenId(mediaId);
            return Created(string.Empty, "Media Content removed successfully.");
        }
        catch (MediaContentDoesNotExistsException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (RemoveDataFailedException ex)
        {
            return StatusCode(500, $"Failed to remove media content. Please try again. {ex.Message}");
        }
       
    }
    
}



    

