namespace Project.DTOs.UserDTOs;

public class UpdateUserDTO
{
    public string? Firstname { get; set; }
    public string? Lastname { get; set; }
    public string Nickname { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Country { get; set; } = null!;
    public string Status { get; set; } = null!;
}