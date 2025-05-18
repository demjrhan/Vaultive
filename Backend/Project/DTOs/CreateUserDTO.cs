using Project.Models.Enumerations;

namespace Project.DTOs;

public class CreateUserDTO
{
    public string? Firstname { get; set; }
    public string? Lastname { get; set; }
    public string Nickname { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Country { get; set; } = null!;
    public Status Status { get; set; }
}