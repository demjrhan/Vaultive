namespace Project.DTOs.MediaContentDTOs;

public class MovieResponseDTO
{
    public MediaContentDetailedResponseDTO MediaContentDetailedResponse { get; set; }
    public ICollection<string> Genres { get; set; }
    
    
}