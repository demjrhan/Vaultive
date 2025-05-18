namespace Project.DTOs.ReviewDTOs;

public class UpdateReviewDTO
{
    public int Id { get; set; }
    public double Rating { get; set; }
    public string? Comment { get; set; }
}