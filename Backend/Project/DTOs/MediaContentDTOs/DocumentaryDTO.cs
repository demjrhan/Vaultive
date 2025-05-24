namespace Project.DTOs.MediaContentDTOs;

public class DocumentaryDTO
{
    public MediaContentDetailedResponseDTO MediaContentDetailedResponse { get; set; }
    public ICollection<string> Topics { get; set; }
}