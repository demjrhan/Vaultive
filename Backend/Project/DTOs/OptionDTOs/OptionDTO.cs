namespace Project.DTOs.OptionDTOs;

public class OptionDTO
{
    public ICollection<string>? Languages { get; set; } = new HashSet<string>();

}