namespace Project.DTOs.MediaContentDTOs;

public class MovieResponseDTO
{
    public MediaContentDetailedResponseDTO MediaContentDetailedResponse { get; set; }
    public List<string> Genres { get; set; }
    
    
}