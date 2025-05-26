namespace Project.DTOs.MediaContentDTOs;

public class CreateDocumentaryDTO
{
    public CreateMediaContentDTO MediaContent { get; set; } = null!;
    public ICollection<string> Topics { get; set; } = new List<string>();
}