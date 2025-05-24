namespace Project.DTOs.MediaContentDTOs;

public class MovieDTO
{
    public MediaContentDetailedDTO MediaContentDetailed { get; set; }
    public ICollection<string> Genres { get; set; }
    
    
}