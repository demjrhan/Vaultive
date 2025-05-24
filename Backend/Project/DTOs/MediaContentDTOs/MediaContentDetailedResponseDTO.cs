
using Project.DTOs.OptionDTOs;
using Project.DTOs.ReviewDTOs;
using Project.DTOs.StreamingServiceDTOs;

namespace Project.DTOs.MediaContentDTOs;

public class MediaContentDetailedResponseDTO
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateOnly ReleaseDate { get; set; }
    public string OriginalLanguage { get; set; } = null!;
    public string Country { get; set; } = null!;
    public int Duration { get; set; }
    public string YoutubeTrailerURL { get; set; }  = null!;
    public string? PosterImageName { get; set; }
    
    public OptionDTO? SubtitleOption { get; set; }
    
    public OptionDTO? AudioOption { get; set; }
    
    public ICollection<StreamingServiceDTO> StreamingServices { get; set; } = new List<StreamingServiceDTO>();

    public ICollection<ReviewDTO> Reviews { get; set; } = new List<ReviewDTO>();

}