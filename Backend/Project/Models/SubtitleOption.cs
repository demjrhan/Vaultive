namespace Project.Models;

public class SubtitleOption
{

    public ICollection<string> Languages { get; set; } = new List<string>();

    public string MediaTitle { get; set; } = null!;
    public MediaContent MediaContent { get; set; } = null!;
}
