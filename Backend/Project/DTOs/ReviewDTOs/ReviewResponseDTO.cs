using Project.DTOs.UserDTOs;

namespace Project.DTOs.ReviewDTOs;

public class ReviewResponseDTO
{
    public int Id { get; set; }
    public string MediaTitle { get; set; } = null!;
    public double Rating { get; set; }
    public string? Comment { get; set; }
    public string? Username { get; set; }
    public string? WatchedOn { get; set; }
}