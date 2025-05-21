namespace Project.DTOs.FrontendDTOs;

public class MediaContentFrontendDTO
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string YoutubeTrailerURL { get; set; }  = null!;
    public string? PosterImageName { get; set; }
    
    public ICollection<StreamingServiceResponseFrontendDTO> StreamingServices { get; set; } = new HashSet<StreamingServiceResponseFrontendDTO>();

    public ICollection<ReviewResponseFrontendDTO> Reviews { get; set; } = new HashSet<ReviewResponseFrontendDTO>();

}