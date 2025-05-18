namespace Project.DTOs.ReviewDTOs;

public class AddReviewDTO
{
    public int UserId { get; set; }
    public string MediaTitle { get; set; } = null!;
    public double Rating { get; set; }
    public string? Comment { get; set; }
}