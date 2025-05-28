namespace Project.DTOs.FrontendDTOs;

public class MediaContentFrontendDTO
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string? YoutubeTrailerURL { get; set; }
    public string? PosterImageName { get; set; }
    
    public ICollection<StreamingServiceFrontendDTO> StreamingServices { get; set; } = new List<StreamingServiceFrontendDTO>();

}