namespace Project.DTOs.ReviewDTOs;

public class AddReviewDTO
{
    public int UserId { get; set; }
    public string MediaTitle { get; set; } = null!;
    public string Comment { get; set; } = null!;
}