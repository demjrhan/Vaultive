namespace Project.DTOs.MediaContentDTOs;

public class UpdateShortFilmDTO
{
    public UpdateMediaContentDTO MediaContent { get; set; } = new();
    public List<string> Genres { get; set; } = new();
    public string SchoolName { get; set; } = null!;

}