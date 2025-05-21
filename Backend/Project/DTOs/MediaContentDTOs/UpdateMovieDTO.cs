namespace Project.DTOs.MediaContentDTOs;

public class UpdateMovieDTO
{
    
    public UpdateMediaContentDTO MediaContent { get; set; } = new();

    public HashSet<string> Genres { get; set; }  = new();

}