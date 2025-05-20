namespace Project.Models;

public class AudioOption
{

    public ICollection<string>? Languages { get; set; } = new HashSet<string>();

    public int MediaId { get; set; }

    public MediaContent MediaContent { get; set; } = null!;
}
