namespace Project.DTOs.FrontendDTOs;

public class MovieResponseFrontendDTO
{
    public MediaContentFrontendDTO MediaContent { get; set; }
    public List<string> Genres { get; set; }
}