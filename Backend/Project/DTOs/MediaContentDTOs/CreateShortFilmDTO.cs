namespace Project.DTOs.MediaContentDTOs;

public class CreateShortFilmDTO
{
    public CreateMediaContentDTO MediaContent { get; set; }
    public ICollection<string> Genres { get; set; } 
    public string SchoolName { get; set; } = null!;

}