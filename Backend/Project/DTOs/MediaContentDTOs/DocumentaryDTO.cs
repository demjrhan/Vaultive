namespace Project.DTOs.MediaContentDTOs;

public class DocumentaryDTO
{
    public MediaContentDetailedResponseDTO MediaContentDetailedResponse { get; set; }
    public List<string> Topics { get; set; }
}