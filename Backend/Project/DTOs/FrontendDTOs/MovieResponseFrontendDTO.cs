namespace Project.DTOs.FrontendDTOs;

public class MovieResponseFrontendDTO
{
    public MediaContentFrontendResponseDTO MediaContent { get; set; }
    public List<string> Genres { get; set; }
}