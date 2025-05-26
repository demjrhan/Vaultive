namespace Project.DTOs.MediaContentDTOs;

public class UpdateDocumentaryDTO
{
    public CreateMediaContentDTO MediaContent { get; set; } =  new();
    public List<string> Topics { get; set; } = new();
}