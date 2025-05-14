namespace Project.Models;

public class SubtitleOption
{

    public ICollection<SubtitleLanguage> SubtitleLanguages { get; set; } = new HashSet<SubtitleLanguage>();

    public string MediaTitle { get; set; } = null!;
    public MediaContent MediaContent { get; set; } = null!;
}
