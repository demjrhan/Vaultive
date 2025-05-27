namespace Project.DTOs.UserDTOs;

public class UserDTO
{
    public int Id { get; set; }
    public string? Firstname { get; set; }
    public string? Lastname { get; set; }
    public string Nickname { get; set; }
    public string Country { get; set; }
    public string Status { get; set; }
}