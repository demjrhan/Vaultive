namespace Project.Models;

public class SubtitleOption
{

    public ICollection<string> Languages { get; set; } = new List<string>();
    
    public int MediaId { get; set; }

    public MediaContent MediaContent { get; set; } = null!;
}
