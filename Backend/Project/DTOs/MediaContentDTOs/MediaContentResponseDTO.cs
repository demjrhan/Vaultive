
using Project.DTOs.ReviewDTOs;
using Project.DTOs.StreamingServiceDTOs;

namespace Project.DTOs.MediaContentDTOs;

public class MediaContentResponseDTO
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime ReleaseDate { get; set; }
    public string OriginalLanguage { get; set; } = null!;
    public string Country { get; set; } = null!;
    public int Duration { get; set; }
    public string YoutubeTrailerURL { get; set; }  = null!;
    public string? PosterImageName { get; set; }
    
    public ICollection<StreamingServiceResponseDTO> StreamingServices { get; set; } = new HashSet<StreamingServiceResponseDTO>();

    public ICollection<ReviewResponseDTO> Reviews { get; set; } = new HashSet<ReviewResponseDTO>();

}