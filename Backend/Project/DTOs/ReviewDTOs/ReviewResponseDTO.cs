using Project.DTOs.UserDTOs;

namespace Project.DTOs.ReviewDTOs;

public class ReviewResponseDTO
{
    public double Rating { get; set; }
    public string Comment { get; set; } = null!;

    public UserResponseDTO User { get; set; }
}