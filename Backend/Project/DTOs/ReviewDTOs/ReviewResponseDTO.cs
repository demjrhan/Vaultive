namespace Project.DTOs.ReviewDTOs;

public class ReviewResponseDTO
{
    public int Id { get; set; }
    public string MediaTitle { get; set; } = null!;
    public string? Comment { get; set; }
    public string? Nickname { get; set; }
    public DateOnly WatchedOn { get; set; }
}