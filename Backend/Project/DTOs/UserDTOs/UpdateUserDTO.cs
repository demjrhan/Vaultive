using Project.Models.Enumerations;

namespace Project.DTOs.UserDTOs;

public class UpdateUserDTO
{
    public int Id { get; set; }
    public string? Firstname { get; set; }
    public string? Lastname { get; set; }
    public string Nickname { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Country { get; set; } = null!;
    public Status Status { get; set; }
}